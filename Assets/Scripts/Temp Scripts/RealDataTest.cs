using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealDataTest : MonoBehaviour
{

    Robot r1, r2;
    IVisualization bg1, bg2;

    // Start is called before the first frame update
    void Start()
    {
        r1 = DataManager.Instance.GetRobot("RobotTarget98");
        r2 = DataManager.Instance.GetRobot("RobotTarget99");

        HashSet<Robot> hs1 = new HashSet<Robot>() { r1 };
        HashSet<Robot> hs2 = new HashSet<Robot>() { r2 };
        HashSet<string> vars = new HashSet<string>() { "state" };
        bg1 = new BarGraph(hs1, vars);
        bg2 = new BarGraph(hs2, vars);

        VisualizationManager.Instance.AddVisualization("r1.bg", bg1);
        VisualizationManager.Instance.AddVisualization("r2.bg", bg2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
