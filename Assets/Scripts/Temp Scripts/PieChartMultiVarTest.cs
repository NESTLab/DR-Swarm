using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieChartMultiVarTest : MonoBehaviour {

    Robot r1, r2;
    float theta = 0;

    IVisualization pc1, pc2;

    // Use this for initialization
    void Start () {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r1.SetVariable("var1", 0.5f);
        r1.SetVariable("var2", 0.2f);
        r1.SetVariable("var3", 0.3f);
        r1.SetVariable("var4", 1.0f);

        r2 = DataManager.Instance.GetRobot("RobotTarget2");
        r2.SetVariable("var1", 0.7f);
        r2.SetVariable("var2", 0.05f);
        r2.SetVariable("var3", 0.3f);
        r2.SetVariable("var4", 1.2f);

        List<Robot> robots = new List<Robot>();
        robots.Add(r1);
        robots.Add(r2);

        List<string> vars = new List<string>();
        vars.Add("var1");
        vars.Add("var2");
        vars.Add("var3");
        vars.Add("var4");

        List<Robot> one = new List<Robot>();
        one.Add(r1);

        // pc1 = new PieChartMultiVar(r1, "var1", "var2");
        //pc2 = new PieChartMultiVar(r1, "var1", "var2", "var3", "var4");
        pc1 = new PieChartMultiVar(vars, robots);
        pc2 = new PieChartMultiVar(vars, one);
    }
	
	// Update is called once per frame
	void Update () {
        theta += 0.01f;

        r1.SetVariable("var1", Mathf.Sin(theta) * Mathf.Sin(theta));

        r1.SetVariable("var2", Mathf.Cos(theta) * Mathf.Cos(theta));

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
