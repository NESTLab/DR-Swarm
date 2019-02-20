using graphNameSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace graphNameSpace
{
    public enum graph
    {
        Line,
        Pie,
        PieMulti,
        Bar,
        TwoDRange,
        TwoDMap,
        NoGraph
    }
}

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
    public int RobotOptions = 0;

    public string _sentGraphType = "";
    public List<RangePolicy> allRPolicies = new List<RangePolicy>();
    

    public graph GraphType = graph.NoGraph; //What type of graph is currently being set
    public string sentGraphType
    {
        get { return _sentGraphType; }
        set
        {
            _sentGraphType = value;
            //Set options based on type of graph
            Debug.Log("Changing Graph Type");
            if (_sentGraphType == "Line")
            {
                GraphType = graph.Line;
                Options = 2;
                TotalOptions = 100;
                RobotOptions = 0;
            }
            else if (_sentGraphType == "Pie")
            {
                GraphType = graph.Pie;
                Options = 1;
                TotalOptions = 1;
                RobotOptions = 0;
            }
            else if (_sentGraphType == "PieMulti")
            {
                GraphType = graph.PieMulti;
                Options = 2;
                RobotOptions = 1;
                TotalOptions = 100;
            }
            else if (_sentGraphType == "Bar")
            {
                GraphType = graph.Bar;
                Options = 1;
                RobotOptions = 0;
                TotalOptions = 100;
            }
            else if (_sentGraphType == "2DMap")
            {
                GraphType = graph.TwoDMap;
                Options = 0;
                RobotOptions = 0;
                TotalOptions = 100;
            }
            else if (_sentGraphType == "2DRange")
            {
                GraphType = graph.TwoDRange;
                Options = 1;
                RobotOptions = 0;
                TotalOptions = 100;
            }
        }
    }

    //TAGS
    public List<Tag> allTags = new List<Tag>();
    //Create a new tag
    public void CreateTag(string name, List<Robot> robots)
    {
        Tag newtag = new Tag(name, robots);
        allTags.Add(newtag);
    }

    //ADD VARS
    public List<string> wantedVars = new List<string>();
    //Get vars from selected robots 
    public void updateTotalVars()
    {
        HashSet<string> varFromRobots = new HashSet<string>();
        //List<Robot> allRobots = new List<Robot>();
        //allRobots.Add(DataManager.Instance.GetRobot("RobotTarget1"));
        //allRobots.Add(DataManager.Instance.GetRobot("RobotTarget2"));
        //allRobots.Add(DataManager.Instance.GetRobot("RobotTarget3"));
        //allRobots.Add(DataManager.Instance.GetRobot("RobotTarget4"));
        //allRobots.Add(DataManager.Instance.GetRobot("RobotTarget5"));
        if (robots.Count > 0)
        {
            varFromRobots.UnionWith(robots[0].GetVariables());
        }
        foreach (Robot r in robots)
        {
            varFromRobots.IntersectWith(r.GetVariables());
        }
        wantedVars = new List<string>(varFromRobots);
    }


    //ADD ROBOTS
    public List<Robot> robots = new List<Robot> { };//Robots for the current graph

    //Add a robot to the array to get the total robots for the graph
    public void AddRobot(string r)
    {
        if (r == "r1") { robots.Add(DataManager.Instance.GetRobot("RobotTarget1")); }
        else if (r == "r2") { robots.Add(DataManager.Instance.GetRobot("RobotTarget2")); }
        else if (r == "r3") { robots.Add(DataManager.Instance.GetRobot("RobotTarget3")); }
        else if (r == "r4") { robots.Add(DataManager.Instance.GetRobot("RobotTarget4")); }
        else if (r == "r5") { robots.Add(DataManager.Instance.GetRobot("RobotTarget5")); }
        else if (r == "r6") { robots.Add(DataManager.Instance.GetRobot("RobotTarget6")); }
        else if (r == "r7") { robots.Add(DataManager.Instance.GetRobot("RobotTarget7")); }
        else if (r == "r8") { robots.Add(DataManager.Instance.GetRobot("RobotTarget8")); }
        else if (r == "r9") { robots.Add(DataManager.Instance.GetRobot("RobotTarget9")); }
        else if (r == "r10") { robots.Add(DataManager.Instance.GetRobot("RobotTarget10")); }

    }

    //If there is a known list of robots to add, currently not using
    public void AddAllRobots(List<string> robots)
    {
        foreach (string r in robots)
        {
            AddRobot(r);
        }
    }

    public bool AddRobotMode = false;

    //Add robot by clicking on them in AR
    public HashSet<string> touchedRobots = new HashSet<string> { };
    public void addRobotByTouch(string r)
    {
        touchedRobots.Add(r);
        //Debug.Log(touchedRobots.ToString());
    }

    //ADD GRAPH
    private bool _addGraph = false; //Var to tell when to add a graph
    public bool addGraph
    {
        get { return _addGraph; }
        set
        {
            _addGraph = value;
            if (_addGraph == true) { createGraph(); }// Add graph here

        }
    }

    public List<IVisualization> allVizs = new List<IVisualization>(); //temp list of all visualizations
    public List<string> allVizsNames = new List<string>(); //temp list of all visualizations
    public List<MapPolicy> allMPolicies = new List<MapPolicy>();


    //Create a graph, Needed are robots, variables, type
    //Calls VizManager to add the graph, currently adds title as well
    private void createGraph()
    {
        string title = "";
        DateTime foo = DateTime.UtcNow;
        long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
        if (GraphType == graph.Line)
        {
            Robot r1 = robots[0];
            robots.RemoveAt(0);
            string xvar = wantedVars[0];
            string yvar = wantedVars[1];
            IVisualization graphToAdd = new LineGraph(xvar, yvar, r1, robots.ToArray());
            title = xvar + "," + yvar + " Line " + unixTime;//TODO: Add another unique symbol to this?
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
        else if (GraphType == graph.Pie)
        {
            Robot r1 = robots[0];
            robots.RemoveAt(0);
            Robot r2 = robots[0];
            robots.RemoveAt(0);
            string var = wantedVars[0];
            IVisualization graphToAdd = new PieChart(var, r1, r2, robots.ToArray());
            title = var + "," + " Pie " + unixTime;//TODO: Add another unique symbol to this?
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
        else if (GraphType == graph.PieMulti)
        {
            Robot r1 = robots[0];
            robots.RemoveAt(0);
            string var = wantedVars[0];
            wantedVars.RemoveAt(0);
            IVisualization graphToAdd = new PieChartMultiVar(r1, var, wantedVars.ToArray());
            title = var + "," + "Multi Pie " + unixTime;//TODO: Add another unique symbol to this?
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
        else if (GraphType == graph.Bar)
        {

            HashSet<Robot> hashRobots = new HashSet<Robot>(robots);
            HashSet<string> hashVars = new HashSet<string>(wantedVars);
            IVisualization graphToAdd = new BarGraph(hashRobots, hashVars);
            title = "Bar " + unixTime;//TODO: Add another unique symbol to this?
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);

            /*
            Robot r1 = robots[0];
            robots.RemoveAt(0);
            string var = wantedVars[0];
            wantedVars.RemoveAt(0);
            HashSet<Robot> hashRobots = new HashSet<Robot>(robots);
            HashSet<string> hashVars = new HashSet<string>(wantedVars);


            IVisualization graphToAdd = new BarGraph(r1, hashRobots, var, hashVars);
            DateTime foo = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
            title =  "Bar " + unixTime;//TODO: Add another unique symbol to this?
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
            */
        }
        else if (GraphType == graph.TwoDRange)
        {
            Debug.Log("Policies " + allRPolicies + allRPolicies[0] +" " +  allRPolicies.Count +"init2 color" + allRPolicies[1].color );
            title = "TwoDRange " + unixTime;
            Robot r1 = robots[0];
            robots.RemoveAt(0);
            string var = wantedVars[0];
            IVisualization graphToAdd = new RangeIndicator(var, allRPolicies, r1, robots.ToArray());
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
        else if (GraphType == graph.TwoDMap)
        {
            title = "TwoDMAp " + unixTime;

        }

        _addGraph = false;
    }



    #region SINGLETON PATTERN
    public static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
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

//Class for tags, allows us to associate a list of robots to a tag name.
public class Tag
{
    public string name;
    public List<Robot> robots;
    public Tag(string name, List<Robot> robots)
    {
        this.name = name;
        this.robots = robots;
    }
}
