using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

// TODO: Something in here is a little slow, look into this
// I would like to get 100 data points on screen per robot, on 5 different graphs
public class LineGraphContainer : VisualizationContainer {

    List<Robot> robots = new List<Robot>();
    Dictionary<Robot, List<Vector2>> dataPoints = new Dictionary<Robot, List<Vector2>>();

    int resolution = 100;
    private Dictionary<int, GameObject> circles;
    private Dictionary<Robot, GameObject> connectingLines;

    private Rect drawArea = new Rect(0, 0, 0, 0);

    protected override void Start()
    {
        base.Start();

        this.circles = new Dictionary<int, GameObject>();
        this.connectingLines = new Dictionary<Robot, GameObject>();
    }

    protected override void Draw()
    {
        // Compute the min and max values for x and y
        // TODO: This is kind of ugly, make cleaner
        List<float> xValues = new List<float>();
        List<float> yValues = new List<float>();
        foreach (Robot r in robots)
        {
            IEnumerable<Vector2> values = dataPoints[r];

            xValues.AddRange(values.Select(v => v.x));
            yValues.AddRange(values.Select(v => v.y));
        }

        float xMin = xValues.Min();
        float xMax = xValues.Max();
        float yMin = yValues.Min();
        float yMax = yValues.Max();
        drawArea = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);

        foreach (Robot r in robots)
        {
            DrawRobot(r);
        }
    }

    protected override void UpdateData(Dictionary<Robot, List<float>> data)
    {
        foreach (Robot r in data.Keys)
        {
            if (!robots.Contains(r))
            {
                robots.Add(r);
            }

            if (!dataPoints.ContainsKey(r))
            {
                dataPoints[r] = new List<Vector2>();
            }

            dataPoints[r].Add(new Vector2(data[r][0], data[r][1]));
            
            if (dataPoints[r].Count > resolution)
            {
                dataPoints[r].RemoveAt(0);
            }
        }
    }

    // Helper Functions
    private void DrawRobot(Robot robot)
    {
        IEnumerable<Vector2> data = dataPoints[robot].Select(v =>
        {
            float x = container.sizeDelta.x * (v.x - drawArea.xMin) / (drawArea.width);
            float y = container.sizeDelta.y * (v.y - drawArea.yMin) / (drawArea.height);

            return new Vector2(x, y);
        });

        /*LineRenderer line = GetConnectingLine(robot).GetComponent<LineRenderer>();
        if (line.positionCount != data.Count())
        {
            line.positionCount = data.Count();
        }
        line.SetPositions(data.Select(v => new Vector3(v.x, v.y, -0.001f)).ToArray());
        */

        int i = 0;
        foreach (Vector2 dataPoint in data)
        {
            GameObject circle = GetCircle(i + resolution * robots.IndexOf(robot));
            circle.GetComponent<Image>().color = robot.color;
            circle.SetActive(true);

            RectTransform circleTransform = circle.GetComponent<RectTransform>();
            circleTransform.anchoredPosition = new Vector2(dataPoint.x, dataPoint.y);

            i++;
        }
    }

    private GameObject GetCircle(int index)
    {
        if (!circles.ContainsKey(index))
        {
            GameObject circleObject = new GameObject("circle", typeof(Image));
            circleObject.SetActive(false);
            circleObject.transform.SetParent(container, false);
            //circleObject.GetComponent<Image>().sprite = circleSprite;

            RectTransform transform = circleObject.GetComponent<RectTransform>();
            transform.sizeDelta = new Vector2(11, 11);
            transform.anchorMin = new Vector2(0, 0);
            transform.anchorMax = new Vector2(0, 0);

            circles[index] = circleObject;
            return circleObject;
        }

        return circles[index];
    }

    private GameObject GetConnectingLine(Robot robot) {
        if (!connectingLines.ContainsKey(robot))
        {
            GameObject connectionObject = new GameObject(robot.name + "Line", typeof(Image));
            connectionObject.transform.SetParent(container.transform);

            LineRenderer line = connectionObject.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
            line.material.color = robot.color;

            RectTransform t = connectionObject.GetComponent<RectTransform>();
            t.sizeDelta = new Vector2(600, 440);
            t.anchorMin = Vector2.zero;
            t.anchorMax = Vector2.zero;
            t.pivot = Vector2.zero;
            t.localPosition = Vector3.zero;
            t.localScale = Vector3.one;
            t.localRotation = new Quaternion(0, 0, 0, 0);

            Image i = connectionObject.GetComponent<Image>();
            i.color = Color.clear;

            connectingLines.Add(robot, connectionObject);
            return connectionObject;
        }

        return connectingLines[robot];
    }
}