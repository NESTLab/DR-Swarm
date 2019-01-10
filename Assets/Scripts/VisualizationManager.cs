using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class VisualizationManager : MonoBehaviour
{

    // TODO: See if there is a better data structure for this class, this works for now though
    Dictionary<Robot, HashSet<string>> robotVisualizations; // TODO: find a better name for this
    Dictionary<string, BehaviorSubject<IVisualization>> visualizations;

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

    public void AddVisualization(string name, IVisualization visualization)
    {
        if (!visualizations.ContainsKey(name))
        {
            visualizations.Add(name, new BehaviorSubject<IVisualization>(visualization));
            UpdateRobotVisualizations(name, visualization);
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
            observable.OnNext(visualization);
            UpdateRobotVisualizations(name, visualization);
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
                if (robotVisualizations.ContainsKey(robot))
                {
                    robotVisualizations[robot].Remove(name);
                }
            }

            visualizations.Remove(name);
        }
        else
        {
            throw new Exception(String.Format("Cannot remove visualization ('{0}'), does not exist.", name));
        }
    }

    public ISet<string> GetVisualizationsForRobot(Robot robot)
    {
        return robotVisualizations[robot];
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

    private void UpdateRobotVisualizations(string name, IVisualization visualization)
    {
        foreach (Robot robot in visualization.GetRobots())
        {
            if (!robotVisualizations.ContainsKey(robot))
            {
                robotVisualizations[robot] = new HashSet<string>();
            }

            robotVisualizations[robot].Add(name);
        }
    }

    // Use this for initialization
    void Start()
    {
        visualizations = new Dictionary<string, BehaviorSubject<IVisualization>>();
        robotVisualizations = new Dictionary<Robot, HashSet<string>>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
