using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (UIManager.Instance.AddRobotMode)
                {
                    if (hit.collider.tag == "imageTarget1")
                    {
                        GameObject obj = GameObject.FindGameObjectWithTag("imageTarget1");
                        Debug.Log("Console clicked target" + obj);
                        UIManager.Instance.addRobotByTouch("r1");
                    }
                    if (hit.collider.tag == "imageTarget2")
                    {
                        GameObject obj = GameObject.FindGameObjectWithTag("imageTarget2");
                        Debug.Log("Console clicked target" + obj);
                        UIManager.Instance.addRobotByTouch("r2");

                    }
                }
            }
        }
    }
}

