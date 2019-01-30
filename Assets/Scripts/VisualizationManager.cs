using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class VisualizationManager : MonoBehaviour
{
    // TODO: See if there is a better data structure for this class, this works for now though
    Dictionary<Robot, BehaviorSubject<HashSet<string>>> robotVisualizations; // TODO: find a better name for this
    Dictionary<string, BehaviorSubject<IVisualization>> visualizations;

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
        BehaviorSubject<IVisualization> observable;
        if (visualizations.TryGetValue(name, out observable))
        {
            observable.OnCompleted();

            foreach (Robot robot in observable.Value.GetRobots())
            {
                UpdateRobotVisualizations(name, observable.Value, UpdateKind.Remove);
            }

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
            visualizationNamesObs.OnNext(nameSet);
        }
    }

    private BehaviorSubject<HashSet<string>> GetRobotVisualizationSubject(Robot robot)
    {
        BehaviorSubject<HashSet<string>> visualizationNamesObs;
        if (robotVisualizations.TryGetValue(robot, out visualizationNamesObs))
        {
            return visualizationNamesObs;
        } else
        {
            visualizationNamesObs = new BehaviorSubject<HashSet<string>>(new HashSet<string>());
            robotVisualizations.Add(robot, visualizationNamesObs);
            return visualizationNamesObs;
        }
    }

    public IObservable<ISet<string>> GetObservableVisualizationsForRobot(Robot robot)
    {
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

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
