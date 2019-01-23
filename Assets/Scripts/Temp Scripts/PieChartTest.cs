using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChartTest : MonoBehaviour {

    Robot r1, r2, r3, r4, r5;
    float theta = 0;

    IVisualization pc1, pc2;

	// Use this for initialization
	void Start () {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r1.SetVariable("val", 0.0f);
        r1.color = new Color(1f, 1f, 1f); // white

        r2 = DataManager.Instance.GetRobot("RobotTarget2");
        r2.SetVariable("val", 0.0f);
        r2.color = new Color(0f, 1f, 1f); // cyan

        r3 = DataManager.Instance.GetRobot("RobotTarget3");
        r3.SetVariable("val", 1f);
        r3.color = new Color(1f, 0f, 1f); // magenta

        r4 = DataManager.Instance.GetRobot("RobotTarget4");
        r4.SetVariable("val", 0.8f);
        r4.color = new Color(0f, 1f, 0f); // green

        r5 = DataManager.Instance.GetRobot("RobotTarget5");
        r5.SetVariable("val", 0.2f);
        r5.color = new Color(1f, 1f, 0f); // yellow

        pc1 = new PieChart("val", r1, r2); 
        pc2 = new PieChart("val", r1, r2, r3, r4, r5);
    }
	
	// Update is called once per frame
	void Update () {

        theta += 0.01f;

        r1.SetVariable("val", Mathf.Sin(theta) * Mathf.Sin(theta));

        r2.SetVariable("val", Mathf.Cos(theta) * Mathf.Cos(theta));

        if (Input.GetKeyDown("1")) {
            Debug.Log("add pc2");
            VisualizationManager.Instance.AddVisualization("testvis2", pc2);
        }
        else if (Input.GetKeyDown("2")) {
            Debug.Log("remove pc2");
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
