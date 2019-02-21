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
    public List<Toggle> allToggles = new List<Toggle>();
    public List<Toggle> allChecked = new List<Toggle>();
    public HashSet<string> prevCheckedRobots = new HashSet<string> { };
    public GameObject panel; //Robot panel 
    public bool updateToggles = false;
    public GameObject tagPanel;

    //Var panels
    public GameObject basicVarPanel;
    public GameObject mapPanel;
    public GameObject rangePanel;


    void Start()
    {
        prevCheckedRobots = UIManager.Instance.touchedRobots;
        if (t1 != null)
        {
            allToggles.Add(t1);
            allToggles.Add(t2);
            allToggles.Add(t3);
            allToggles.Add(t4);
            allToggles.Add(t5);

            foreach (Toggle t in allToggles)
            {
                t.onValueChanged.AddListener(delegate
                {
                    toggleDisable(t);
                });
            }
        }
        if (prevCheckedRobots.Count > 0)
        {

            if (prevCheckedRobots.Contains("r1"))
            {
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
                        Debug.Log(prevCheckedRobots.Count);
                        Debug.Log("cehckign r2" + prevCheckedRobots.Contains("r2"));

                        if (prevCheckedRobots.Contains("r1"))
                        {
                            t1.isOn = true;
                        }
                        if (prevCheckedRobots.Contains("r2"))
                        {
                            Debug.Log("Checking t2");
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
        //UIManager.Instance.addGraph = true;
        updateToggles = true;
        List<string> checkedTags = new List<string>();
        int children = tagPanel.transform.childCount;



        //foreach (HingeJoint joint in hingeJoints)
        //   joint.useSpring = false;


        Component[] prefabs;

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


}
