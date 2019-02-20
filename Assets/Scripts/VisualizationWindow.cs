using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class VisualizationWindow : MonoBehaviour {
    // The robot associated with this visualization window
    private Robot robot;

    // The canvas object of the visualization window, created from a prefab
    private GameObject canvas;

    // The transform of the container, which is a subobject ofthe canvas
    private RectTransform container;

    // A set of names for the visualization containers associated with this window
    private HashSet<string> visualizationNames;

    // Use this for initialization
    void Start() {
        canvas = (GameObject)Instantiate(Resources.Load("VisualizationCanvas"), transform);
        container = canvas.transform.Find("Container").GetComponent<RectTransform>();
        visualizationNames = new HashSet<string>();
        robot = DataManager.Instance.GetRobot(name);

        // Subscribe to the set of visualizations for this robot
        // A callback gets called if a visualization was added or removed for the robot
        VisualizationManager.Instance.GetObservableVisualizationsForRobot(robot).Subscribe(VisualizationListUpdated);
    }

    private void VisualizationListUpdated(ISet<string> visualizationSet)
    { 
        // Compute the set of visualizations added and removed
        HashSet<string> addedVisualizations = new HashSet<string>(visualizationSet);
        addedVisualizations.ExceptWith(visualizationNames);

        HashSet<string> removedVisualizations = new HashSet<string>(visualizationNames);
        removedVisualizations.ExceptWith(visualizationSet);

        // Create and destory containers for added and removed visualizations
        foreach (string visualizationName in removedVisualizations)
        {
            Destroy(container.transform.Find(visualizationName).gameObject);
            visualizationNames.Remove(visualizationName);
        }

        foreach (string visualizationName in addedVisualizations)
        {
            CreateVisualizationContainer(visualizationName);
            visualizationNames.Add(visualizationName);
        }
    }

    private void CreateVisualizationContainer(string visualizationName)
    {
        // Create an empty 2D gameobject for the container to be stored in
        GameObject gameObject = new GameObject(visualizationName, typeof(Image));
        gameObject.transform.SetParent(container);
        gameObject.GetComponent<Image>().color = Color.clear;

        // Set the origin of the container to be in the bottom left
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        transform.sizeDelta = container.GetComponent<RectTransform>().sizeDelta;
        transform.anchorMin = Vector2.zero;
        transform.anchorMax = Vector2.zero;
        transform.pivot = Vector2.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = new Quaternion(0, 0, 0, 0);

        // Create a container based off of the visualization type
        // TODO: Try using "visitor" pattern here
        Type visualizationType = VisualizationManager.Instance.GetVisualizationType(visualizationName);
        if (visualizationType == typeof(LineGraph))
        {
            LineGraphContainer container = gameObject.AddComponent<LineGraphContainer>();
            container.robot = this.robot;
            container.visualizationName = visualizationName;
            container.container = transform;
        }
        else if (visualizationType == typeof(PieChart))
        {
            PieChartContainer container = gameObject.AddComponent<PieChartContainer>();
            container.robot = this.robot;
            container.visualizationName = visualizationName;
            container.container = transform;
        }
        else if (visualizationType == typeof(PieChartMultiVar)) 
        {
            PieChartMultiVarContainer container = gameObject.AddComponent<PieChartMultiVarContainer>();
            container.robot = this.robot;
            container.visualizationName = visualizationName;
            container.container = transform;
        }
        else if (visualizationType == typeof(RangeIndicator))
        {
            RangeIndicatorContainer container = gameObject.AddComponent<RangeIndicatorContainer>();
            container.robot = this.robot;
            container.visualizationName = visualizationName;
            container.container = transform;
        }
        else if (visualizationType == typeof(MapIndicator)) {
            MapIndicatorContainer container = gameObject.AddComponent<MapIndicatorContainer>();
            container.robot = this.robot;
            container.visualizationName = visualizationName;
            container.container = transform;
        }
        else if (visualizationType == typeof(BarGraph))
        {
            BarGraphContainer container = gameObject.AddComponent<BarGraphContainer>();
            container.robot = this.robot;
            container.visualizationName = visualizationName;
            container.container = transform;
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
