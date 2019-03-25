using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for creating a tag 
/// </summary>
public class CreateTag : MonoBehaviour
{
    public List<Toggle> allToggles = new List<Toggle>(); //List of toggles
    public List<Toggle> allChecked = new List<Toggle>(); //List of checked toggles
    public bool updateToggles; //If they need to be updated
    public HashSet<string> prevCheckedRobots = new HashSet<string> { };//List of robots that are checked during touch robots

    //UI objects
    public Button sendTags;
    public Button back;
    public GameObject panel; //Robot panel
    public InputField tagName;
    public Toggle t1;
    public Toggle t2;
    public Toggle t3;
    public Toggle t4;
    public Toggle t5;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
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
    /// <summary>
    ///  Update is called once per frame
    /// </summary>
    void Update()
    {
        if (panel != null)
        {
            if (panel.activeSelf)
            {
                if (updateToggles)
                {
                    prevCheckedRobots = UIManager.Instance.touchedRobots;
                    if (prevCheckedRobots.Count > 0) { addprevCheckedRobots(); }
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

    /// <summary>
    /// Send the tag to the UIManager
    /// </summary>
    public void sendTag()
    {
        string name = tagName.text;
        List<Robot> robotsTag = new List<Robot> { };//Robots for the current graph

        if (t1.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget1")); }
        if (t2.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget2")); }
        if (t3.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget3")); }
        if (t4.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget4")); }
        if (t5.isOn) { robotsTag.Add(DataManager.Instance.GetRobot("RobotTarget5")); }
        UIManager.Instance.CreateTag(name, robotsTag);
        ClearToggles();
    }

    /// <summary>
    /// Clears if any toggles have been checked
    /// </summary>
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

    /// <summary>
    /// If any robots have been checked in touch they will be checked on
    /// </summary>
    void addprevCheckedRobots()
    {
        if (prevCheckedRobots.Contains("r1")) { t1.isOn = true; }
        if (prevCheckedRobots.Contains("r2")) { t2.isOn = true; }
        if (prevCheckedRobots.Contains("r3")) { t3.isOn = true; }
        if (prevCheckedRobots.Contains("r4")) { t4.isOn = true; }
        if (prevCheckedRobots.Contains("r5")) { t5.isOn = true; }
    }
}
