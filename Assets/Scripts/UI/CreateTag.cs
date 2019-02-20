using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTag : MonoBehaviour
{
    public Toggle t1;
    public Toggle t2;
    public Toggle t3;
    public Toggle t4;
    public Toggle t5;
    public List<Toggle> allToggles = new List<Toggle>();
    public List<Toggle> allChecked = new List<Toggle>();
    public GameObject panel; //Robot panel
    public bool updateToggles;

    public InputField tagName;
    // Start is called before the first frame update
    void Start()
    {
        allToggles.Add(t1);
        allToggles.Add(t2);
        allToggles.Add(t3);
        allToggles.Add(t4);
        allToggles.Add(t5);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void sendTag()
    {
        List<Robot> robotsTag = new List<Robot> { };//Robots for the current graph

        if (t1.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget1")); }
        if (t2.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget2")); }
        if (t3.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget3")); }
        if (t4.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget4")); }
        if (t5.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget5")); }

    }


    public void ClearToggles()
    {
        allChecked.Clear();
        foreach (Toggle t in allToggles)
        {
            t.interactable = true;
            t.isOn = false;
        }
        updateToggles = true;

    }
}
