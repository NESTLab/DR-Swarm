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
    List<string> variables = new List<string>();
    //Dictionary<Robot, float> dataDict = new Dictionary<Robot, float>();
    Dictionary<Robot, Dictionary<string, float>> dataDict = new Dictionary<Robot, Dictionary<string, float>>();

    private Dictionary<Robot, GameObject> barContainers; // this is wrong - need a container for the bars that's connected to the robots
    private Dictionary<Robot, Dictionary<string, GameObject>> bars; // is this what I really want?
    // can I make a prefab that contains an unknown number of bars?

    private Dictionary<string, Color> varColors = new Dictionary<string, Color>(); 
    private float curHVal = 0f;
    private float invphi = 1f / 1.618f; // golden ratio

    // Initialize things
    protected override void Start()
    {
        //TODO: maybe remove
        base.Start();

        barContainers = new Dictionary<Robot, GameObject>();
        bars = new Dictionary<Robot, Dictionary<string, GameObject>>();
    }

    private GameObject GetBarContainer(Robot robot) {
        if (!barContainers.ContainsKey(robot)) { //this is wrong - needs to be a container for the bars
            // TODO: create bar prefab
            //GameObject blankBar = (GameObject)Instantiate(Resources.Load("Bar"), transform);
            GameObject container = new GameObject("barContainer", typeof(Image));
            //blankWedge.transform.SetParent(chartContainer.transform, false);
            // TODO: set parent for bar
            barContainers[robot] = container;
        }

        return barContainers[robot];
    }

    // this might make the bar containers function useless
    private GameObject GetBar(Robot robot, string var) {
        if (!bars.ContainsKey(robot)) {
            // initialize the dictionary
            Dictionary<string, GameObject> barDict = new Dictionary<string, GameObject>();
            bars[robot] = barDict;

            GameObject blankBar = (GameObject)Instantiate(Resources.Load("Bar"), transform);
            blankBar.GetComponent<Image>().color = varColors[var];
            bars[robot][var] = blankBar;
        }
        else if (!bars[robot].ContainsKey(var)) {
            // initialize the single bar
            GameObject blankBar = (GameObject)Instantiate(Resources.Load("Bar"), transform);
            blankBar.GetComponent<Image>().color = varColors[var];
            bars[robot][var] = blankBar;
        }

        return bars[robot][var];
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() {
        float containerSpacing = 0f;
        float barSpacing = 0f;
        float barCount = 0f;
        float containerCount = 0f;

        foreach (Robot r in robots) {
            barCount = 0f;
            GameObject container = GetBarContainer(r);
            // set parent for this
            container.transform.SetParent(transform);

            //container.transform.localScale = Vector3.one;
            //container.transform.localRotation = new Quaternion(0, 0, 0, 0);

            container.GetComponent<Image>().color = Color.white;

            RectTransform t = container.GetComponent<RectTransform>();
            t.anchorMax = new Vector2(0.5f, 0.5f);
            t.anchorMin = new Vector2(0.5f, 0.5f);
            t.pivot = new Vector2(0.5f, 0.5f);
            t.localScale = Vector3.one;
            t.localRotation = new Quaternion(0, 0, 0, 0);
            t.sizeDelta = new Vector2(200f, 200f); // change this eventually
            t.anchoredPosition = new Vector2((containerSpacing + t.rect.width) * containerCount, 0f);


            // now that we have the container, we need to fill it with the bars
            foreach (string var in variables) {
                GameObject bar = GetBar(r, var);

                // put the bar inside the container
                bar.transform.SetParent(container.transform, false);

                // set size
                float value = dataDict[r][var];
                RectTransform tb = bar.GetComponent<RectTransform>();
                tb.sizeDelta = new Vector2(30f, value * 100f); // for now
                tb.anchorMax = new Vector2(0f, 0f);
                tb.anchorMin = new Vector2(0f, 0f);
                tb.pivot = new Vector2(0f, 0f);
                tb.anchoredPosition = new Vector2((barSpacing + tb.rect.width) * barCount, 0f);


                barCount += 1;
            }

            containerCount += 1;
        }
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
    protected override void UpdateData(Dictionary<Robot, Dictionary<string, float>> data)
    {
        // TODO: figure out how to handle maximum value - probably the same as linegraph
        foreach (Robot r in data.Keys) {
            if (!robots.Contains(r)) {
                robots.Add(r);
            }
        }

        Robot robot = robots[0];
        // TODO: do I need to keep track of the variable names too? 
        foreach (string var in data[robot].Keys) {
            if (!variables.Contains(var)) {
                variables.Add(var); 

                // set the color for the new variable
                varColors[var] = Color.HSVToRGB(curHVal, 1, 1);
                curHVal = (curHVal + invphi) % 1.0f;
            }
        }

        // is it really this simple?
        dataDict = data;
    }
}