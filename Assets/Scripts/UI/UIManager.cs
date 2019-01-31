using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;


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
    public int TotalOptions = 1;

    //TODO: Change to ENUM 
    public enum graph
    {
        Line,
        Pie,
        NoGraph
    }

    public string _sentGraphType = "";

    public graph GraphType = graph.NoGraph; //What type of graph is currently being set
    public string sentGraphType
    {
        get {return _sentGraphType; }
        set {
            _sentGraphType = value;
            //Set options based on type of graph
            Debug.Log("Changing Graph Type");
            if (_sentGraphType == "Line") {
                GraphType = graph.Line;
                Options = 2;
                TotalOptions = 2;
            } 
            else if (_sentGraphType == "Pie") {
                GraphType = graph.Pie;
                Options = 1;
                TotalOptions = 1;
            }
        }
    }


    //ADD VARS
    //TODO: Make this automatic? From robots, from datamangager? <<YES
    public List<string> variables = new List<string> {"x", "y"};
    public List<string> wantedVars = new List<string>();


    //ADD ROBOTS
    private List<Robot> robots = new List<Robot>{};//Robots for the current graph

    //Add a robot to the array to get the total robots for the graph
    public void AddRobot(string r) {
        if (r == "r1"){
            robots.Add(DataManager.Instance.GetRobot("RobotTarget1"));
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
            if(_addGraph == true) {createGraph(); }// Add graph here

        }
    }

    public List<IVisualization> allVizs = new List<IVisualization>(); //temp list of all visualizations
    public List<string> allVizsNames = new List<string>(); //temp list of all visualizations


    //Create a graph, Needed are robots, variables, type
    //Calls VizManager to add the graph, currently adds title as well
    private void createGraph(){
        Robot r1 = robots[0];
        robots.RemoveAt(0);
        string title = "";
        if (GraphType == graph.Line){
            string xvar = wantedVars[0];
            string yvar = wantedVars[1];
            IVisualization graphToAdd = new LineGraph(xvar, yvar, r1, robots.ToArray());
            DateTime foo = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
            title = xvar+","+yvar+" Line " + unixTime;//TODO: Add another unique symbol to this?
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
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
