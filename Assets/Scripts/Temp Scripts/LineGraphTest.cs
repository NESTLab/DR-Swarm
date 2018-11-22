using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGraphTest : MonoBehaviour {

    Robot r;
    float x = 0, y = 0;

	// Use this for initialization
	void Start () {
        r = new Robot("a");
        r.SetVariable("x", 0.0f);
        r.SetVariable("y", 0.0f);

        GameObject obj = GameObject.Find("RobotTarget1");
        LineGraph lg = new LineGraph("x", "y", r);
        LineGraphContainer c = obj.AddComponent<LineGraphContainer>();
        c.visualization = lg;
	}
	
	// Update is called once per frame
	void Update () {
        x += 1;
        y = Mathf.Sin(2 * Mathf.PI * x / 120);

        r.SetVariable("x", x);
        r.SetVariable("y", y);
    }
}
