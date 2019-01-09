using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// Workflow
// Add Vis:     UI -> Create IVis object -> Add to manager -> Vis added to Robots
// Edit Vis:    UI -> Change IVis object -> Send to manager -> 
// Add To:      UI -> Select robot -> Send to manager -> 

public class VisualizationManager : MonoBehaviour {

    // TODO: See if there is a better data structure for this class, this works for now though
    Dictionary<Robot, List<string>> robotVisualizations; // TODO: find a better name for this
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
            visualizations[name] = new BehaviorSubject<IVisualization>(visualization);

            foreach (Robot robot in visualization.GetRobots())
            {
                if (!robotVisualizations.ContainsKey(robot))
                {
                    robotVisualizations[robot] = new List<string>();
                }

                robotVisualizations[robot].Add(name);
            }
        } else
        {
            throw new Exception(String.Format("Cannot add visualization ('{0}'), already exists.", name));
        }
    }

    public void EditVisualization(string name, IVisualization visualization)
    {
        if (visualizations.ContainsKey(name))
        {
            visualizations[name].OnNext(visualization);
        }
        else
        {
            throw new Exception(String.Format("Cannot edit visualization ('{0}'), does not exist.", name));
        }
    }

    public void RemoveVisualization(string name)
    {
        if (!visualizations.ContainsKey(name))
        {
            visualizations[name].OnCompleted();

            foreach (Robot robot in visualizations[name].Value.GetRobots())
            {
                if (robotVisualizations.ContainsKey(robot))
                {
                    robotVisualizations[robot].Remove(name);
                }
            }
        }
        else
        {
            throw new Exception(String.Format("Cannot remove visualization ('{0}'), does not exist.", name));
        }
    }

    public List<string> GetVisualizationsForRobot(Robot robot)
    {
        return robotVisualizations[robot];
    }

    private IObservable<IVisualization> GetObservableVisualization(string name)
    {
        if (visualizations.ContainsKey(name))
        {
            return visualizations[name].AsObservable();
        } else
        {
            throw new Exception(String.Format("Cannot get visualization ('{0}'), does not exist", name));
        }
    }

    // Use this for initialization
    void Start() {
        visualizations = new Dictionary<string, BehaviorSubject<IVisualization>>();
        robotVisualizations = new Dictionary<Robot, List<string>>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
