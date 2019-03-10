using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class BarGraphContainer : VisualizationContainer<BarGraph> {
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
    private Dictionary<Robot, GameObject> xLabels = new Dictionary<Robot, GameObject>();
    private Dictionary<int, GameObject> yLabels = new Dictionary<int, GameObject>();

    private float curHVal = 0f;
    private float invphi = 1f / 1.618f; // golden ratio
    private float topVal = 0f;  // this is for dynamically resizing the graphs

    private GameObject graphContainer;
    private GameObject legendContainer;

    private float axisOffset;

    // Initialize things
    protected override void Start() {
        //TODO: maybe remove
        base.Start();

        barContainers = new Dictionary<Robot, GameObject>();
        bars = new Dictionary<Robot, Dictionary<string, GameObject>>();
        legend = new Dictionary<string, GameObject>();

        axisOffset = 30f;

        // set up bar graph container
        graphContainer = new GameObject("BarGraph", typeof(Image));
        graphContainer.transform.SetParent(container.transform, false);
        RectTransform gt = graphContainer.GetComponent<RectTransform>();
        gt.sizeDelta = new Vector2(container.sizeDelta.x, 270f);  //TODO: need to make this dynamically size
        gt.anchorMax = new Vector2(0.5f, 1f);
        gt.anchorMin = new Vector2(0.5f, 1f);
        gt.pivot = new Vector2(0.5f, 1f);
        gt.localScale = Vector3.one;
        gt.localRotation = new Quaternion(0, 0, 0, 0);
        gt.anchoredPosition = Vector2.zero;

        graphContainer.GetComponent<Image>().color = Color.clear;

        // set up legend container
        legendContainer = new GameObject("Legend", typeof(Image));
        legendContainer.transform.SetParent(container.transform, false);
        RectTransform lt = legendContainer.GetComponent<RectTransform>();
        lt.sizeDelta = new Vector2(container.sizeDelta.x, 150f);  //TODO: need to make this dynamically size
        lt.anchorMax = new Vector2(0.5f, 0f);
        lt.anchorMin = new Vector2(0.5f, 0f);
        lt.pivot = new Vector2(0.5f, 0f);
        lt.localScale = Vector3.one;
        lt.localRotation = new Quaternion(0, 0, 0, 0);
        lt.anchoredPosition = Vector2.zero;

        legendContainer.GetComponent<Image>().color = Color.clear;

        // Y axis
        GameObject yaxis = CreateImage("y-axis", gt, Color.white);
        RectTransform yt = yaxis.GetComponent<RectTransform>();
        yt.sizeDelta = new Vector2(1f, gt.sizeDelta.y - axisOffset); //TODO: change this eventually
        yt.anchorMin = Vector2.zero;
        yt.anchorMax = Vector2.zero;
        yt.pivot = Vector2.zero;
        yt.anchoredPosition = new Vector2(axisOffset, axisOffset);  //TODO: change this eventually

        // X axis
        GameObject xaxis = CreateImage("x-axis", gt, Color.white);
        RectTransform xt = xaxis.GetComponent<RectTransform>();
        xt.sizeDelta = new Vector2(gt.sizeDelta.x - axisOffset, 1f); //TODO: change this eventually
        xt.anchorMin = Vector2.zero;
        xt.anchorMax = Vector2.zero;
        xt.pivot = Vector2.zero;
        xt.anchoredPosition = new Vector2(axisOffset, axisOffset);  //TODO: change this eventually

        // gotta be a better way to do these - with a dynamic number of lines
        // top guideline
        GameObject top = CreateImage("top-guide", gt, Color.white);
        RectTransform tt = top.GetComponent<RectTransform>();
        tt.sizeDelta = new Vector2(gt.sizeDelta.x - axisOffset, 1f);
        tt.anchorMin = new Vector2(0f, 1f);
        tt.anchorMax = new Vector2(0f, 1f);
        tt.pivot = new Vector2(0f, 1f);
        tt.anchoredPosition = new Vector2(axisOffset, 0f);

        yLabels[1] = CreateLabel("top-label");

        // middle guideline
        GameObject mid = CreateImage("mid-guide", gt, Color.white);
        RectTransform mt = mid.GetComponent<RectTransform>();
        mt.sizeDelta = new Vector2(gt.sizeDelta.x - axisOffset, 1f);
        mt.anchorMin = new Vector2(0f, 0.5f);
        mt.anchorMax = new Vector2(0f, 0.5f);
        mt.pivot = new Vector2(0f, 0.5f);
        mt.anchoredPosition = new Vector2(axisOffset, 0f);

        yLabels[0] = CreateLabel("mid-label");
    }

    private GameObject CreateImage(string name, RectTransform parent, Color color) {
        GameObject image = new GameObject(name, typeof(Image));
        image.transform.SetParent(parent, false);
        image.GetComponent<Image>().color = color;

        RectTransform t = image.GetComponent<RectTransform>();
        t.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        t.localScale = Vector3.one;

        return image;
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
        if (!barContainers.ContainsKey(robot)) { 
            GameObject container = new GameObject("barContainer", typeof(Image));
            container.transform.SetParent(graphContainer.transform, false);
            container.GetComponent<Image>().color = Color.clear;
            barContainers[robot] = container;
        }

        return barContainers[robot];
    }

    // this might make the bar containers function useless
    private GameObject GetBar(Robot robot, RectTransform parent, string var) {
        if (!bars.ContainsKey(robot)) {
            // initialize the dictionary
            Dictionary<string, GameObject> barDict = new Dictionary<string, GameObject>();
            bars[robot] = barDict;
        }

        if (!bars[robot].ContainsKey(var)) {
            // initialize the single bar
            GameObject blankBar = CreateImage("bar", parent, Color.white);
            blankBar.GetComponent<Image>().color = varColors[var];
            bars[robot][var] = blankBar;
        }

        return bars[robot][var];
    }

    private GameObject GetXLabel(Robot r) {
        if (!xLabels.ContainsKey(r)) {
            GameObject label = CreateLabel("x-label");
            xLabels[r] = label;
        }

        return xLabels[r];
    }

    private GameObject CreateLabel(string name) {
        GameObject label = new GameObject(name, typeof(Text));
        Text text = label.GetComponent<Text>();

        text.color = Color.white;
        text.fontSize = 14;
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.alignment = TextAnchor.MiddleCenter;

        return label;
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() {
        float containerSpacing = 10f;
        float containerCount = 0f;
        float barSpacing = 0f;  // change this eventually
        float barCount = 0f;
        float keyXSpacing = 2f;
        float keyYSpacing = 10f;
        int keyCount = 0;

        foreach (Robot r in robots) {
            barCount = 0f;
            GameObject barContainer = GetBarContainer(r);            

            RectTransform ct = barContainer.GetComponent<RectTransform>();
            ct.anchorMax = new Vector2(0f, 0f);
            ct.anchorMin = new Vector2(0f, 0f);
            ct.pivot = new Vector2(0f, 0f);
            ct.localScale = Vector3.one;
            ct.localRotation = new Quaternion(0, 0, 0, 0);
            RectTransform parent = graphContainer.GetComponent<RectTransform>();
            // width should be container width divided by number of robots
            float containerSize = ((parent.sizeDelta.x - axisOffset) / robots.Count) - containerSpacing;
            ct.sizeDelta = new Vector2(containerSize, parent.sizeDelta.y - axisOffset);
            ct.anchoredPosition = new Vector2(((containerSize + containerSpacing) * containerCount) + axisOffset + 2, axisOffset + 2); //TODO: change this eventually
            
            // x labels
            GameObject xLabel = GetXLabel(r);
            xLabel.transform.SetParent(graphContainer.transform, false);
            xLabel.GetComponent<Text>().text = r.name;
            
            RectTransform xTransform = xLabel.GetComponent<RectTransform>();
            xTransform.anchorMin = new Vector2(0f, 0f);
            xTransform.anchorMax = new Vector2(0f, 0f);
            xTransform.pivot = new Vector2(0.5f, 0f);
            xTransform.anchoredPosition = new Vector2(((containerSpacing + containerSize) * containerCount) + axisOffset + 2 + (containerSize/2), 0f);
            xTransform.sizeDelta = new Vector2(100f, axisOffset);  //TODO: change eventually

            // now that we have the container, we need to fill it with the bars
            foreach (string var in variables) {
                GameObject bar = GetBar(r, ct, var);

                // put the bar inside the container
                bar.transform.SetParent(barContainer.transform, false);
                RectTransform bparent = barContainer.GetComponent<RectTransform>();

                // width should be container width divided by number of robots
                float barSize = (bparent.sizeDelta.x) / variables.Count;

                // set size
                float value = dataDict[r][var];

                if (value > topVal) {
                    topVal = value;
                }

                float constant = bparent.sizeDelta.y / topVal;

                RectTransform tb = bar.GetComponent<RectTransform>();
                tb.sizeDelta = new Vector2(barSize, value * constant); 
                tb.anchorMax = new Vector2(0f, 0f);
                tb.anchorMin = new Vector2(0f, 0f);
                tb.pivot = new Vector2(0f, 0f);
                tb.anchoredPosition = new Vector2((barSpacing + tb.rect.width) * barCount, 0f);

                // set guide labels
                GameObject top = yLabels[1];
                top.transform.SetParent(graphContainer.transform, false);
                top.GetComponent<Text>().text = string.Format("{0:0.##}", topVal);
                RectTransform tTransform = top.GetComponent<RectTransform>();
                tTransform.anchorMin = new Vector2(0f, 1f);
                tTransform.anchorMax = new Vector2(0f, 1f);
                tTransform.pivot = new Vector2(0.5f, 1f);
                tTransform.anchoredPosition = new Vector2(axisOffset/2, 0f); 
                tTransform.sizeDelta = new Vector2(axisOffset, 25f);  //TODO: change eventually

                GameObject mid = yLabels[0];
                mid.transform.SetParent(graphContainer.transform, false);
                mid.GetComponent<Text>().text = string.Format("{0:0.##}", topVal / 2);
                RectTransform mTransform = mid.GetComponent<RectTransform>();
                mTransform.anchorMin = new Vector2(0f, 1f);
                mTransform.anchorMax = new Vector2(0f, 1f);
                mTransform.pivot = new Vector2(0.5f, 1f);
                mTransform.anchoredPosition = new Vector2(axisOffset / 2, -graphContainer.GetComponent<RectTransform>().sizeDelta.y/2); 
                mTransform.sizeDelta = new Vector2(axisOffset, 25f);  //TODO: change eventually

                // update barCount
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
            kt.anchorMax = new Vector2(0f, 1f);
            kt.anchorMin = new Vector2(0f, 1f);
            kt.pivot = new Vector2(0f, 1f);
            kt.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            kt.localRotation = new Quaternion(0, 0, 0, 0);

            // translate each key lower than the last
            float x = (keyXSpacing + kt.rect.width) * (keyCount / 2) * kt.localScale.x;
            float y = (-keyYSpacing - kt.rect.height) * ((keyCount) % 2) * kt.localScale.y;
            kt.anchoredPosition = new Vector2(x, y);  //TODO: this is not the best solution

            keyCount++;
        }
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
    protected override void UpdateData(Dictionary<Robot, Dictionary<string, float>> data) {
        foreach (Robot r in data.Keys) {
            if (!robots.Contains(r)) {
                robots.Add(r);
                dataDict[r] = new Dictionary<string, float>();
            }

            foreach (string var in data[r].Keys) {
                dataDict[r][var] = data[r][var];

                //TODO: this is not the best way to do it, but not seeing another option at the moment
                if (!variables.Contains(var)) {
                    variables.Add(var);

                    // set the color for the new variable
                    varColors[var] = Color.HSVToRGB(curHVal, 1, 1);
                    curHVal = (curHVal + invphi) % 1.0f;
                }
            }
        }
    }
}