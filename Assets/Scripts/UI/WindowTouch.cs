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
    public string robotName5;


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
            string parentname = hit.collider.gameObject.transform.parent.ToString();
            if (parentname.Equals(robotName1))
            {
                UIManager.Instance.RobotToSwitchVisualizations = stripOffEnd(robotName1);
            }
            if (parentname.Equals(robotName2))
            {
                UIManager.Instance.RobotToSwitchVisualizations = stripOffEnd(robotName2);
            }
            if (parentname.Equals(robotName3))
            {
                UIManager.Instance.RobotToSwitchVisualizations = stripOffEnd(robotName3);
            }
            if (parentname.Equals(robotName4))
            {
                UIManager.Instance.RobotToSwitchVisualizations = stripOffEnd(robotName4);
            }
            if (parentname.Equals(robotName5))
            {
                UIManager.Instance.RobotToSwitchVisualizations = stripOffEnd(robotName5);
            }
        }
    }

    /// <summary>
    /// Set the robot name
    /// </summary>
    /// <param name="robot">name of the robot</param>
    public void SwitchVizOnWindow(string robot)
    {
        robotName1 = robot;
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
