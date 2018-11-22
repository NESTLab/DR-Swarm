using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using System.Linq.Expressions;

public class LineGraphContainer : VisualizationContainer {

    Dictionary<Robot, List<float>> xAxisData;
    Dictionary<Robot, List<float>> yAxisData;

    int resolution;
    private List<GameObject> circles;
    private List<GameObject> connectingLines;

    public LineGraphContainer(LineGraph lineGraph, int resolution) : base(lineGraph)
    {
        xAxisData = new Dictionary<Robot, List<float>>();
        yAxisData = new Dictionary<Robot, List<float>>();

        this.resolution = resolution;

        this.circles = new List<GameObject>();
        this.connectingLines = new List<GameObject>();

        // Create circles and connecting lines
        GameObject lastCircle = null;
        for (int i = 0; i < resolution; i++)
        {
            int xPos = Mathf.RoundToInt(i * (container.sizeDelta.x / resolution));

            GameObject circle = CreateCircle();
            circle.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0);
            this.circles.Add(circle);

            if (lastCircle != null)
            {
                GameObject connectionObject = new GameObject("connection", typeof(Image));
                connectionObject.transform.SetParent(container, false);
                connectionObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                connectionObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

                this.connectingLines.Add(connectionObject);
            }
            lastCircle = circle;
        }
    }

    protected override void Draw()
    {
        IEnumerable<float> xEnumerable = xAxisData.Values.SelectMany(l => l);
        IEnumerable<float> yEnumerable = yAxisData.Values.SelectMany(l => l);

        float xMax = xEnumerable.Min();
        float xMin = xEnumerable.Max();

        float yMax = yEnumerable.Min();
        float yMin = yEnumerable.Max();

        IEnumerable<Vector2> dataPoints = Enumerable.Zip<float, float, Vector2>(xEnumerable, yEnumerable, (x, y) => new Vector2(x, y));

        RectTransform lastCircle = null;
        int i = 0;
        foreach (Vector2 dataPoint in dataPoints)
        {
            float x = container.sizeDelta.x * (dataPoint.x - xMin) / (xMax - xMin);
            float y = container.sizeDelta.y * (dataPoint.x - yMin) / (yMax - yMin);
            RectTransform circle = circles[i++].GetComponent<RectTransform>();
            circle.anchoredPosition = new Vector2(x, y);

            if (lastCircle != null)
            {
                UpdateConnectionLine(connectingLines[i - 1], lastCircle.anchoredPosition, circle.anchoredPosition);
            }
            lastCircle = circle;
        }
    }

    protected override void UpdateData(Dictionary<Robot, List<float>> data)
    {
        foreach (Robot r in data.Keys)
        {
            xAxisData[r].Add(data[r][0]);
            yAxisData[r].Add(data[r][1]);
        }
    }

    // Helper Functions
    private GameObject CreateCircle()
    {
        GameObject circleObject = new GameObject("circle", typeof(Image));
        circleObject.transform.SetParent(container, false);
        //circleObject.GetComponent<Image>().sprite = circleSprite;

        RectTransform transform = circleObject.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(11, 11);
        transform.anchorMin = new Vector2(0, 0);
        transform.anchorMax = new Vector2(0, 0);

        return circleObject;
    }

    private void UpdateConnectionLine(GameObject line, Vector2 positionA, Vector2 positionB)
    {
        Vector2 dir = (positionB - positionA).normalized;
        float distance = Vector2.Distance(positionA, positionB);

        RectTransform transform = line.GetComponent<RectTransform>();
        transform.anchorMin = new Vector2(0, 0);
        transform.anchorMax = new Vector2(0, 0);
        transform.sizeDelta = new Vector2(distance, 3.0f);
        transform.anchoredPosition = positionA + dir * distance * 0.5f;
        transform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    private float GetAngleFromVectorFloat(Vector2 dir)
    {
        float difference = (dir.y / dir.x);
        float angleRot = Mathf.Atan(difference) * 180 / Mathf.PI;
        return angleRot;
    }
}