using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicatorTest : MonoBehaviour
{
    Robot r1, r2, r3, r4, r5;
    IVisualization ri1, ri2;
    float theta = 0;

    // Start is called before the first frame update
    void Start()
    {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r1.SetVariable("val", 0.0f);

        r2 = DataManager.Instance.GetRobot("RobotTarget2");
        r2.SetVariable("val", 0.0f);

        r3 = DataManager.Instance.GetRobot("RobotTarget3");
        r3.SetVariable("val", 1f);

        r4 = DataManager.Instance.GetRobot("RobotTarget4");
        r4.SetVariable("val", 0.8f);

        r5 = DataManager.Instance.GetRobot("RobotTarget5");
        r5.SetVariable("val", 0.2f);

        RangePolicy policy1 = new RangePolicy("lowRange", 0f, 0.3f);
        policy1.color = Color.black;
        policy1.shape = RangePolicy.IndicatorShape.Check;
        RangePolicy policy2 = new RangePolicy("midRange", 0.3f, 0.7f);
        policy2.color = Color.green;
        policy2.shape = RangePolicy.IndicatorShape.Check;
        RangePolicy policy3 = new RangePolicy("highRange", 0.7f, 1f);
        policy3.color = Color.red;
        policy3.shape = RangePolicy.IndicatorShape.Check;

        List<RangePolicy> colorpolicies = new List<RangePolicy>();
        colorpolicies.Add(policy1);
        colorpolicies.Add(policy2);
        colorpolicies.Add(policy3);

        RangePolicy policy4 = new RangePolicy("lowRange", 0f, 03f);
        policy4.color = Color.blue;
        policy4.shape = RangePolicy.IndicatorShape.Circle;
        RangePolicy policy5 = new RangePolicy("midRange", 0.3f, 0.7f);
        policy5.color = Color.blue;
        policy5.shape = RangePolicy.IndicatorShape.Square;
        RangePolicy policy6 = new RangePolicy("highRange", 0.7f, 1f);
        policy6.color = Color.blue;
        policy6.shape = RangePolicy.IndicatorShape.Triangle;

        List<RangePolicy> shapepolicies = new List<RangePolicy>();
        shapepolicies.Add(policy4);
        shapepolicies.Add(policy5);
        shapepolicies.Add(policy6);

        // maybe the indicator classes should only take in a single robot
        ri1 = new RangeIndicator("val", colorpolicies, r1); // not sure how to add more robots without the window getting irrelevant indicators
        ri2 = new RangeIndicator("val", shapepolicies, r1);
    }

    // Update is called once per frame
    void Update()
    {
        theta += 0.01f;

        r1.SetVariable("val", Mathf.Sin(theta) * Mathf.Sin(theta));

        if (Input.GetKeyDown("1")) {
            Debug.Log("add ri2");
            VisualizationManager.Instance.AddVisualization("testvis2", ri2);
        }
        else if (Input.GetKeyDown("2")) {
            Debug.Log("remove ri2");
            VisualizationManager.Instance.RemoveVisualization("testvis2");
        }
        else if (Input.GetKeyDown("3")) {
            VisualizationManager.Instance.AddVisualization("testvis", ri1);
        }
        else if (Input.GetKeyDown("4")) {
            VisualizationManager.Instance.RemoveVisualization("testvis");
        }
    }
}
