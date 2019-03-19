using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using shapeNamespace;

public class MapIndicatorTest : MonoBehaviour
{
    Robot r1, r2;
    IVisualization mi1, mi2;
    float theta = 0;
    float rotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r1.SetVariable("val1", 0.5f);
        r1.SetVariable("val2", 0.5f);
        r1.SetVariable("val3", 0.0f);

        r2 = DataManager.Instance.GetRobot("RobotTarget2");
        r2.SetVariable("val1", 0.8f);
        r2.SetVariable("val2", 0.3f);
        r2.SetVariable("val3", 180f);

        MapPolicy mp1 = new MapPolicy("color", "val2", MapPolicy.MapPolicyType.color);
        MapPolicy mp2 = new MapPolicy("fill", "val1", MapPolicy.MapPolicyType.fillAmount);
        MapPolicy mp3 = new MapPolicy("orientation", "val3", MapPolicy.MapPolicyType.orientation);

        List<MapPolicy> policies = new List<MapPolicy>();
        policies.Add(mp1);
        policies.Add(mp2);
        policies.Add(mp3);

        mi1 = new MapIndicator(policies, Color.blue, IndicatorShape.Circle, r1, r2);

        mi2 = new MapIndicator(policies, Color.blue, IndicatorShape.Arrow, r1, r2);
    }

    // Update is called once per frame
    void Update()
    {
        theta += 0.01f;
        rotation += 1;

        r1.SetVariable("val2", Mathf.Sin(theta) * Mathf.Sin(theta));
        r2.SetVariable("val1", Mathf.Cos(theta) * Mathf.Cos(theta));

        r1.SetVariable("val3", rotation);

        if (Input.GetKeyDown("1")) {
            Debug.Log("add mi2");
            VisualizationManager.Instance.AddVisualization("testvis2", mi2);
        }
        else if (Input.GetKeyDown("2")) {
            Debug.Log("remove mi2");
            VisualizationManager.Instance.RemoveVisualization("testvis2");
        }
        else if (Input.GetKeyDown("3")) {
            VisualizationManager.Instance.AddVisualization("testvis", mi1);
        }
        else if (Input.GetKeyDown("4")) {
            VisualizationManager.Instance.RemoveVisualization("testvis");
        }
    }
}
