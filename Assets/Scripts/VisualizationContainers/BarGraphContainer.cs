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
    Dictionary<Robot, Dictionary<string, float>> dataDict = new Dictionary<Robot, Dictionary<string, float>>();

    private Dictionary<Robot, GameObject> barContainers; 
    private Dictionary<Robot, Dictionary<string, GameObject>> bars;
    private Dictionary<string, GameObject> legend;

    private Dictionary<string, Color> varColors = new Dictionary<string, Color>(); 
    private float curHVal = 0f;
    private float invphi = 1f / 1.618f; // golden ratio

    private GameObject graphContainer;
    private GameObject legendContainer;

    // Initialize things
    protected override void Start()
    {
        //TODO: maybe remove
        base.Start();

        barContainers = new Dictionary<Robot, GameObject>();
        bars = new Dictionary<Robot, Dictionary<string, GameObject>>();
        legend = new Dictionary<string, GameObject>();

        // set up bar graph container
        graphContainer = new GameObject("BarGraph", typeof(Image));
        graphContainer.transform.SetParent(container.transform, false);
        RectTransform gt = graphContainer.GetComponent<RectTransform>();
        gt.sizeDelta = new Vector2(container.sizeDelta.x, 250f);
        gt.anchorMax = new Vector2(0.5f, 1f);
        gt.anchorMin = new Vector2(0.5f, 1f);
        gt.pivot = new Vector2(0.5f, 1f);
        gt.localScale = Vector3.one;
        gt.localRotation = new Quaternion(0, 0, 0, 0);
        gt.anchoredPosition = Vector2.zero;

        graphContainer.GetComponent<Image>().color = Color.blue;

        // set up legend container
        legendContainer = new GameObject("Legend", typeof(Image));
        legendContainer.transform.SetParent(container.transform, false);
        RectTransform lt = legendContainer.GetComponent<RectTransform>();
        lt.sizeDelta = new Vector2(container.sizeDelta.x, 150f);
        lt.anchorMax = new Vector2(0.5f, 0f);
        lt.anchorMin = new Vector2(0.5f, 0f);
        lt.pivot = new Vector2(0.5f, 0f);
        lt.localScale = Vector3.one;
        lt.localRotation = new Quaternion(0, 0, 0, 0);
        lt.anchoredPosition = Vector2.zero;

        legendContainer.GetComponent<Image>().color = Color.blue;

        // Y axis
        GameObject yaxis = new GameObject("y-axis", typeof(Image));
        yaxis.transform.SetParent(gt, false);
        yaxis.GetComponent<Image>().color = Color.white;
        RectTransform yt = yaxis.GetComponent<RectTransform>();
        yt.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        yt.localScale = Vector3.one;
        yt.sizeDelta = new Vector2(1f, gt.sizeDelta.y); // change this eventually
        yt.anchorMin = Vector2.zero;
        yt.anchorMax = Vector2.zero;
        yt.pivot = Vector2.zero;
        yt.anchoredPosition = Vector2.zero;
        
        // X axis
        GameObject xaxis = new GameObject("x-axis", typeof(Image));
        xaxis.transform.SetParent(gt, false);
        xaxis.GetComponent<Image>().color = Color.white;
        RectTransform xt = xaxis.GetComponent<RectTransform>();
        xt.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        xt.localScale = Vector3.one;
        xt.sizeDelta = new Vector2(gt.sizeDelta.x, 1f); // change this eventually
        xt.anchorMin = Vector2.zero;
        xt.anchorMax = Vector2.zero;
        xt.pivot = Vector2.zero;
        xt.anchoredPosition = Vector2.zero;
    }

    private GameObject GetLegendKey(string var) {
        if (!legend.ContainsKey(var)) {
            GameObject blankKey = (GameObject)Instantiate(Resources.Load("LegendKey"), transform);
            blankKey.transform.SetParent(legendContainer.transform, false);
            legend[var] = blankKey;
        }

        return legend[var];
    }

    private GameObject GetBarContainer(Robot robot) {
        if (!barContainers.ContainsKey(robot)) { //this is wrong - needs to be a container for the bars
            // TODO: delete bar prefab
            GameObject container = new GameObject("barContainer", typeof(Image));
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
        int keyCount = 0;
        float keySpacing = 2f;

        foreach (Robot r in robots) {
            barCount = 0f;
            GameObject barContainer = GetBarContainer(r);
            // set parent for this
            barContainer.transform.SetParent(graphContainer.transform, false);

            barContainer.GetComponent<Image>().color = Color.white;

            RectTransform ct = barContainer.GetComponent<RectTransform>();
            ct.anchorMax = new Vector2(0f, 0f);
            ct.anchorMin = new Vector2(0f, 0f);
            ct.pivot = new Vector2(0f, 0f);
            ct.localScale = Vector3.one;
            ct.localRotation = new Quaternion(0, 0, 0, 0);
            ct.sizeDelta = new Vector2(200f, 200f); // change this eventually
            ct.anchoredPosition = new Vector2((containerSpacing + ct.rect.width) * containerCount, 0f);


            // now that we have the container, we need to fill it with the bars
            foreach (string var in variables) {
                GameObject bar = GetBar(r, var);

                // put the bar inside the container
                bar.transform.SetParent(barContainer.transform, false);

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

        foreach (string var in variables) {
            // set color and text values for each variable
            GameObject key = GetLegendKey(var);
            key.transform.SetParent(legendContainer.transform, false);

            GameObject icon = key.transform.Find("Icon").gameObject;
            icon.GetComponent<Image>().color = varColors[var];  // same color as bar

            GameObject text = key.transform.Find("Text").gameObject;
            text.GetComponent<Text>().text = var;

            // set key position
            RectTransform kt = key.GetComponent<RectTransform>();
            kt.anchorMax = new Vector2(0f, 0.5f);
            kt.anchorMin = new Vector2(0f, 0.5f);
            kt.pivot = new Vector2(0f, 0.5f);
            kt.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            kt.localRotation = new Quaternion(0, 0, 0, 0);

            // translate each key lower than the last
            kt.anchoredPosition = new Vector2((keySpacing + kt.rect.width) * keyCount * kt.localScale.x, 0f);

            keyCount++;
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