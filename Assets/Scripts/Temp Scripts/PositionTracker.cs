using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrSwarm.Model;

public class PositionTracker : MonoBehaviour {

    Robot robot;
    Vector3 lastPosition;

    // Use this for initialization
    void Start() {
        lastPosition = new Vector3(0, 0, 0);

        string robotName = gameObject.name;
        robot = DataManager.Instance.GetRobot(robotName);
    }

    // Update is called once per frame
   	void Update() {
        if ((lastPosition - transform.position).magnitude > 0.005)
        {
            SetPosition(transform.position);
            lastPosition = transform.position;
        }
	}

    void SetPosition(Vector3 position)
    {
        robot.SetVariable("position_x", position.x);
        robot.SetVariable("position_y", position.y);
        robot.SetVariable("position_z", position.z);
    }
}
