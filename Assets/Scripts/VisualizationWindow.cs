using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class VisualizationWindow : MonoBehaviour {
    private Robot robot;
    private GameObject canvas;
    private RectTransform container;

    private HashSet<string> visualizationContainers;

    // Use this for initialization
    void Start() {
        canvas = (GameObject)Instantiate(Resources.Load("VisualizationCanvas"), transform);
        container = canvas.transform.Find("Container").GetComponent<RectTransform>();
        visualizationContainers = new HashSet<string>();
        robot = DataManager.Instance.GetRobot(name);

        VisualizationManager.Instance.GetObservableVisualizationsForRobot(robot).Subscribe(VisualizationListUpdated);
    }

    private void VisualizationListUpdated(ISet<string> visualizationSet)
    { 
        HashSet<string> addedVisualizations = new HashSet<string>(visualizationSet);
        addedVisualizations.ExceptWith(visualizationContainers);

        HashSet<string> removedVisualizations = new HashSet<string>(visualizationContainers);
        removedVisualizations.ExceptWith(visualizationSet);

        foreach (string visualizationName in removedVisualizations)
        {
            Destroy(gameObject.GetComponent(visualizationName));
            visualizationContainers.Remove(visualizationName);
        }

        foreach (string visualizationName in addedVisualizations)
        {
            CreateVisualizationContainer(visualizationName);
        }
    }

    private void CreateVisualizationContainer(string visualizationName)
    {
        // TODO: I don't think this is good OO, there may be some pattern to do this better
        Type visualizationType = VisualizationManager.Instance.GetVisualizationType(visualizationName);
        if (visualizationType == typeof(LineGraph))
        {
            LineGraphContainer container = gameObject.AddComponent<LineGraphContainer>();
            container.visualizationName = visualizationName;
            container.container = this.container;
        }
        else if (visualizationType == typeof(PieChart))
        {
            PieChartContainer container = gameObject.AddComponent<PieChartContainer>();
            container.visualizationName = visualizationName;
            container.container = this.container;
        }
        else if (visualizationType == typeof(Indicator))
        {
            IndicatorContainer container = gameObject.AddComponent<IndicatorContainer>();
            container.visualizationName = visualizationName;
            container.container = this.container;
        }
        else if (visualizationType == typeof(BarGraph))
        {
            BarGraphContainer container = gameObject.AddComponent<BarGraphContainer>();
            container.visualizationName = visualizationName;
            container.container = this.container;
        } else
        {
            throw new Exception("Invalid visualization type.");
        }
    }

    // Update is called once per frame
    void Update() {
        canvas.transform.LookAt(GameObject.Find("Camera").transform);

        // Rotate 180 around Y axis, because LookAt points the Z axis at the camera
        // when instead we want it pointing away from the camera
        canvas.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
    }
}
