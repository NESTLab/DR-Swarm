using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehavior : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    public int clickState;
    public Transform target;
    public float x = 0f;
    public float y = 0f;
    public float z = .15f;
    public Transform screen;


    // Use this for initialization
    void Start () {
        target.SetParent(screen);
	}
	
	// Update is called once per frame
	void Update () {
        MoveObject(target, x, y, z);
	}


    void MoveObject(Transform transform, float x, float y, float z)
    {
        Vector3 newPos = new Vector3(x, y, z);
        Debug.Log(transform.transform.position);
        //transform.Translate(0, 0, Time.deltaTime);
        transform.transform.position = newPos;
        Debug.Log(transform.transform.position);
    }
}
