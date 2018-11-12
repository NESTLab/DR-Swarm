using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieGraph : MonoBehaviour {
    //should eventually make all of these private 
    //eventually need to instantiate these in start() the same way wedges is
    public float[] data;
    public Color[] wedgeColors;

    public Image wedgePrefab;
    public Image[] wedges;

    public float total = 0f;
    public float zRotation = 0f;

    // Use this for initialization
    void Start() {
        data[0] = 5f;
        data[1] = 10f;
        wedges = new Image[2];
        //set the total
        for (int i = 0; i < data.Length; i++) {
            total += data[i];
        }
        //initialize the wedges
        for (int i = 0; i < data.Length; i++) {
            Debug.Log("making new wedge");
            Image newWedge = Instantiate(wedgePrefab) as Image;
            wedges[i] = newWedge;
        }
        MakeGraph();
    }

    //update the pie graph
    void MakeGraph() {
        zRotation = 0f;
        for (int i = 0; i < data.Length; i++) {
            Debug.Log("updating wedge");
            Image wedge = wedges[i];
            wedge.transform.SetParent(transform, false);
            wedge.color = wedgeColors[i];
            wedge.fillAmount = data[i] / total;
            wedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= wedge.fillAmount * 360f;
            wedges[i] = wedge; //this probably isn't necessary, but might as well
        }
    }

    // Update is called once per frame
    void Update() {
        // just for progress bar
        zRotation = 0f;

        if(data[0]/total < 1) {
            data[0] += .01f;
            data[1] = total - data[0];
        }
        else {
            Debug.Log("done");
            data[0] = total;
            data[1] = 0f;
        }
        
        MakeGraph();
    }
}
