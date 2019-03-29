using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

/// <summary>
/// Class responsible for drawing the multivariable pie chart visualization.
/// </summary>
/// <remarks>
/// Instances of VisualizationContainer have access to the container RectTransform container: 
/// the RectTransform of the drawable area in the canvas. 
/// This is NOT the same as canvas.GetComponent<RectTransform>()
/// </remarks>
public class PieChartMultiVarContainer : VisualizationContainer<PieChartMultiVar> {
    HashSet<string> variables = new HashSet<string>(); 
    Dictionary<string, float> dataDict = new Dictionary<string, float>();

    private Dictionary<string, GameObject> wedges;
    private Dictionary<string, GameObject> legend;

    // Total is the sum of all data in the pie chart.
    private float total; 
    private float zRotation = 0f;
    // curHVal is the current hue of HSV color.
    private float curHVal = 0f; 

    private GameObject chartContainer;
    private GameObject legendContainer; 

    /// <summary>
    /// Initializes the visualization container.
    /// </summary>
    protected override void Start () {
        // TODO: maybe remove
        base.Start();

        wedges = new Dictionary<string, GameObject>();
        legend = new Dictionary<string, GameObject>();

        // Set up pie chart container.
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

        // Set up legend container.
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

    /// <summary>
    /// Get a Wedge from the wedges dictionary.
    /// </summary>
    /// <param name="var"> The name of the variable associated with the desired Wedge. </param>
    /// <returns>
    /// Returns the Wedge associated with a specific variable, or a new Wedge if none has been assigned yet.
    /// </returns>
    private GameObject GetWedge(string var) {
        if (!wedges.ContainsKey(var)) {
            GameObject blankWedge = (GameObject)Instantiate(Resources.Load("Wedge"), transform);
            blankWedge.transform.SetParent(chartContainer.transform, false);

            // Set a new color distinct from nearby wedges.
            blankWedge.GetComponent<Image>().color = Color.HSVToRGB(curHVal, 1, 1);
            curHVal = (curHVal + 0.7f) % 1.0f; // TODO: change to inverse phi like the other classes

            wedges[var] = blankWedge;
        }

        return wedges[var];
    }

    /// <summary>
    /// Get a LegendKey from the legend dictionary.
    /// </summary>
    /// <param name="var"> The name of the variable associated with the desired LegendKey. </param>
    /// <returns>
    /// Returns the legend key associated with a specific variable, or a new LegendKey if none has been assigned yet.
    /// </returns>
    private GameObject GetLegendKey(string var) {
        if (!legend.ContainsKey(var)) {
            GameObject blankKey = (GameObject)Instantiate(Resources.Load("LegendKey"), transform);
            blankKey.transform.SetParent(legendContainer.transform, false);
            legend[var] = blankKey;
        }

        return legend[var];
    }

    /// <summary>
    /// Update the Unity scene. Called automatically each frame update.
    /// </summary>
    public override void Draw() {
        zRotation = 0f;
        float keySpacing = 10f;
        int keyCount = 0; 

        foreach (string v in variables) {
            GameObject wedge = GetWedge(v);
            wedge.transform.SetParent(chartContainer.transform, false);
            wedge.GetComponent<Image>().fillAmount = dataDict[v] / total; 
            wedge.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));

            RectTransform parent = chartContainer.GetComponent<RectTransform>();
            wedge.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.rect.width, parent.rect.height);

            zRotation -= wedge.GetComponent<Image>().fillAmount * 360f;

            // Set color and text values for each variable.
            GameObject key = GetLegendKey(v);
            key.transform.SetParent(legendContainer.transform, false);

            GameObject icon = key.transform.Find("Icon").gameObject;
            // We want the legend icon to be the same color as the wedge.
            icon.GetComponent<Image>().color = wedge.GetComponent<Image>().color;

            GameObject text = key.transform.Find("Text").gameObject;
            text.GetComponent<Text>().text = v;

            // Set key position.
            RectTransform t = key.GetComponent<RectTransform>();
            t.anchorMax = new Vector2(0.5f, 1f);
            t.anchorMin = new Vector2(0.5f, 1f);
            t.pivot = new Vector2(0.5f, 1f);
            t.localScale = Vector3.one;
            t.localRotation = new Quaternion(0, 0, 0, 0);

            // Translate each key lower than the last.
            t.anchoredPosition = new Vector2(0f, (-keySpacing - t.rect.height) * keyCount);

            keyCount++;
        }
    }

    /// <summary>
    /// Update internal storage of data. Called automatically when data in corresponding Visualization class.
    /// </summary>
    /// <param name="data"> Dictionary of all data relevant to the visualization. </param>
    protected override void UpdateData(Dictionary<Robot, Dictionary<string, float>> data) {
        float newTotal = 0;
        // TODO: use Union and Intersect to account for dynamically added and removed variables
        // EXAMPLE: variables.UnionWith(this.visualization.GetVariables());

        if (data.ContainsKey(this.robot)) {
            foreach (string var in data[robot].Keys) {
                dataDict[var] = data[robot][var];

                // Update the total.
                newTotal += dataDict[var];

                if (!variables.Contains(var)) {
                    variables.Add(var);
                }
            }

            total = newTotal;
        }
    }
}
