using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarChartTest : MonoBehaviour {

    Robot r1, r2, r3, r4;
    float theta = 0f;

    IVisualization bc1, bc2;

    // Use this for initialization
    void Start () {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r1.SetVariable("var1", 0.5f);
        r1.SetVariable("var2", 1f);
        r1.SetVariable("var3", 1.2f);

        r2 = DataManager.Instance.GetRobot("RobotTarget2");
        r2.SetVariable("var1", 0.8f);
        r2.SetVariable("var2", 0.2f);
        r2.SetVariable("var3", 1.2f);

        r3 = DataManager.Instance.GetRobot("RobotTarget3");
        r3.SetVariable("var1", 0.0f);
        r3.SetVariable("var2", 0.2f);
        r3.SetVariable("var3", 1.2f);

        r4 = DataManager.Instance.GetRobot("RobotTarget4");
        r4.SetVariable("var1", 1.1f);
        r4.SetVariable("var2", 0.6f);
        r4.SetVariable("var3", 1.2f);

        HashSet<Robot> robots = new HashSet<Robot>();
        robots.Add(r2);
        robots.Add(r3);
        robots.Add(r4);

        HashSet<string> vars = new HashSet<string>();
        vars.Add("var2");
        vars.Add("var3");

        HashSet<Robot> lessRobots = new HashSet<Robot>();
        //lessRobots.Add(r2);

        HashSet<string> lessVars = new HashSet<string>();

        bc1 = new BarGraph(r1, robots, "var1", vars);
        bc2 = new BarGraph(r1, lessRobots, "var1", vars);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("1")) {
            Debug.Log("add bc2");
            VisualizationManager.Instance.AddVisualization("testvis2", bc2);
        }
        else if (Input.GetKeyDown("2")) {
            Debug.Log("remove bc2");
            VisualizationManager.Instance.RemoveVisualization("testvis2");
        }
        else if (Input.GetKeyDown("3")) {
            VisualizationManager.Instance.AddVisualization("testvis", bc1);
        }
        else if (Input.GetKeyDown("4")) {
            VisualizationManager.Instance.RemoveVisualization("testvis");
        }
    }
}
