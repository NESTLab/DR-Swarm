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

    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {

        
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
    }

   

    //Called when the next button is pressed
    //Checks for each toggle to tell if it is on, if its onn, need to add the robot
    //Also calls addGraph
    public void toggleAdd() {
        if(t1.isOn) {
            UIManager.Instance.AddRobot("r1");
        }
        if(t2.isOn) {
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
        UIManager.Instance.addGraph = true;
        
    }

    public void setToGraph(string i)
    {
        UIManager.Instance.sentGraphType = i;
    }


}
