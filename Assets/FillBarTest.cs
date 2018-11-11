using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBarTest : MonoBehaviour {
    public float percentage; //dummy value for now, goes between 0 and 1
    public Vector3 originalSize;
    public Vector3 up;
    public Vector3 down;
    public Vector3 direction;
    //empty part of progress bar
    [SerializeField] private GameObject emptyContainer;

	// Use this for initialization
	void Start () {
        up = new Vector3(0, 0, 0.005f);
        down = new Vector3(0, 0, -0.005f);
        direction = up;

        originalSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z) * 2;
        percentage = 0.25f;
	}
	
	// Update is called once per frame
	void Update () {
        
        if(transform.localScale.z > .4f && direction == up) {
            direction = down;
        }
        else if(transform.localScale.z < 0.02f && direction == down) {
            direction = up;
        }

        transform.localScale += direction;

        emptyContainer.transform.localScale -= direction;
        

        /*
        transform.localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z * percentage);
        emptyContainer.transform.localScale = new Vector3(originalSize.x, originalSize.y, originalSize.z * (1 - percentage));
   */
    }
}
