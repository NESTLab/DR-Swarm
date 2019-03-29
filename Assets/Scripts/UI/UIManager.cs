using System;
using System.Collections.Generic;
using UnityEngine;
using DrSwarm;
using DrSwarm.Model;
using DrSwarm.Model.Visualizations;

public class UIManager : MonoBehaviour
{
    public enum Graph
    {
        Line,
        Pie,
        PieMulti,
        Bar,
        TwoDRange,
        TwoDMap,
        NoGraph
    }

    // Start is called before the first frame update   

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int Options = 1;//How many min variable options
    public int EOptions = 0;
    public int TotalOptions = 1;
    public int RobotOptions = 0;

    public string _sentGraphType = "";
    public bool EditVizBool = false;
    public IVisualization editviz;
    public string editVizName;
    public Graph editVizGraphType;
    public List<string> editVars = new List<string>();
    public List<RangePolicy> editRangePolicys = new List<RangePolicy>();
    public List<MapPolicy> editMapPolicys = new List<MapPolicy>();
    public bool updateVizPanel = false;
    public IndicatorShape editDShape;
    public Color editDColor;


    public Graph GraphType = Graph.NoGraph; //What type of graph is currently being set
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
                GraphType = Graph.Line;
                Options = 2;
                TotalOptions = 100;
                RobotOptions = 0;
            }
            else if (_sentGraphType == "Pie")
            {
                GraphType = Graph.Pie;
                Options = 1;
                TotalOptions = 1;
                RobotOptions = 0;
            }
            else if (_sentGraphType == "PieMulti")
            {
                GraphType = Graph.PieMulti;
                Options = 2;
                RobotOptions = 1;
                TotalOptions = 100;
            }
            else if (_sentGraphType == "Bar")
            {
                GraphType = Graph.Bar;
                Options = 1;
                RobotOptions = 0;
                TotalOptions = 100;
            }
            else if (_sentGraphType == "2DMap")
            {
                GraphType = Graph.TwoDMap;
                Options = 0;
                RobotOptions = 0;
                TotalOptions = 100;
            }
            else if (_sentGraphType == "2DRange")
            {
                GraphType = Graph.TwoDRange;
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
        Debug.Log("TAGS" + allTags.Count);
    }
    public List<string> checkedTagNames = new List<string>();
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
        //Debug.Log("Amount of checed" + checkedTagNames.Count);
        //Debug.Log("THERE ARE + " + thebots.Count);
        robots = new List<Robot>(thebots);
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
    public List<string> editVizRobots = new List<string>();
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
    public List<RangePolicy> allRPolicies = new List<RangePolicy>();
    public Color sentColor = new Color();
    public IndicatorShape sentShape = new IndicatorShape();

    //Create a graph, Needed are robots, variables, type
    //Calls VizManager to add the graph, currently adds title as well
    private void createGraph()
    {
        string title = "";
        DateTime foo = DateTime.UtcNow;
        long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
        if (GraphType == Graph.Line)
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
            Debug.Log("Type trufalse " + ("LineGraph" == graphToAdd.GetType().ToString()));
        }
        else if (GraphType == Graph.Pie)
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
        else if (GraphType == Graph.PieMulti)
        {
            //Robot r1 = robots[0];
            //robots.RemoveAt(0);
            //string var = wantedVars[0];
            //wantedVars.RemoveAt(0);
            IVisualization graphToAdd = new PieChartMultiVar(robots, wantedVars);
            title = "Multi Pie " + unixTime;//TODO: Add another unique symbol to this?
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
        else if (GraphType == Graph.Bar)
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
        else if (GraphType == Graph.TwoDRange)
        {
            //Debug.Log("Policies " + allRPolicies + allRPolicies[0] + " " + allRPolicies.Count + "init2 color" + allRPolicies[1].color);
            title = "TwoDRange " + unixTime;
            Robot r1 = robots[0];
            robots.RemoveAt(0);
            string var = wantedVars[0];
            IVisualization graphToAdd = new RangeIndicator(var, allRPolicies, sentColor, sentShape, r1, robots.ToArray());
            VisualizationManager.Instance.AddVisualization(title, graphToAdd);
            allVizs.Add(graphToAdd);
            allVizsNames.Add(title);
        }
        else if (GraphType == Graph.TwoDMap)
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

        //reset edit
        if (EditVizBool)
        {
            VisualizationManager.Instance.RemoveVisualization(editVizName);
            allVizs.Remove(editviz);
            allVizsNames.Remove(editVizName);
            Debug.Log("Viz count to " + allVizs.Count);
            EditVizBool = false;
            editVizName = "";
            editVizGraphType = Graph.NoGraph;
            editVars = new List<string>();
            editRangePolicys = new List<RangePolicy>();
            editMapPolicys = new List<MapPolicy>();
            updateVizPanel = true;
            EOptions = 0;
            editDShape = new IndicatorShape();
            editDColor = new Color();
            editVizRobots = new List<string>();
            updateVizPanel = true;
        }
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
