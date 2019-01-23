using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddRobots : MonoBehaviour
{
    // Start is called before the first frame update

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


    public void toggleAdd() {
        if(t1.isOn) {
            UIManager.Instance.AddRobot("r1");
        }
        if(t2.isOn) {
            UIManager.Instance.AddRobot("r2");
        }
        UIManager.Instance.add = true;
        
    }

    
}
