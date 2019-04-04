using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarChartTest : MonoBehaviour {

    Robot r1, r2;
    IVisualization bc1;

    // Use this for initialization
    void Start () {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r2 = DataManager.Instance.GetRobot("RobotTarget2");

        HashSet<Robot> robots = new HashSet<Robot>();
        robots.Add(r1);
        robots.Add(r2);

        HashSet<string> vars = new HashSet<string>();
        vars.Add("x");
        vars.Add("y");

        bc1 = new BarGraph(vars, robots);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("1")) {
            VisualizationManager.Instance.AddVisualization("bargraph1", bc1);
        } else if (Input.GetKeyDown("2")) {
            VisualizationManager.Instance.RemoveVisualization("bargraph1");
        }
    }
}
