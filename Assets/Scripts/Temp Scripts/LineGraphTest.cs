using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGraphTest : MonoBehaviour {

    Robot r1, r2;
    float x = 0, y = 0;

	// Use this for initialization
	void Start () {
        r1 = new Robot("a");
        r1.SetVariable("x", 0.0f);
        r1.SetVariable("y", 0.0f);

        r2 = new Robot("b");
        r2.SetVariable("x", 0.0f);
        r2.SetVariable("y", 0.0f);

        GameObject obj = GameObject.Find("RobotTarget1");
        LineGraph lg = new LineGraph("x", "y", r1, r2);
        LineGraphContainer c = obj.AddComponent<LineGraphContainer>();
        c.visualization = lg;
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
        r2.SetVariable("y", Mathf.Cos(theta));
    }
}
