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

    int resolution = 10;
    private Dictionary<int, GameObject> circles;
    private Dictionary<int, GameObject> connectingLines;

    private Rect drawArea = new Rect(0, 0, 0, 0);

    protected override void Start()
    {
        base.Start();

        this.circles = new Dictionary<int, GameObject>();
        this.connectingLines = new Dictionary<int, GameObject>();
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
        int i = resolution * robots.IndexOf(robot);
        Vector2 lastCirclePos = Vector2.negativeInfinity;
        foreach (Vector2 dataPoint in dataPoints[robot])
        {
            GameObject circle = GetCircle(i);
            circle.GetComponent<Image>().color = robot.color;
            circle.SetActive(true);

            float x = container.sizeDelta.x * (dataPoint.x - drawArea.xMin) / (drawArea.width);
            float y = container.sizeDelta.y * (dataPoint.y - drawArea.yMin) / (drawArea.height);
            RectTransform circleTransform = circle.GetComponent<RectTransform>();
            circleTransform.anchoredPosition = new Vector2(x, y);

            if (!lastCirclePos.Equals(Vector2.negativeInfinity))
            {
                GameObject connectingLine = GetConnectingLine(i);
                connectingLine.GetComponent<Image>().color = robot.color;
                connectingLine.SetActive(true);
                UpdateConnectionLine(connectingLine, lastCirclePos, circleTransform.anchoredPosition);
            }
            lastCirclePos = circleTransform.anchoredPosition;
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

    private GameObject GetConnectingLine(int index) {
        if (!connectingLines.ContainsKey(index))
        {
            GameObject connectionObject = new GameObject("connection", typeof(Image));
            connectionObject.SetActive(false);
            connectionObject.transform.SetParent(container, false);
            connectionObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            RectTransform transform = connectionObject.GetComponent<RectTransform>();
            transform.sizeDelta = new Vector2(0, 0);
            transform.anchorMin = new Vector2(0, 0);
            transform.anchorMax = new Vector2(0, 0);

            connectingLines[index] = connectionObject;
            return connectionObject;
        }

        return connectingLines[index];
    }

    // TODO: This function is particularly slow for some reason
    private void UpdateConnectionLine(GameObject line, Vector2 positionA, Vector2 positionB)
    {
        Vector2 dir = (positionB - positionA).normalized;
        float distance = Vector2.Distance(positionA, positionB);

        RectTransform transform = line.GetComponent<RectTransform>();
        transform.sizeDelta = new Vector2(distance, 3.0f);
        transform.anchoredPosition = positionA + dir * distance * 0.5f;
        transform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    private float GetAngleFromVectorFloat(Vector2 dir)
    {
        float angleRot = Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI;
        return angleRot;
    }
}