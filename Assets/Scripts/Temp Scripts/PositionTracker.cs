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
        dict = DataModel.Instance.GetRobotDict(robotName);
        dict.SetValue("position", lastPosition);
    }

    // Update is called once per frame
   	void Update() {
        if ((lastPosition - transform.position).magnitude > 0.0001)
        {
            dict.SetValue("position", transform.position);
        }

        lastPosition = transform.position;
	}
}
