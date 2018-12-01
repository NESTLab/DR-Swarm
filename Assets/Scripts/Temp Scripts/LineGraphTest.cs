using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGraphTest : MonoBehaviour {

    Robot r1, r2, r3, r4, r5;
    float x = 0, y = 0;

	// Use this for initialization
	void Start () {
        r1 = new Robot("a");
        r1.SetVariable("x", 0.0f);
        r1.SetVariable("y", 0.0f);

        r2 = new Robot("b");
        r2.SetVariable("x", 0.0f);
        r2.SetVariable("y", 0.0f);

        r3 = new Robot("c");
        r3.SetVariable("x", 0.0f);
        r3.SetVariable("y", 0.0f);

        r4 = new Robot("d");
        r4.SetVariable("x", 0.0f);
        r4.SetVariable("y", 0.0f);

        r5 = new Robot("e");
        r5.SetVariable("x", 0.0f);
        r5.SetVariable("y", 0.0f);

        LineGraph lg = new LineGraph("x", "y", r1, r2, r3, r4, r5);
        LineGraphContainer c = GameObject.Find("RobotTarget1").AddComponent<LineGraphContainer>();
        c.visualization = lg;

        LineGraphContainer c2 = GameObject.Find("RobotTarget2").AddComponent<LineGraphContainer>();
        c2.visualization = lg;
    }
	
	// Update is called once per frame
	void Update () {
        //float theta = 2 * Mathf.PI * (Time.fixedTime / 2);
        //x += Mathf.Pow(1.25f, theta) * Mathf.Cos(theta);
        //y = Mathf.Pow(1.25f, theta) * Mathf.Sin(theta);

        x += 1;

        float theta = 2 * Mathf.PI * (x / 60);

        r1.SetVariable("x", x);
        r1.SetVariable("y", Mathf.Sin(theta));

        r2.SetVariable("x", x);
        r2.SetVariable("y", Mathf.Sin(theta) + 2);

        r3.SetVariable("x", x);
        r3.SetVariable("y", Mathf.Sin(theta) + 4);

        r4.SetVariable("x", x);
        r4.SetVariable("y", Mathf.Sin(theta) + 6);

        r5.SetVariable("x", x);
        r5.SetVariable("y", Mathf.Sin(theta) + 8);
    }
}
