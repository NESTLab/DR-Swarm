using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DrSwarm.Model;
using DrSwarm.Model.Visualizations;

namespace DrSwarm.VisualizationContainers
{
    public class PieChartContainer : VisualizationContainer<PieChart>
    {
        // Instances of VisualizationContainer have access to the container
        // RectTransform container: the RectTransform of the drawable area in the
        // canvas. NOT the same as canvas.GetComponent<RectTransform>()
        List<Robot> robots = new List<Robot>();
        HashSet<string> variables = new HashSet<string>();

        Dictionary<Robot, float> dataDict = new Dictionary<Robot, float>();

        private Dictionary<Robot, GameObject> wedges;
        private Dictionary<Robot, GameObject> legend;

        private float total; // sum of all data in pie chart
        private float zRotation = 0f;

        private GameObject chartContainer;
        private GameObject legendContainer;

        // Initialize things
        protected override void Start()
        {
            // TODO: maybe remove
            base.Start();

            wedges = new Dictionary<Robot, GameObject>();
            legend = new Dictionary<Robot, GameObject>();

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

        private GameObject GetWedge(Robot robot)
        {
            if (!wedges.ContainsKey(robot))
            {
                GameObject blankWedge = (GameObject)Instantiate(Resources.Load("Wedge"), transform);
                blankWedge.transform.SetParent(chartContainer.transform, false);
                wedges[robot] = blankWedge;
            }

            return wedges[robot];
        }

        private GameObject GetLegendKey(Robot robot)
        {
            if (!legend.ContainsKey(robot))
            {
                GameObject blankKey = (GameObject)Instantiate(Resources.Load("LegendKey"), transform);
                blankKey.transform.SetParent(legendContainer.transform, false);
                legend[robot] = blankKey;
            }

            return legend[robot];
        }

        // Update stuff in Unity scene. Called automatically each frame update
        public override void Draw()
        {
            zRotation = 0f;
            float keySpacing = 10f;
            int keyCount = 0;

            foreach (Robot r in robots)
            {
                GameObject wedge = GetWedge(r);
                wedge.transform.SetParent(chartContainer.transform, false);
                wedge.GetComponent<Image>().color = r.color;
                wedge.GetComponent<Image>().fillAmount = dataDict[r] / total;
                wedge.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));

                RectTransform parent = chartContainer.GetComponent<RectTransform>();
                wedge.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.rect.width, parent.rect.height);

                zRotation -= wedge.GetComponent<Image>().fillAmount * 360f;

                // set color and text values for each robot
                GameObject key = GetLegendKey(r);
                key.transform.SetParent(legendContainer.transform, false);

                GameObject icon = key.transform.Find("Icon").gameObject;
                icon.GetComponent<Image>().color = r.color;

                GameObject text = key.transform.Find("Text").gameObject;
                text.GetComponent<Text>().text = r.name;

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
        protected override void UpdateData(Dictionary<Robot, Dictionary<string, float>> data)
        {
            float newTotal = 0;
            // TODO: make set account for removed variables as well
            variables.UnionWith(this.visualization.GetVariables());
            foreach (Robot r in data.Keys)
            {
                if (!robots.Contains(r))
                {
                    robots.Add(r);
                }

                foreach (string var in variables)
                {
                    dataDict[r] = data[r][var];
                }
            }

            // TODO: incorporate this into the other for loop - doesn't need to be separate
            // update the total 
            foreach (Robot r in dataDict.Keys)
            {
                newTotal += dataDict[r];
            }

            total = newTotal;
        }
    }
}