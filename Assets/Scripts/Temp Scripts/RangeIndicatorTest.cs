using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicatorTest : MonoBehaviour
{
    Robot r1, r2, r3, r4, r5;
    IVisualization ri1, ri2;

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

        List<RangePolicy> policies = new List<RangePolicy>();
        policies.Add(policy1);
        policies.Add(policy2);
        policies.Add(policy3);

        ri1 = new RangeIndicator("val", policies, r1);
        ri2 = new RangeIndicator("val", policies, r4);
    }

    // Update is called once per frame
    void Update()
    {
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
