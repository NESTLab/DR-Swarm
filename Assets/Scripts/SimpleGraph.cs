using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class SimpleGraph : MonoBehaviour
{
    /* Unity Properties */
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private string variableName;
    [SerializeField] private int numDataPoints;

    private RectTransform graphContainer;
    
    private List<GameObject> circles;
    private List<GameObject> connectingLines;

    private float yMax = float.NegativeInfinity, yMin = float.PositiveInfinity;

    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        this.circles = new List<GameObject>();
        this.connectingLines = new List<GameObject>();

        // Create circles and connecting lines
        GameObject lastCircle = null;
        for (int i = 0; i < numDataPoints; i++)
        {
            int xPos = Mathf.RoundToInt(i * (graphContainer.sizeDelta.x / numDataPoints));

            GameObject circle = CreateCircle();
            circle.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0);
            this.circles.Add(circle);

            if (lastCircle != null)
            {
                GameObject connectionObject = new GameObject("connection", typeof(Image));
                connectionObject.transform.SetParent(graphContainer, false);
                connectionObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                connectionObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

                this.connectingLines.Add(connectionObject);
            }
            lastCircle = circle;
        }

        // Subscribe to data
        string robotName = transform.parent.gameObject.name;
        VariableDict dict = DataManager.Instance.GetRobotDict(robotName);
        dict.GetObservableValue<float>(variableName).Subscribe(value =>
        {
            yMin = (value < yMin) ? value : yMin;
            yMax = (value > yMax) ? value : yMax;

            float newCirclePosition = 0;
            if (yMax - yMin != 0)
                newCirclePosition = graphContainer.sizeDelta.y * ((value - yMin) / (yMax - yMin));

            // Update circle positions
            // For every circle i, set i.y = (i+1).y
            for (int i = 0; i < circles.Count; i++)
            {
                RectTransform circle = circles[i].GetComponent<RectTransform>();

                float x = circle.anchoredPosition.x;
                float y;
                if (i < circles.Count - 1)
                {
                    RectTransform nextCircle = circles[i + 1].GetComponent<RectTransform>();
                     y = nextCircle.anchoredPosition.y;
                } else
                {
                    y = newCirclePosition;
                }
                circle.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

                if (i > 1)
                {
                    GameObject connectionObject = connectingLines[i - 1];
                    RectTransform prevCircle = circles[i - 1].GetComponent<RectTransform>();
                    UpdateConnectionLine(connectionObject, circle.anchoredPosition, prevCircle.anchoredPosition);
                }
            }
        });
    }

    private void Update()
    {
        transform.LookAt(GameObject.Find("ARCamera").transform);

        // Rotate 180 around Y axis, because LookAt points the Z axis at the camera
        // when instead we want it pointing away from the camera
        transform.Rotate(new Vector3(0, 180, 0), Space.Self);
    }
    
    private GameObject CreateCircle()
    {
        GameObject circleObject = new GameObject("circle", typeof(Image));
        circleObject.transform.SetParent(graphContainer, false);
        circleObject.GetComponent<Image>().sprite = circleSprite;

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
