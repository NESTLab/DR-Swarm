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
                        UIManager.Instance.addRobotByTouch("r1");
                    }
                    else if (hit.collider.gameObject.name == "RobotTarget2")
                    {
                        UIManager.Instance.addRobotByTouch("r2");
                    }
                    else if (hit.collider.gameObject.name == "RobotTarget3")
                    {
                        UIManager.Instance.addRobotByTouch("r3");
                    }
                    else if (hit.collider.gameObject.name == "RobotTarget4")
                    {
                        UIManager.Instance.addRobotByTouch("r4");
                    }
                    else if (hit.collider.gameObject.name == "RobotTarget5")
                    {
                        UIManager.Instance.addRobotByTouch("r5");
                    }
                    else if (hit.collider.gameObject.name == "RobotTarget6")
                    {
                        UIManager.Instance.addRobotByTouch("r6");
                    }
                    else if (hit.collider.gameObject.name == "RobotTarget7")
                    {
                        UIManager.Instance.addRobotByTouch("r7");
                    }
                    else if (hit.collider.gameObject.name == "RobotTarget8")
                    {
                        UIManager.Instance.addRobotByTouch("r8");
                    }
                    else if (hit.collider.gameObject.name == "RobotTarget9")
                    {
                        UIManager.Instance.addRobotByTouch("r9");
                    }
                    else if (hit.collider.gameObject.name == "RobotTarget10")
                    {
                        UIManager.Instance.addRobotByTouch("r10");
                    }
                }
            }
        }
    }
}

