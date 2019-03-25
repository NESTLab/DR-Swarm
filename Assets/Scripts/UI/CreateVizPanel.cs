using graphNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that creates the list of viz on the main panel.
/// </summary>
public class CreateVizPanel : MonoBehaviour
{
    float initpos = -37f; //Position offset for the prefabs
    public List<IVisualization> allVizs = new List<IVisualization>();//All Visualizations, IViz
    public List<string> allVizsNames = new List<string>(); //Visualization names
    public int totalViz = 0; // Total prefabs the script needs to add
    float offset = 0f; // Offset for when a prefab gets added
    public int i = 0;//number of vizs
    //game objects
    public GameObject vizPanel; //Parent Panel, set when adding script
    public GameObject editVizPanel;
    public GameObject menuPanel;
    //sprites for the images
    public Sprite bar;
    public Sprite line;
    public Sprite pie;
    public Sprite pieMult;
    public Sprite map;
    public Sprite range;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    { }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        allVizs = UIManager.Instance.allVizs; //Get the vizs
        allVizsNames = UIManager.Instance.allVizsNames;//Get all the names
        if (vizPanel != null) //Checking if the panel is active (not null)
        {
            if (allVizs.Count != totalViz || UIManager.Instance.updateVizPanel) // Only Update if we need to add/remove prefabs from the panel
            {
                i = 0;
                foreach (Transform child in vizPanel.transform) //Get everything out of the panel
                {
                    Destroy(child.gameObject);
                }
                offset = 0; //Reset the Offset
                if (allVizs.Count > 0)
                {
                    foreach (IVisualization viz in allVizs)
                    {
                        GameObject vizPrefab = (GameObject)Instantiate(Resources.Load("UI/SingleVizUIPrefab"), transform); //Initialize the prefab
                        vizPrefab.transform.SetParent(vizPanel.transform); //All the prefabs must have the same parent
                        RectTransform t = vizPrefab.GetComponent<RectTransform>(); //Set the position
                        t.sizeDelta = new Vector2(0, 75f);
                        t.anchorMax = new Vector2(1f, 1f);
                        t.anchorMin = new Vector2(0f, 1f);
                        t.anchoredPosition = new Vector2(1f, initpos + offset);
                        t.pivot = new Vector2(.5f, .5f);
                        //Set the componenets of the prefab
                        Text t2 = vizPrefab.transform.Find("Name").GetComponent<Text>();
                        t2.text = allVizsNames[i];
                        Button remv = vizPrefab.transform.Find("rmvViz").GetComponent<Button>();
                        string name = allVizsNames[i];
                        remv.onClick.AddListener(delegate { removeViz(name, viz); });
                        Button edit = vizPrefab.transform.Find("editViz").GetComponent<Button>();
                        edit.onClick.AddListener(delegate { editViz(name, viz); });
                        Image img = vizPrefab.transform.Find("vizImage").GetComponent<Image>();
                        graph viztype = getVizType(allVizs[i]);
                        setImage(img, viztype);
                        Toggle visability = vizPrefab.transform.Find("Visability").GetComponent<Toggle>();
                        visability.onValueChanged.AddListener(delegate { toggleVizDisplay(name, viz); });

                        //Maitance variables
                        totalViz++;
                        i++;
                        offset = offset + -70f;
                    }
                    totalViz = allVizs.Count;
                }
                UIManager.Instance.updateVizPanel = false;
            }
        }
        else { UIManager.Instance.updateVizPanel = true; }
    }

    /// <summary>
    /// To remove a visualization you only need to pass the name to the VizManager.
    /// The IViz here is only for sending the viz to UIManagager to keep track of how many vizs there currently are.
    /// </summary>
    /// <param name="title">title of the viz</param>
    /// <param name="vz">actual vizualization</param>
    void removeViz(string title, IVisualization vz)
    {
        allVizs.Remove(vz);
        allVizsNames.Remove(title);
        VisualizationManager.Instance.RemoveVisualization(title);
        UIManager.Instance.allVizs = allVizs;
        UIManager.Instance.allVizsNames = allVizsNames;
    }

    /// <summary>
    /// Attached to edit buttons of each prefab, set it to the edit panels.
    /// </summary>
    /// <param name="title"> Name of viz</param>
    /// <param name="viz">The viz</param>
    void editViz(string title, IVisualization viz)
    {
        UIManager.Instance.editVizName = title;
        UIManager.Instance.editviz = viz;
        UIManager.Instance.EditVizBool = true;
        editVizPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    /// <summary>
    /// Set the image of the viz prefab.
    /// </summary>
    /// <param name="a">Image object</param>
    /// <param name="g">Graph type</param>
    void setImage(Image a, graph g)
    {
        if (g == graph.Bar) { a.sprite = bar; }
        else if (g == graph.Line) { a.sprite = line; }
        else if (g == graph.Pie) { a.sprite = pie; }
        else if (g == graph.PieMulti) { a.sprite = pieMult; }
        else if (g == graph.TwoDMap) { a.sprite = map; }
        else if (g == graph.TwoDRange) { a.sprite = range; }
    }

    /// <summary>
    /// Turn off the visualization through VizManager.
    /// </summary>
    /// <param name="vizName">name of the viz</param>
    /// <param name="viz">viz</param>
    void toggleVizDisplay(string vizName, IVisualization viz)
    {
        VisualizationManager.Instance.toggleVisualizationFromManager(vizName);
    }

    /// <summary>
    /// Get the graph type of the viz.
    /// </summary>
    /// <param name="viz">viz type</param>
    /// <returns></returns>
    graph getVizType(IVisualization viz)
    {
        if (viz.GetType().ToString() == "LineGraph") { return graph.Line; }
        else if (viz.GetType().ToString() == "BarGraph") { return graph.Bar; }
        else if (viz.GetType().ToString() == "MapIndicator") { return graph.TwoDMap; }
        else if (viz.GetType().ToString() == "PieChart") { return graph.Pie; }
        else if (viz.GetType().ToString() == "PieChartMultiVar") { return graph.PieMulti; }
        else if (viz.GetType().ToString() == "RangeIndicator") { return graph.TwoDRange; }
        else { return graph.Line; }
    }
}
