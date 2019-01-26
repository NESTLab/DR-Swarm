using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddRobots : MonoBehaviour
{
    // Start is called before the first frame update

    //Toggles on the UI, represent one for each robot
    //Is there a way to do this dynamically/better?
    public Toggle t1; 
    public Toggle t2;
    public Toggle t3;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        UIManager.Instance.addGraph = true;
        
    }

    
}
