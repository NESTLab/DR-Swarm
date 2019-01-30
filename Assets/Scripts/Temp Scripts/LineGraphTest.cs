using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGraphTest : MonoBehaviour {

    Robot r1, r2, r3, r4, r5;
    float x = 0, y = 0;
    float a = 0, b = 0;

    IVisualization lg, lg2;

	// Use this for initialization
	void Start () {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r1.SetVariable("x", 0.0f);
        r1.SetVariable("y", 0.0f);

        r2 = DataManager.Instance.GetRobot("RobotTarget2");
        r2.SetVariable("x", 0.0f);
        r2.SetVariable("y", 0.0f);

        r3 = DataManager.Instance.GetRobot("RobotTarget3");
        r3.SetVariable("x", 0.0f);
        r3.SetVariable("y", 0.0f);

        r4 = DataManager.Instance.GetRobot("RobotTarget4");
        r4.SetVariable("x", 0.0f);
        r4.SetVariable("y", 0.0f);

        r5 = DataManager.Instance.GetRobot("RobotTarget5");
        r5.SetVariable("x", 0.0f);
        r5.SetVariable("y", 0.0f);

        lg = new LineGraph("x", "y", r1, r2);
        lg2 = new LineGraph("a", "b", r1);
    }
	
	// Update is called once per frame
	void Update () {
        //float theta = 2 * Mathf.PI * (Time.fixedTime / 2);
        //x += Mathf.Pow(1.25f, theta) * Mathf.Cos(theta);
        //y = Mathf.Pow(1.25f, theta) * Mathf.Sin(theta);

        x += 1;
        a += 1;

        float theta = 2 * Mathf.PI * (x / 60);

        r1.SetVariable("x", x);
        r1.SetVariable("y", Mathf.Sin(theta + 4 * Mathf.PI / 4));

        r1.SetVariable("a", a);
        r1.SetVariable("b", 4 * (a % 60) / 60);

        r2.SetVariable("x", x);
        r2.SetVariable("y", Mathf.Sin(theta + 3 * Mathf.PI / 4) + 2);

        r3.SetVariable("x", x);
        r3.SetVariable("y", Mathf.Sin(theta + 2 * Mathf.PI / 4) + 4);

        r4.SetVariable("x", x);
        r4.SetVariable("y", Mathf.Sin(theta + 1 * Mathf.PI / 4) + 6);

        r5.SetVariable("x", x);
        r5.SetVariable("y", Mathf.Sin(theta) + 8);

        if (Input.GetKeyDown("1"))
        {
            VisualizationManager.Instance.AddVisualization("testvis2", lg2);
        } else if (Input.GetKeyDown("2"))
        {
            VisualizationManager.Instance.RemoveVisualization("testvis2");
        } else if (Input.GetKeyDown("3"))
        {
            VisualizationManager.Instance.EditVisualization("testvis2", lg);
        }
    }
}
