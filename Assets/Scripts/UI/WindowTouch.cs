using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to a VisualizationWindow
/// </summary>
public class WindowTouch : MonoBehaviour
{
    public string robotName1;
    public string robotName2;
    public string robotName3;
    public string robotName4;


    // Start is called before the first frame update
    void Start()
    { }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Pressed left click, casting ray.");
            CastRay();
        }
    }

    void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit)
        {
            //Debug.Log("Object is " + hit.collider.gameObject.name + "  " + robotName);
            //Debug.Log("Parent is  !" + hit.collider.gameObject.transform.parent.ToString() +"!");
            //Debug.Log(hit.collider.gameObject.transform.parent.ToString().Equals(robotName.ToString()));
            string parentname = hit.collider.gameObject.transform.parent.ToString();
            if (parentname.Equals(robotName1))
            {
                Debug.Log("IT WORKED1!" + stripOffEnd(robotName1)+"!");
                UIManager.Instance.RobotToSwitchVisualizations = stripOffEnd(robotName1);
            }
            if (parentname.Equals(robotName2))
            {
                Debug.Log("IT WORKED2");
            }
            if (parentname.Equals(robotName3))
            {
                Debug.Log("IT WORKED3");
            }
            if (parentname.Equals(robotName4))
            {
                Debug.Log("IT WORKED4");
            }
        }
    }

    /*
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            { // if left button pressed...
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.name == "VisualizationCanvas") // if the name of the gameobject is 
                    {
                        Debug.Log("HIT");
                    }
                    Debug.Log("HIT" + hit.collider.gameObject.name);
                }
            }
        }
        */
    /// <summary>
    /// Set the robot name
    /// </summary>
    /// <param name="robot">name of the robot</param>
    public void SwitchVizOnWindow(string robot)
    {
        robotName1 = robot;
        //Debug.Log("Swithcing on robot" + robotName);
    }

    /// <summary>
    /// Remove the (UnityEngine.Transform) from the parent name
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private string stripOffEnd(string s)
    {
        return s.Substring(0, 12);
    }
}
