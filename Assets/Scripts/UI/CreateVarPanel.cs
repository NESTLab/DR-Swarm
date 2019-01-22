using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateVarPanel : MonoBehaviour
{
    public Dropdown d1;
        List<string> m_DropOptions = new List<string> {};
    public GameObject varPanel;
    public Text t1;
    // Start is called before the first frame update
    private int currOptions =1;
    float offset = -50f;
    public List<Dropdown> dropdowns = new List<Dropdown>{};

    void Start()
    {// this.data.Keys.ToArray();
        Robot r1 = DataManager.Instance.GetRobot("RobotTarget1");
        //m_DropOptions=r1.data.Keys.ToArray();
        varPanel =  GameObject.Find("OptionPanel");
        d1.ClearOptions();
        d1.AddOptions(m_DropOptions);
        int options = VisualizationManager.Instance.Options;
        if (options > 1) {
            for(int i = 0; i < options - 1 ; i++) {
                Vector3 pos = new Vector3(d1.transform.position.x, (d1.transform.position.y + offset), d1.transform.position.z);
                Vector3 post = new Vector3(t1.transform.position.x, (t1.transform.position.y + offset), t1.transform.position.z);
                Quaternion q = d1.transform.rotation; 
                Dropdown  d2 = Instantiate(d1, pos, q, varPanel.transform);//, pos, d1.transform.rotation);
                Text t2 = Instantiate(t1, post, q, varPanel.transform);//, pos, d1.transform.rotation);
                t2.text = "Variable " + (i+2);
                offset = offset + offset;
                dropdowns.Add(d2);
            }
            
            currOptions = options;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int options = VisualizationManager.Instance.Options;
        if (options > currOptions) {
            for(int i = 0; i < options - 1 ; i++) {
                Vector3 pos = new Vector3(d1.transform.position.x, (d1.transform.position.y + offset), d1.transform.position.z);
                Vector3 post = new Vector3(t1.transform.position.x, (t1.transform.position.y + offset), t1.transform.position.z);
                Quaternion q = d1.transform.rotation; 
                Dropdown  d2 = Instantiate(d1, pos, q, varPanel.transform);//, pos, d1.transform.rotation);
                Text t2 = Instantiate(t1, post, q, varPanel.transform);//, pos, d1.transform.rotation);
                t2.text = "Variable " + (i+2);
                offset = offset +offset;
            }
            
            currOptions = options;
        }
        if (options < currOptions) {
            foreach (Dropdown x in dropdowns){
         //TODO: DESTROY
              //  Destroy(x);
            }
        }

    }


}
