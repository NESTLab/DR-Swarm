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
                    Debug.Log("name" + hit.collider.gameObject.name); //RobotTarget1, RobotTarget2
                    if (hit.collider.gameObject.name == "RobotTarget1")
                    {
                        //GameObject obj = GameObject.FindGameObjectWithTag("imageTarget1");
                        //Debug.Log("Console clicked target" + obj);
                        UIManager.Instance.addRobotByTouch("r1");
                    }
                    if (hit.collider.gameObject.name == "RobotTarget2")
                    {
                        //GameObject obj = GameObject.FindGameObjectWithTag("imageTarget2");
                        //Debug.Log("Console clicked target" + obj);
                        UIManager.Instance.addRobotByTouch("r2");

                    }
                }
            }
        }
    }
}

