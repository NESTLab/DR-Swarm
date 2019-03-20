using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class VisualizationManager : MonoBehaviour
{
    // TODO: See if there is a better data structure for this class, this works for now though

    // Dictionary that associates a robot with an observable set of visualizations
    Dictionary<Robot, BehaviorSubject<HashSet<string>>> robotVisualizations; // TODO: find a better name for this
    
    // Dictionary to associate a visualization's unique name with a observable IVisualization object
    Dictionary<string, BehaviorSubject<IVisualization>> visualizations;

    public string turnOffVisualizationName = null;

    private enum UpdateKind {
        Add,
        Remove
    }

    public VisualizationManager()
    {
        visualizations = new Dictionary<string, BehaviorSubject<IVisualization>>();
        robotVisualizations = new Dictionary<Robot, BehaviorSubject<HashSet<string>>>();
    }

    #region SINGLETON PATTERN
    // Thanks https://answers.unity.com/questions/891380/unity-c-singleton.html
    private static VisualizationManager _instance;
    public static VisualizationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<VisualizationManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("VisualizationManager");
                    _instance = container.AddComponent<VisualizationManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    #region Add/Edit/Remove Visualizations
    public void AddVisualization(string name, IVisualization visualization)
    {

        // Add the visualization to the dictionary unless it's already been added
        if (!visualizations.ContainsKey(name))
        {
            visualizations.Add(name, new BehaviorSubject<IVisualization>(visualization));
            UpdateRobotVisualizations(name, visualization, UpdateKind.Add);
        }
        else
        {
            throw new Exception(String.Format("Cannot add visualization ('{0}'), already exists.", name));
        }
    }

    public void EditVisualization(string name, IVisualization visualization)
    {
        BehaviorSubject<IVisualization> observable;
        if (visualizations.TryGetValue(name, out observable))
        {
            // Compute the set of robots that were added and the set that were removed by the edit
            ISet<Robot> addedRobots = new HashSet<Robot>(visualization.GetRobots());
            addedRobots.ExceptWith(observable.Value.GetRobots());

            ISet<Robot> removedRobots = new HashSet<Robot>(observable.Value.GetRobots());
            removedRobots.ExceptWith(visualization.GetRobots());

            // Update the observable set of visualizations for each added/removed robot
            foreach (Robot robot in addedRobots)
            {
                BehaviorSubject<HashSet<string>> visualizationNamesObs = GetRobotVisualizationSubject(robot);
                HashSet<string> nameSet = visualizationNamesObs.Value;
                nameSet.Add(name);
                visualizationNamesObs.OnNext(nameSet);
            }

            foreach (Robot robot in removedRobots)
            {
                BehaviorSubject<HashSet<string>> visualizationNamesObs = GetRobotVisualizationSubject(robot);
                HashSet<string> nameSet = visualizationNamesObs.Value;
                nameSet.Remove(name);
                visualizationNamesObs.OnNext(nameSet);
                
            }
            

            // Update the visualization subject
            observable.OnNext(visualization);
        }
        else
        {
            throw new Exception(String.Format("Cannot edit visualization ('{0}'), does not exist.", name));
        }
    }

    public void RemoveVisualization(string name)
    {
        // Check if the visualization exists in the dictionary
        BehaviorSubject<IVisualization> observable;
        if (visualizations.TryGetValue(name, out observable))
        {
            // Send the complete event
            observable.OnCompleted();
            
            // Remove the visualization from all the robots it is associated with
            UpdateRobotVisualizations(name, observable.Value, UpdateKind.Remove);

            // Finally remove it from the dictionary
            visualizations.Remove(name);
        }
        else
        {
            throw new Exception(String.Format("Cannot remove visualization ('{0}'), does not exist.", name));
        }
    }
    #endregion

    #region Observable Visualization List
    private void UpdateRobotVisualizations(string name, IVisualization visualization, UpdateKind type)
    {
        // (Add/remove) the visualization for each robot it is associated with
        foreach (Robot robot in visualization.GetRobots())
        {
            BehaviorSubject<HashSet<string>> visualizationNamesObs = GetRobotVisualizationSubject(robot);
            HashSet<string> nameSet = visualizationNamesObs.Value;
            switch (type)
            {
                case UpdateKind.Add:
                    nameSet.Add(name);
                    break;

                case UpdateKind.Remove:
                    nameSet.Remove(name);
                    break;
            }

            // Update the observable set of visualizations associated with this robot
            visualizationNamesObs.OnNext(nameSet);
        }
    }

    private BehaviorSubject<HashSet<string>> GetRobotVisualizationSubject(Robot robot)
    {
        // Get the set of visualization names associated with this robot
        BehaviorSubject<HashSet<string>> visualizationNamesObs;
        if (robotVisualizations.TryGetValue(robot, out visualizationNamesObs))
        {
            return visualizationNamesObs;
        } else
        {
            // If it doesn't exist already, create a new observable set and return it
            visualizationNamesObs = new BehaviorSubject<HashSet<string>>(new HashSet<string>());
            robotVisualizations.Add(robot, visualizationNamesObs);
            return visualizationNamesObs;
        }
    }

    public IObservable<ISet<string>> GetObservableVisualizationsForRobot(Robot robot)
    {
        // Turn the subject of visualizations associated with this robot into an observable
        // This makes it so classes other than VisualizationManager can only subscribe to this set
        // instead of being able to OnNext, OnError, and OnComplete
        return GetRobotVisualizationSubject(robot).AsObservable();
    }
    #endregion

    #region Observable Visualizations
    public Type GetVisualizationType(string name)
    {
        if (visualizations.ContainsKey(name))
        {
            return visualizations[name].Value.GetType();
        }
        else
        {
            throw new Exception(String.Format("Cannot get visualization type ('{0}'), does not exist", name));
        }
    }

    public IObservable<IVisualization> GetObservableVisualization(string name)
    {
        if (visualizations.ContainsKey(name))
        {
            return visualizations[name].AsObservable();
        }
        else
        {
            throw new Exception(String.Format("Cannot get visualization ('{0}'), does not exist", name));
        }
    }
    #endregion

    public void toggleVisualizationFromManager(string visualizationName)
    {
        turnOffVisualizationName = visualizationName;
    }


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
