using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    
    // Start is called before the first frame update   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

     
    public string _GraphType = "";
    public int Options = 1;
    public string GraphType {
        get {return _GraphType;}
        set {
            _GraphType = value;
            if(_GraphType == "Line") { Options = 2; }
            else if (_GraphType == "Pie") { Options = 1;}
        }
    }


//ADD VARS
public List<string> variables = new List<string> {"x", "y"};

//ADD ROBOTS
    public List<Robot> robots = new List<Robot>{};

    public void AddRobot(string r) {
        if(r == "r1"){
            robots.Add(DataManager.Instance.GetRobot("RobotTarget1"));//Aparently not finding?
        }
        else if (r =="r2") {
            robots.Add(DataManager.Instance.GetRobot("RobotTarget2"));
        }else if (r =="r3") {
            robots.Add(DataManager.Instance.GetRobot("RobotTarget3"));
        }else if (r =="r4") {
            robots.Add(DataManager.Instance.GetRobot("RobotTarget4"));
        }else if (r =="r5") {
            robots.Add(DataManager.Instance.GetRobot("RobotTarget5"));
        }
    }

    public void AddAllRobots(List<string> robots) {
        foreach(string r in robots){
            AddRobot(r);
        }
    }

//ADD GRAPH
    public bool _addGraph = false;
    public bool addGraph {
        get {return _addGraph;}
        set {
            _addGraph = value;
            Debug.Log("add");
            if(_addGraph ==true) {createGraph(); }// Add graph here

        }
    }


    public void createGraph(){
        Debug.Log("GraphEntered");
        if(GraphType =="Line"){
            Debug.Log("Graph");
            string xvar = variables[0];
            string yvar = variables[1];
            Robot r1 = robots[0];
            robots.RemoveAt(0);
            IVisualization lg = new LineGraph("x", "y", r1, robots.ToArray());
            string title = xvar+","+yvar+" Line";//TODO: Add another unique symbol to this?
            VisualizationManager.Instance.AddVisualization(title, lg);

        }
    }



    #region SINGLETON PATTERN
    public static UIManager _instance;
    public static UIManager Instance
    {
         get {
             if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIManager>();
             
                if (_instance == null)
                {
                    GameObject container = new GameObject("UIManager");
                    _instance = container.AddComponent<UIManager>();
                }
            }
     
            return _instance;
        }
    }
    #endregion

    public UIManager()
    {

    }



    


}
