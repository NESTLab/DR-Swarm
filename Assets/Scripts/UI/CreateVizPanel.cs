using graphNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateVizPanel : MonoBehaviour
{
    public GameObject vizPanel; //Parent Panel, set when adding script
    float initpos = -37f; //Position offset for the prefabs

    public List<IVisualization> allVizs = new List<IVisualization>();//All Visualizations, IViz
    public List<string> allVizsNames = new List<string>(); //Visualization names
    public int totalViz = 0; // Total prefabs the script needs to add
    float offset = 0f; // Offset for when a prefab gets added
    public GameObject editVizPanel;
    public GameObject menuPanel;
    public Sprite bar;
    public Sprite line;
    public Sprite pie;
    public Sprite pieMult;
    public Sprite map;
    public Sprite range;


    // Start is called before the first frame update
    void Start()
    {
        /*
        allVizs = UIManager.Instance.allVizs;
        allVizsNames = UIManager.Instance.allVizsNames;
        if (vizPanel != null)
        {
            if (allVizs.Count > 0)
            {
                foreach (IVisualization viz in allVizs)
                {
                    GameObject vizPrefab = (GameObject)Instantiate(Resources.Load("SingleVizUIPrefab"), transform);
                    vizPrefab.transform.SetParent(vizPanel.transform, false);
                    RectTransform t = vizPrefab.GetComponent<RectTransform>();
                    t.sizeDelta = new Vector2(0, 75f);
                    t.anchorMax = new Vector2(1f, 1f);
                    t.anchorMin = new Vector2(0f, 1f);
                    t.anchoredPosition = new Vector2(1f, initpos);
                    t.pivot = new Vector2(.5f, .5f);
                    totalViz++;
                    offset = offset + -70f;

                }
            }
        }
        */
    }

    // Update is called once per frame
    public int i = 0;
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
                        Debug.Log("Graph type" + viztype);
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
        else
        {
            UIManager.Instance.updateVizPanel = true;
        }
    }

    //To remove a visualization you only need to pass the name to the VizManager
    //The IViz here is only for sending the viz to UIManagager to keep track of how many vizs there currently are
    //^NOT BEST WAY TO DO THIS(atm)
    void removeViz(string title, IVisualization vz)
    {
        allVizs.Remove(vz);
        allVizsNames.Remove(title);
        VisualizationManager.Instance.RemoveVisualization(title);
        UIManager.Instance.allVizs = allVizs;
        UIManager.Instance.allVizsNames = allVizsNames;
        Debug.Log("Viz count to " + UIManager.Instance.allVizs.Count);
    }

    void editViz(string title, IVisualization viz)
    {
        UIManager.Instance.editVizName = title;
        UIManager.Instance.editviz = viz;
        UIManager.Instance.EditVizBool = true;
        editVizPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    void setImage(Image a, graph g)
    {
        if (g == graph.Bar) { a.sprite = bar; }
        else if (g == graph.Line) { a.sprite = line; }
        else if (g == graph.Pie) { a.sprite = pie; }
        else if (g == graph.PieMulti) { a.sprite = pieMult; }
        else if (g == graph.TwoDMap) { a.sprite = map; }
        else if (g == graph.TwoDRange) { a.sprite = range; }
    }

    void toggleVizDisplay(string vizName, IVisualization viz)
    {
        Debug.Log("Logging Name:" + vizName);
        VisualizationManager.Instance.toggleVisualizationFromManager(vizName);
    }

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
