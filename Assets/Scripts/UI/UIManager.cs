using graphNameSpace;
using shapeNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Name space for types of graphs
/// </summary>
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

/// <summary>
/// UI Manager
/// Singleton class that managages the data and behavior for the UI
/// </summary>
public class UIManager : MonoBehaviour
{

    // Start is called before the first frame update   
    void Start()
    { }

    // Update is called once per frame
    void Update()
    { }

    public int Options = 1;//How many min variable options
    public int EOptions = 0; ///
    public int TotalOptions = 1;
    public int RobotOptions = 0;

    public string _sentGraphType = "";
    public bool EditVizBool = false;
    public IVisualization editviz;
    public string editVizName;
    public graph editVizGraphType;
    public List<string> editVars = new List<string>();
    public List<RangePolicy> editRangePolicys = new List<RangePolicy>();
    public List<MapPolicy> editMapPolicys = new List<MapPolicy>();
    public bool updateVizPanel = false;
    public IndicatorShape editDShape;
    public Color editDColor;


    public graph GraphType = graph.NoGraph; //What type of graph is currently being set

    /// <summary>
    /// Variable that will detect when changed
    /// </summary>
    public string sentGraphType
    {
        get { return _sentGraphType; }
        set
        {
            _sentGraphType = value;
            //Set options based on type of graph
            if (_sentGraphType == "Line")
            {
                GraphType = graph.Line; //Sets the graph type
                Options = 2; //Min number of options
                TotalOptions = 100; //Total Options <<Not used atm
                RobotOptions = 0; // <<not used atm
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


    #region TAGS
    public List<Tag> allTags = new List<Tag>();
    //Create a new tag

    /// <summary>
    /// creates a tag that is stored in the allTags Function
    /// </summary>
    /// <param name="name"> Name of the tag</param>
    /// <param name="robots">List of the robots included in the tag</param>
    public void CreateTag(string name, List<Robot> robots)
    {
        Tag newtag = new Tag(name, robots);
        allTags.Add(newtag);
        Debug.Log("TAGS" + allTags.Count);
    }

    public List<string> checkedTagNames = new List<string>();

    /// <summary>
    /// Add tag robots to the checked robots list
    /// </summary>
    public void addCheckedTags()
    {
        //Match string to tag name
        //add union of robots
        HashSet<Robot> thebots = new HashSet<Robot>(robots);
        foreach (string s in checkedTagNames)
        {
            Tag c = new Tag("", new List<Robot>());
            for (int i = 0; i < allTags.Count; i++)
            {
                if (allTags[i].name == s) { c = allTags[i]; break; }
            }
            if (c.robots.Count > 0)
            {
                thebots.UnionWith(c.robots);
            }
        }
        robots = new List<Robot>(thebots);
    }
    #endregion

    #region Variables
    public List<string> wantedVars = new List<string>();
    //Get vars from selected robots 
    /// <summary>
    /// Get the variables from the currently selected robots. Uses an intersection of all the variables
    /// </summary>
    public void updateTotalVars()
    {
        HashSet<string> varFromRobots = new HashSet<string>();
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
    #endregion

    #region Robots
    //ADD ROBOTS
    public List<Robot> robots = new List<Robot> { };//Robots for the current graph
    //Add a robot to the array to get the total robots for the graph

    /// <summary>
    /// Add a robot to the array to get the total robots for the graph
    /// </summary>
    /// <param name="r">robot name string ("r" + int)</param>
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
    /*
        //If there is a known list of robots to add, currently not using
        /// <summary>
        /// If there is a known list of robots to add, currently not using
        /// </summary>
        /// <param name="robots"></param>
        public void AddAllRobots(List<string> robots)
        {
            foreach (string r in robots)
            {
                AddRobot(r);
            }
        }
        */

    public bool AddRobotMode = false; //Used for the touch robots, know when clicking on a robot will add it

    //Add robot by clicking on them in AR
    public HashSet<string> touchedRobots = new HashSet<string> { };
    public List<string> editVizRobots = new List<string>();

    /// <summary>
    /// Add robot by clicking on them in AR, Triggered by touchscript
    /// </summary>
    /// <param name="r">robot name</param>
    public void addRobotByTouch(string r)
    {
        touchedRobots.Add(r);
    }
    #endregion

    #region Graphs
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
    public List<MapPolicy> allMPolicies = new List<MapPolicy>(); //List of all Map policies during add/edit
    public List<RangePolicy> allRPolicies = new List<RangePolicy>(); //List of all range policies during add/edit
    public Color sentColor = new Color(); //default color sent through range or map
    public IndicatorShape sentShape = new IndicatorShape(); //default shape sent through range or map

    /// <summary>
    /// Create a graph, Needed are robots, variables, type
    /// Calls VizManager to add the graph, currently adds title as well
    /// </summary>
    private void createGraph()
    {
        string title = ""; //title of visualization, uses graph type+ dateTime
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
            title = var + "," + " Pie " + unixTime;
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
        else if (GraphType == graph.PieMulti)
        {
            IVisualization graphToAdd = new PieChartMultiVar(robots, wantedVars);
            title = "Multi Pie " + unixTime;
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
        else if (GraphType == graph.Bar)
        {

            HashSet<Robot> hashRobots = new HashSet<Robot>(robots);
            HashSet<string> hashVars = new HashSet<string>(wantedVars);
            IVisualization graphToAdd = new BarGraph(hashRobots, hashVars);
            title = "Bar " + unixTime;
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);

        }
        else if (GraphType == graph.TwoDRange)
        {
            title = "TwoDRange " + unixTime;
            Robot r1 = robots[0];
            robots.RemoveAt(0);
            string var = wantedVars[0];
            IVisualization graphToAdd = new RangeIndicator(var, allRPolicies, sentColor, sentShape, r1, robots.ToArray());
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
        else if (GraphType == graph.TwoDMap)
        {
            title = "TwoDMAp " + unixTime;
            Robot r1 = robots[0];
            robots.RemoveAt(0);
            string var = wantedVars[0];
            IVisualization graphToAdd = new MapIndicator(allMPolicies, sentColor, sentShape, r1, robots.ToArray());
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }

        _addGraph = false;
        robots = new List<Robot>();
        updateVizPanel = true;

        //reset edit
        if (EditVizBool)
        {
            VisualizationManager.Instance.RemoveVisualization(editVizName);
            allVizs.Remove(editviz);
            allVizsNames.Remove(editVizName);
            Debug.Log("Viz count to " + allVizs.Count);
            EditVizBool = false;
            editVizName = "";
            editVizGraphType = graph.NoGraph;
            editVars = new List<string>();
            editRangePolicys = new List<RangePolicy>();
            editMapPolicys = new List<MapPolicy>();
            
            EOptions = 0;
            editDShape = new IndicatorShape();
            editDColor = new Color();
            editVizRobots = new List<string>();
        }
    }
    #endregion



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
    { }


}

/// <summary>
/// Class for tags, allows us to associate a list of robots to a tag name.
/// </summary>
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
