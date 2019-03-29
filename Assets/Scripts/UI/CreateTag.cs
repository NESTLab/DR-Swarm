using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DrSwarm.Model;

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
    public Button sendTags;
    public Button back;
    public HashSet<string> prevCheckedRobots = new HashSet<string> { };

    public InputField tagName;
    // Start is called before the first frame update
    void Start()
    {
        prevCheckedRobots = UIManager.Instance.touchedRobots;
        allToggles.Add(t1);
        allToggles.Add(t2);
        allToggles.Add(t3);
        allToggles.Add(t4);
        allToggles.Add(t5);
        sendTags.onClick.AddListener(sendTag);
        back.onClick.AddListener(ClearToggles);
        if (prevCheckedRobots.Count > 0)
        {
            addprevCheckedRobots();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (panel != null)
        {
            if (panel.activeSelf)
            {
                if (updateToggles)
                {
                    prevCheckedRobots = UIManager.Instance.touchedRobots;

                    if (prevCheckedRobots.Count > 0)
                    {
                        addprevCheckedRobots();
                    }

                    updateToggles = false;
                }
            }
            else
            {
                updateToggles = true;
            }
        }
        else
        {
            updateToggles = true;
        }

    }

    public void sendTag()
    {

        string name = tagName.text;
        List<Robot> robotsTag = new List<Robot> { };//Robots for the current graph

        if (t1.isOn) { Debug.Log("ON"); robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget1")); }
        if (t2.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget2")); }
        if (t3.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget3")); }
        if (t4.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget4")); }
        if (t5.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget5")); }
        UIManager.Instance.CreateTag(name, robotsTag);
        ClearToggles();
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


    void addprevCheckedRobots()
    {
        if (prevCheckedRobots.Contains("r1"))
        {
            Debug.Log("true");
            t1.isOn = true;
        }
        if (prevCheckedRobots.Contains("r2"))
        {
            t2.isOn = true;
        }
        if (prevCheckedRobots.Contains("r3"))
        {
            t3.isOn = true;
        }
        if (prevCheckedRobots.Contains("r4"))
        {
            t4.isOn = true;
        }
        if (prevCheckedRobots.Contains("r5"))
        {
            t5.isOn = true;
        }

    }

}
