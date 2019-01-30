using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

// TODO: add another for BarChartMultiVar
public class BarGraphContainer : VisualizationContainer<BarGraph>
{
    // Instances of VisualizationContainer have access to the container
    // RectTransform container: the RectTransform of the drawable area in the
    // canvas. NOT the same as canvas.GetComponent<RectTransform>()
    List<Robot> robots = new List<Robot>();
    Dictionary<Robot, float> dataDict = new Dictionary<Robot, float>();

    private Dictionary<Robot, GameObject> bars;

    // Initialize things
    protected override void Start()
    {
        //TODO: maybe remove
        base.Start();

        bars = new Dictionary<Robot, GameObject>();
    }

    private GameObject GetBar(Robot robot) {
        if (!bars.ContainsKey(robot)) {
            // TODO: create bar prefab
            GameObject blankBar = (GameObject)Instantiate(Resources.Load("Bar"), transform);
            //blankWedge.transform.SetParent(chartContainer.transform, false);
            // TODO: set parent for bar
            bars[robot] = blankBar;
        }

        return bars[robot];
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() {
        foreach (Robot r in robots) {
            // TODO: fill with stuff
            GameObject bar = GetBar(r);
        }
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
    protected override void UpdateData(Dictionary<Robot, List<float>> data) {
        foreach (Robot r in data.Keys) {
            if (!robots.Contains(r)) {
                robots.Add(r);
            }

            dataDict[r] = data[r][0];
        }
    }
}