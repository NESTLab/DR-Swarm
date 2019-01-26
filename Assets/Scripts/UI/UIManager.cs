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

    public int Options = 1;//How many min variable options
    //TODO: Change to ENUM 
    private string _GraphType = ""; //What type of graph is currently being set
    public string GraphType {
        get {return _GraphType;}
        set {
            _GraphType = value;
            //Set options based on type of graph
            if(_GraphType == "Line") { Options = 2; } 
            else if (_GraphType == "Pie") { Options = 1;}
        }
    }


    //ADD VARS
    //TODO: Make this automatic? From robots, from datamangager?
    public List<string> variables = new List<string> {"x", "y"};

    //ADD ROBOTS
    private List<Robot> robots = new List<Robot>{};//Robots for the current graph

    //Add a robot to the array to get the total robots for the graph
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

    //If there is a known list of robots to add, currently not using
    public void AddAllRobots(List<string> robots) {
        foreach(string r in robots){
            AddRobot(r);
        }
    }

    //ADD GRAPH
    private bool _addGraph = false; //Var to tell when to add a graph
    public bool addGraph {
        get {return _addGraph;}
        set {
            _addGraph = value;
            Debug.Log("add");
            if(_addGraph == true) {createGraph(); }// Add graph here

        }
    }

    //Create a graph, Needed are robots, variables, type
    //Calls VizManager to add the graph, currently adds title as well
    private void createGraph(){
        Robot r1 = robots[0];
        robots.RemoveAt(0);
        if (GraphType =="Line"){
            string xvar = variables[0];
            string yvar = variables[1];
            IVisualization graph = new LineGraph("x", "y", r1, robots.ToArray());
            string title = xvar+","+yvar+" Line";//TODO: Add another unique symbol to this?
        }
        VisualizationManager.Instance.AddVisualization(title, graph);
        _addGraph = false;
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

    public UIManager()//Constructor
    {

    }



    


}
