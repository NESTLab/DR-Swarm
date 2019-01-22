using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChartTest : MonoBehaviour {

    Robot r1, r2, r3, r4, r5;

    IVisualization pc1, pc2;

	// Use this for initialization
	void Start () {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r1.SetVariable("val", 0.5f);

        r2 = DataManager.Instance.GetRobot("RobotTarget2");
        r2.SetVariable("val", 0.5f);

        r3 = DataManager.Instance.GetRobot("RobotTarget3");
        r3.SetVariable("val", 1f);

        r4 = DataManager.Instance.GetRobot("RobotTarget4");
        r4.SetVariable("val", 0.8f);

        r5 = DataManager.Instance.GetRobot("RobotTarget5");
        r5.SetVariable("val", 0.2f);

        pc1 = new PieChart("val", r1, r2); 
        pc2 = new PieChart("val", r1, r2, r3, r4, r5);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("1")) {
            VisualizationManager.Instance.AddVisualization("testvis2", pc2);
        }
        else if (Input.GetKeyDown("2")) {
            VisualizationManager.Instance.RemoveVisualization("testvis2");
        }
        else if (Input.GetKeyDown("3")) {
            VisualizationManager.Instance.AddVisualization("testvis", pc1);
        }
        else if (Input.GetKeyDown("4")) {
            VisualizationManager.Instance.RemoveVisualization("testvis");
        }
    }
}
