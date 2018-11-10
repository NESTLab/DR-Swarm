using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleGraph : MonoBehaviour
{

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private string variableName;

    private RectTransform graphContainer;
    private Graph g;

    private class Graph
    {
        List<GameObject> circles;
        List<GameObject> connectingLines;

        RectTransform container;
        Sprite circleSprite;

        int numDataPoints;

        public Graph(RectTransform container, Sprite circleSprite, int n)
        {
            this.container = container;
            this.circleSprite = circleSprite;

            this.circles = new List<GameObject>();
            this.connectingLines = new List<GameObject>();

            this.numDataPoints = n;

            GameObject lastCircle = null;
            for (int i = 0; i < n; i++)
            {
                GameObject circle = CreateCircle();
                circle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
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

        public void Update(List<int> values)
        {
            List<int> lastValues = values.GetRange(Mathf.Max(values.Count - numDataPoints, 0), Mathf.Min(numDataPoints, values.Count));

            float graphHeight = container.sizeDelta.y;
            float yMaximum = 100f;
            float xSize = container.sizeDelta.x / circles.Count;

            GameObject lastCircle = null;
            for (int i = 0; i < lastValues.Count; i++)
            {
                GameObject circle = circles[i];
                float x = i * xSize;
                float y = (lastValues[i] / yMaximum) * graphHeight;
                circle.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

                if (lastCircle != null)
                {
                    GameObject connectionObject = connectingLines[i - 1];
                    Vector2 positionA = circle.GetComponent<RectTransform>().anchoredPosition;
                    Vector2 positionB = lastCircle.GetComponent<RectTransform>().anchoredPosition;
                    UpdateConnectionLine(connectionObject, positionA, positionB);
                }
                lastCircle = circle;
            }
        }

        private GameObject CreateCircle()
        {
            GameObject circleObject = new GameObject("circle", typeof(Image));
            circleObject.transform.SetParent(container, false);
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

    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        g = new Graph(graphContainer, circleSprite, 50);
    }

    int n = 0;
    List<int> values = new List<int>() { };
    private void Update()
    {
        transform.LookAt(GameObject.Find("ARCamera").transform);

        string robotName = transform.parent.gameObject.name;
        VariableDict dict = DataModel.Instance.GetRobotDict(robotName);
        if (dict.Has(variableName))
        {
            object variable = dict.Get(variableName);
            if (variable.GetType() == typeof(Vector3))
            {
                values.Add(Mathf.RoundToInt(1000 * ((Vector3)variable).z));
            } else if (variable.GetType() == typeof(int))
            {
                values.Add(50 * (int)variable);
            } else if (variable.GetType() == typeof(float))
            {
                values.Add(Mathf.RoundToInt(50 * (float)variable));
            }
        }

        g.Update(values);
        n++;
    }
}
