using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealDataTest : MonoBehaviour
{

    Robot r1, r2;
    IVisualization lg, lg2;

    // Start is called before the first frame update
    void Start()
    {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r2 = DataManager.Instance.GetRobot("RobotTarget2");

        lg = new LineGraph("t", "color", r1);
        lg2 = new LineGraph("t", "color", r2);

        VisualizationManager.Instance.AddVisualization("r1.lg", lg);
        VisualizationManager.Instance.AddVisualization("r2.lg", lg2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
