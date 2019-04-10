using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarChartTest : MonoBehaviour {

    Robot r1, r2;
    IVisualization bc1;
    float theta = 0f;

    // Use this for initialization
    void Start () {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r1.SetVariable("x", 0f);
        r1.SetVariable("y", 0f);

        r2 = DataManager.Instance.GetRobot("RobotTarget2");
        r2.SetVariable("x", 0.5f);
        r2.SetVariable("y", 0.7f);

        HashSet<Robot> robots = new HashSet<Robot>();
        robots.Add(r1);
        robots.Add(r2);

        HashSet<string> vars = new HashSet<string>();
        vars.Add("x");
        vars.Add("y");

        bc1 = new BarGraph(robots, vars);
    }
	
	// Update is called once per frame
	void Update () {
        theta += 0.01f;

        r1.SetVariable("x", Mathf.Sin(theta) * Mathf.Sin(theta));
        r1.SetVariable("y", Mathf.Cos(theta) * Mathf.Cos(theta));

        if (Input.GetKeyDown("1")) {
            VisualizationManager.Instance.AddVisualization("bargraph1", bc1);
        } else if (Input.GetKeyDown("2")) {
            VisualizationManager.Instance.RemoveVisualization("bargraph1");
        }
    }
}
