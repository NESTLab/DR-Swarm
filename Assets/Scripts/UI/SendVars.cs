using graphNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for sending vars from one variable to another
/// </summary>
public class SendVars : MonoBehaviour
{
    //Toggles on the UI, represent one for each robot
    public Toggle t1;
    public Toggle t2;
    public Toggle t3;
    public Toggle t4;
    public Toggle t5;
    public Toggle t6;
    public Toggle t7;
    public Toggle t8;
    public Toggle t9;
    public Toggle t10;
    //other game objects
    public Toggle selectAll;
    public GameObject tagPanel;
    public GameObject panel; //Robot panel 

    public List<Toggle> allToggles = new List<Toggle>(); //list of all toggles
    public List<Toggle> allChecked = new List<Toggle>(); //list of all toggles that are checked
    public HashSet<string> prevCheckedRobots = new HashSet<string> { }; //robots added through touch
    public List<string> editRobots = new List<string>(); //list of robots from edit
    public bool updateToggles = false; //if toggles(on/off) need to be updated

    //Var panels for switching panels
    public GameObject basicVarPanel;
    public GameObject mapPanel;
    public GameObject rangePanel;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        if (t1 != null) //if toggle this there
        {
            //set up toggles
            ClearToggles();
            editRobots = UIManager.Instance.editVizRobots;
            selectAll.onValueChanged.AddListener(delegate { selectAllToggles(); });
            prevCheckedRobots = UIManager.Instance.touchedRobots;
            allToggles.Add(t1);
            allToggles.Add(t2);
            allToggles.Add(t3);
            allToggles.Add(t4);
            allToggles.Add(t5);
            allToggles.Add(t6);
            allToggles.Add(t7);
            allToggles.Add(t8);
            allToggles.Add(t9);
            allToggles.Add(t10);

            foreach (Toggle t in allToggles)
            {
                t.onValueChanged.AddListener(delegate { TurnOffSelectAll(t); });
            }

        }
        //If we need to add in touch or edit robots
        if (prevCheckedRobots.Count > 0 || editRobots.Count > 0)
        {
            addprevCheckedRobots();
        }

    }


    /// <summary>
    ///  Update is called once per frame
    /// </summary>
    void Update()
    {
        if (panel != null) //check if the panel is there
        {
            if (panel.activeSelf)
            {
                if (updateToggles) // update the toggles
                {
                    prevCheckedRobots = UIManager.Instance.touchedRobots;
                    editRobots = UIManager.Instance.editVizRobots;
                    if (prevCheckedRobots.Count > 0 || editRobots.Count > 0) { addprevCheckedRobots(); }
                    updateToggles = false;
                }
            }
            else { updateToggles = true; }
        }
        else { updateToggles = true; }
    }

    /// <summary>
    /// Check toggles if they need to be checked on
    /// </summary>
    void addprevCheckedRobots()
    {
        if (prevCheckedRobots.Contains("r1") || editRobots.Contains("RobotTarget1")) { t1.isOn = true; }
        if (prevCheckedRobots.Contains("r2") || editRobots.Contains("RobotTarget2")) { t2.isOn = true; }
        if (prevCheckedRobots.Contains("r3") || editRobots.Contains("RobotTarget3")) { t3.isOn = true; }
        if (prevCheckedRobots.Contains("r4") || editRobots.Contains("RobotTarget4")) { t4.isOn = true; }
        if (prevCheckedRobots.Contains("r5") || editRobots.Contains("RobotTarget5")) { t5.isOn = true; }
        if (prevCheckedRobots.Contains("r6") || editRobots.Contains("RobotTarget6")) { t6.isOn = true; }
        if (prevCheckedRobots.Contains("r7") || editRobots.Contains("RobotTarget7")) { t7.isOn = true; }
        if (prevCheckedRobots.Contains("r8") || editRobots.Contains("RobotTarget8")) { t8.isOn = true; }
        if (prevCheckedRobots.Contains("r9") || editRobots.Contains("RobotTarget9")) { t9.isOn = true; }
        if (prevCheckedRobots.Contains("r10") || editRobots.Contains("RobotTarget10")) { t10.isOn = true; }
    }

    /// <summary>
    /// Added to the select all toggle
    /// </summary>
    public void selectAllToggles()
    {
        if (selectAll.isOn)
        {
            foreach (Toggle t in allToggles)
            {
                t.isOn = true;
            }
        }
        else
        {
            foreach (Toggle t in allToggles)
            {
                t.isOn = false;
            }
        }

    }

    /// <summary>
    /// Turn off the select all toggle if another toggle is selected
    /// </summary>
    /// <param name="t">toggle this is attached to</param>
    public void TurnOffSelectAll(Toggle t)
    {
        if (!t.isOn) { selectAll.isOn = false; }

    }

    /// <summary>
    /// Turn all toggles off (robot panel)
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
    /// Called when the next button is pressed
    /// Checks for each toggle to tell if it is on, if its on, need to add the robot
    /// Also calls addGraph
    /// </summary>
    public void toggleAdd()
    {
        //Check if the toggle is on
        if (t1.isOn) { UIManager.Instance.AddRobot("r1"); }
        if (t2.isOn) { UIManager.Instance.AddRobot("r2"); }
        if (t3.isOn) { UIManager.Instance.AddRobot("r3"); }
        if (t4.isOn) { UIManager.Instance.AddRobot("r4"); }
        if (t5.isOn) { UIManager.Instance.AddRobot("r5"); }
        if (t6.isOn) { UIManager.Instance.AddRobot("r6"); }
        if (t7.isOn) { UIManager.Instance.AddRobot("r7"); }
        if (t8.isOn) { UIManager.Instance.AddRobot("r8"); }
        if (t9.isOn) { UIManager.Instance.AddRobot("r9"); }
        if (t10.isOn) { UIManager.Instance.AddRobot("r10"); }

        updateToggles = true;
        List<string> checkedTags = new List<string>();
        int children = tagPanel.transform.childCount;

        //Check if any tags in the panel are done
        foreach (Transform child in tagPanel.transform)
        {
            for (int i = 0; i < child.childCount; ++i)
            {
                Transform currentItem = child.GetChild(i);
                if (currentItem.GetComponent<Toggle>() != null)
                {
                    Toggle t = currentItem.GetComponent<Toggle>();
                    if (t.isOn) { checkedTags.Add(t.GetComponentInChildren<Text>().text); }
                }
            }
        }

        //If any tags are selected
        if (checkedTags.Count > 0)
        {
            UIManager.Instance.checkedTagNames = checkedTags;
            UIManager.Instance.addCheckedTags();
        }
        UIManager.Instance.touchedRobots.Clear();
    }

    /// <summary>
    /// Send the graph type
    /// </summary>
    /// <param name="i">graph type (through unity)</param>
    public void setToGraph(string i)
    {
        UIManager.Instance.sentGraphType = i;
    }

    /// <summary>
    /// Attached to main panel for the robots, to know if the user is using touch robots
    /// </summary>
    /// <param name="s"></param>
    public void addRobotMode(Slider s)
    {
        if (s.value == 1) { UIManager.Instance.AddRobotMode = true; }
        else { UIManager.Instance.AddRobotMode = false; }
    }

    /// <summary>
    /// Change which panel needs tobe next (for the variables)
    /// </summary>
    public void setPanel()
    {
        graph UIgraph = UIManager.Instance.GraphType;
        if (UIgraph == graph.TwoDRange) { rangePanel.gameObject.SetActive(true); }
        else if (UIgraph == graph.TwoDMap) { mapPanel.gameObject.SetActive(true); }
        else { basicVarPanel.gameObject.SetActive(true); }

    }

    /// <summary>
    /// Clear the robots that were added through the touch
    /// </summary>
    public void ClearAddedRobots()
    {
        UIManager.Instance.touchedRobots = new HashSet<string>();
    }

}
