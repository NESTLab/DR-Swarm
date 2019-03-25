using graphNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SendVars : MonoBehaviour
{
    // Start is called before the first frame update

    //Toggles on the UI, represent one for each robot
    //Is there a way to do this dynamically/better?
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


    public Toggle selectAll;
    public List<Toggle> allToggles = new List<Toggle>();
    public List<Toggle> allChecked = new List<Toggle>();
    public HashSet<string> prevCheckedRobots = new HashSet<string> { };
    public List<string> editRobots = new List<string>();
    public GameObject panel; //Robot panel 
    public bool updateToggles = false;
    public GameObject tagPanel;

    //Var panels
    public GameObject basicVarPanel;
    public GameObject mapPanel;
    public GameObject rangePanel;


    void Start()
    {

        if (t1 != null)
        {
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
                t.onValueChanged.AddListener(delegate
                {
                    //toggleDisable(t);
                    TurnOffSelectAll(t);
                });
            }
            
        }
        if (prevCheckedRobots.Count > 0 || editRobots.Count > 0)
        {
            Debug.Log("CHECKED ROBTS " + editRobots);
            addprevCheckedRobots();
        }

    }

    void addprevCheckedRobots()
    {
        if (prevCheckedRobots.Contains("r1") || editRobots.Contains("RobotTarget1"))
        {
            Debug.Log("true");
            t1.isOn = true;
        }
        if (prevCheckedRobots.Contains("r2") || editRobots.Contains("RobotTarget2"))
        {
            t2.isOn = true;
        }
        if (prevCheckedRobots.Contains("r3") || editRobots.Contains("RobotTarget3"))
        {
            t3.isOn = true;
        }
        if (prevCheckedRobots.Contains("r4") || editRobots.Contains("RobotTarget4"))
        {
            t4.isOn = true;
        }
        if (prevCheckedRobots.Contains("r5") || editRobots.Contains("RobotTarget5"))
        {
            t5.isOn = true;
        }
        if (prevCheckedRobots.Contains("r6") || editRobots.Contains("RobotTarget6")) 
        {
            t6.isOn = true;
        }
        if (prevCheckedRobots.Contains("r7") || editRobots.Contains("RobotTarget7"))
        {
            t7.isOn = true;
        }
        if (prevCheckedRobots.Contains("r8") || editRobots.Contains("RobotTarget8"))
        {
            t8.isOn = true;
        }
        if (prevCheckedRobots.Contains("r9") || editRobots.Contains("RobotTarget9"))
        {
            t9.isOn = true;
        }
        if (prevCheckedRobots.Contains("r10") || editRobots.Contains("RobotTarget10"))
        {
            t10.isOn = true;
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
                    editRobots = UIManager.Instance.editVizRobots;

                    if (prevCheckedRobots.Count > 0 || editRobots.Count > 0)
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


    public void selectAllToggles()
    {
        if (selectAll.isOn)
        {
            foreach (Toggle t in allToggles)
            {
                t.isOn = true;
            }
        }
        else {
            foreach (Toggle t in allToggles)
            {
                t.isOn = false;
            }
        }

    }

    public void TurnOffSelectAll(Toggle t)
    {
        if (!t.isOn)
        {
            selectAll.isOn = false;
        }

    }
    private void toggleDisable(Toggle check)
    {
        if (UIManager.Instance.RobotOptions > 0)
        {
            if (check.isOn)
            {
                allChecked.Add(check);
                if (allChecked.Count <= UIManager.Instance.RobotOptions)
                {
                    foreach (Toggle t in allToggles)
                    {
                        if (check != t)
                        {
                            t.interactable = false;
                        }
                    }
                }
            }
            else
            {
                allChecked.Remove(check);
                foreach (Toggle t in allToggles)
                {
                    t.interactable = true;
                }
            }
        }
        else
        {

            allChecked.Remove(check);
            foreach (Toggle t in allToggles)
            {
                t.interactable = true;
            }
        }
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



    //Called when the next button is pressed
    //Checks for each toggle to tell if it is on, if its on, need to add the robot
    //Also calls addGraph
    public void toggleAdd()
    {
        if (t1.isOn)
        {
            UIManager.Instance.AddRobot("r1");
        }
        if (t2.isOn)
        {
            UIManager.Instance.AddRobot("r2");
        }
        if (t3.isOn)
        {
            UIManager.Instance.AddRobot("r3");
        }
        if (t4.isOn)
        {
            UIManager.Instance.AddRobot("r4");
        }
        if (t5.isOn)
        {
            UIManager.Instance.AddRobot("r5");
        }
        if (t6.isOn)
        {
            UIManager.Instance.AddRobot("r6");
        }
        if (t7.isOn)
        {
            UIManager.Instance.AddRobot("r7");
        }
        if (t8.isOn)
        {
            UIManager.Instance.AddRobot("r8");
        }
        if (t9.isOn)
        {
            UIManager.Instance.AddRobot("r9");
        }
        if (t10.isOn)
        {
            UIManager.Instance.AddRobot("r10");
        }
        //UIManager.Instance.addGraph = true;
        updateToggles = true;
        List<string> checkedTags = new List<string>();
        int children = tagPanel.transform.childCount;


        foreach (Transform child in tagPanel.transform)
        {
            //GameObject g = child.gameObject.GetComponent<GameObject>();
            //Toggle t = g.gameObject.GetComponent<Toggle>();
            for (int i = 0; i < child.childCount; ++i)
            {
                Transform currentItem = child.GetChild(i);
                if (currentItem.GetComponent<Toggle>() != null)
                {
                    Toggle t = currentItem.GetComponent<Toggle>();
                    if (t.isOn)
                    {
                        Debug.Log("IT WAS ON");
                        checkedTags.Add(t.GetComponentInChildren<Text>().text);
                    }
                }
            }
        }

        if (checkedTags.Count > 0)
        {
            UIManager.Instance.checkedTagNames = checkedTags;
            UIManager.Instance.addCheckedTags();
        }


        UIManager.Instance.touchedRobots.Clear();
    }
    public Component[] ts;

    public void setToGraph(string i)
    {
        UIManager.Instance.sentGraphType = i;
    }

    public void addRobotMode(Slider s)
    {
        if (s.value == 1)
        {
            UIManager.Instance.AddRobotMode = true;
        }
        else
        {
            UIManager.Instance.AddRobotMode = false;
        }
    }


    public void setPanel()
    {
        graph UIgraph = UIManager.Instance.GraphType;
        if (UIgraph == graph.TwoDRange)
        {
            rangePanel.gameObject.SetActive(true);
        }
        else if (UIgraph == graph.TwoDMap)
        {
            mapPanel.gameObject.SetActive(true);
        }
        else
        {
            basicVarPanel.gameObject.SetActive(true);
        }

    }


    public void ClearAddedRobots()
    {
        UIManager.Instance.touchedRobots = new HashSet<string>();
    }

}
