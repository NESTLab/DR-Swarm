using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class PieChartMultiVarContainer : VisualizationContainer<PieChartMultiVar> {
    // Instances of VisualizationContainer have access to the container
    // RectTransform container: the RectTransform of the drawable area in the
    // canvas. NOT the same as canvas.GetComponent<RectTransform>()
    List<Robot> robots = new List<Robot>(); // there's only going to be one this time
    HashSet<string> variables = new HashSet<string>(); // I think this is what we want
    // don't think this s relevant anymore
    Dictionary<string, float> dataDict = new Dictionary<string, float>();

    private Dictionary<string, GameObject> wedges;
    private Dictionary<string, GameObject> legend;

    private float total; // sum of all data in pie chart
    private float zRotation = 0f;

    private GameObject chartContainer;
    private GameObject legendContainer; 

    // Use this for initialization
    protected override void Start () {
        // TODO: maybe remove
        base.Start();

        wedges = new Dictionary<string, GameObject>();
        legend = new Dictionary<string, GameObject>();

        // set up pie chart container
        chartContainer = new GameObject("PieChart", typeof(Image));
        chartContainer.transform.SetParent(container.transform, false);
        RectTransform t = chartContainer.GetComponent<RectTransform>();
        t.sizeDelta = new Vector2(250f, 250f);
        t.anchorMax = new Vector2(0f, 0.5f);
        t.anchorMin = new Vector2(0f, 0.5f);
        t.pivot = new Vector2(0f, 0.5f);
        t.localScale = Vector3.one;
        t.localRotation = new Quaternion(0, 0, 0, 0);
        t.anchoredPosition = Vector2.zero;

        chartContainer.GetComponent<Image>().color = Color.clear;

        // set up legend container
        legendContainer = new GameObject("Legend", typeof(Image));
        legendContainer.transform.SetParent(container.transform, false);
        RectTransform lt = legendContainer.GetComponent<RectTransform>();
        lt.sizeDelta = new Vector2(300f, 300f);
        lt.anchorMax = new Vector2(1f, 0.5f);
        lt.anchorMin = new Vector2(1f, 0.5f);
        lt.pivot = new Vector2(1f, 0.5f);
        lt.localScale = Vector3.one;
        lt.localRotation = new Quaternion(0, 0, 0, 0);
        lt.anchoredPosition = Vector2.zero;

        legendContainer.GetComponent<Image>().color = Color.clear;
    }

    private GameObject GetWedge(string var) {
        if (!wedges.ContainsKey(var)) {
            GameObject blankWedge = (GameObject)Instantiate(Resources.Load("Wedge"), transform);
            blankWedge.transform.SetParent(chartContainer.transform, false);
            wedges[var] = blankWedge;
        }

        return wedges[var];
    }

    private GameObject GetLegendKey(string var) {
        if (!legend.ContainsKey(var)) {
            GameObject blankKey = (GameObject)Instantiate(Resources.Load("LegendKey"), transform);
            blankKey.transform.SetParent(legendContainer.transform, false);
            legend[var] = blankKey;
        }

        return legend[var];
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() {
        zRotation = 0f;
        float keySpacing = 10f;
        int keyCount = 0; // this may not be necessary

        foreach (string v in variables) {
            GameObject wedge = GetWedge(v);
            wedge.transform.SetParent(chartContainer.transform, false);
            wedge.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            wedge.GetComponent<Image>().fillAmount = dataDict[v] / total; 
            wedge.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));

            RectTransform parent = chartContainer.GetComponent<RectTransform>();
            wedge.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.rect.width, parent.rect.height);

            zRotation -= wedge.GetComponent<Image>().fillAmount * 360f;

            // set color and text values for each robot
            GameObject key = GetLegendKey(v);
            key.transform.SetParent(legendContainer.transform, false);

            GameObject icon = key.transform.Find("Icon").gameObject;
            // probably a better way to do this
            icon.GetComponent<Image>().color = wedge.GetComponent<Image>().color;  // same color as wedge

            GameObject text = key.transform.Find("Text").gameObject;
            text.GetComponent<Text>().text = v;

            // set key position
            RectTransform t = key.GetComponent<RectTransform>();
            t.anchorMax = new Vector2(0.5f, 1f);
            t.anchorMin = new Vector2(0.5f, 1f);
            t.pivot = new Vector2(0.5f, 1f);
            t.localScale = Vector3.one;
            t.localRotation = new Quaternion(0, 0, 0, 0);

            // translate each key lower than the last
            t.anchoredPosition = new Vector2(0f, (-keySpacing - t.rect.height) * keyCount);

            keyCount++;
        }
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
    protected override void UpdateData(Dictionary<Robot, Dictionary<string, float>> data) {
        float newTotal = 0;
        // TODO: make set account for removed variables as well
        variables.UnionWith(this.visualization.GetVariables());

        foreach (Robot r in data.Keys) {
            // I think this is useless at this point
            if (!robots.Contains(r)) {
                robots.Add(r);
            }

            foreach (string var in variables) {
                dataDict[var] = data[r][var];
            }
        }

        // TODO: incorporate this into the other for loop - doesn't need to be separate
        // update the total 
        foreach (string var in dataDict.Keys) {
            newTotal += dataDict[var];
        }

        total = newTotal;
    }
}
