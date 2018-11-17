using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour {

    VariableDict dict;
    Vector3 lastPosition;

    // Use this for initialization
    void Start() {
        lastPosition = new Vector3(0, 0, 0);

        string robotName = gameObject.name;
        dict = DataManager.Instance.GetRobotDict(robotName);
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
        dict.SetValue("position_x", position.x);
        dict.SetValue("position_y", position.y);
        dict.SetValue("position_z", position.z);
    }
}
