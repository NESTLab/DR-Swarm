using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FillBarTest : MonoBehaviour {
    private Vector3 originalSize;
    //private Vector3 up;
    //private Vector3 down;
    //private Vector3 direction;
    //empty part of progress bar
    [SerializeField] private GameObject emptyContainer;

    private VariableDict dict;

    // Use this for initialization
    void Start () {
        //up = new Vector3(0, 0, 0.005f);
        //down = new Vector3(0, 0, -0.005f);
        //direction = up;

        originalSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * 2);

        string robotName = transform.parent.gameObject.name; //name of the image target
        dict = DataModel.Instance.GetRobotDict(robotName);
        dict.SetValue("percentage", 0.25f);
        dict.GetObservableValue<float>("percentage").Subscribe(percentage => {
            transform.localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z * percentage);
            emptyContainer.transform.localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z * (1 - percentage));
        });
    }
	
	// Update is called once per frame
	void Update () {
        float value = dict.GetValue<float>("percentage");

        if(value < 1) {
            value += 0.01f;
        }
        else {
            value = 1;
        }

        dict.SetValue("percentage", value);
        Debug.Log("value: " + value);

        /*
        if(transform.localScale.z > .4f && direction == up) {
            direction = down;
        }
        else if(transform.localScale.z < 0.02f && direction == down) {
            direction = up;
        }
        
        transform.localScale += direction;

        emptyContainer.transform.localScale -= direction;
        */

        /*
        transform.localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z * percentage);
        emptyContainer.transform.localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z * (1 - percentage));
        */
    }
}
