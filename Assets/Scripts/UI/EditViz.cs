using graphNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for setup of editting a viz, the first step is getting what the viz type would be.
/// </summary>
public class EditViz : MonoBehaviour
{
    //Gameobjects
    IVisualization edit;
    public Text currVizTitle;
    string title;
    public Button back;
    public Button close;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        edit = null;
        back.onClick.AddListener(Reset);
        close.onClick.AddListener(Reset);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        if (UIManager.Instance.EditVizBool && edit == null)
        {
            edit = UIManager.Instance.editviz;
            changeVizName();
        }

    }

    /// <summary>
    /// Get the current type of the viz and display it.
    /// </summary>
    void changeVizName()
    {
        Text t = currVizTitle.GetComponent<Text>(); //text object
        if (edit.GetType().ToString() == "LineGraph")
        {
            t.text = "Current Viz Type: Line Graph";
            UIManager.Instance.editVizGraphType = graph.Line;
        }
        else if (edit.GetType().ToString() == "BarGraph")
        {
            currVizTitle.GetComponent<Text>().text = "Current Viz Type: Bar Graph";
            UIManager.Instance.editVizGraphType = graph.Bar;
        }
        else if (edit.GetType().ToString() == "MapIndicator")
        {
            currVizTitle.GetComponent<Text>().text = "Current Viz Type: Map Indicator";
            UIManager.Instance.editVizGraphType = graph.TwoDMap;
            UIManager.Instance.editMapPolicys = ((MapIndicator)edit).GetPolicies();
            UIManager.Instance.editDShape = ((MapIndicator)edit).GetDefaultShape();
            UIManager.Instance.editDColor = ((MapIndicator)edit).GetDefaultColor();
        }
        else if (edit.GetType().ToString() == "PieChart")
        {
            currVizTitle.GetComponent<Text>().text = "Current Viz Type: Pie Graph";
            UIManager.Instance.editVizGraphType = graph.Pie;
        }
        else if (edit.GetType().ToString() == "PieChartMultiVar")
        {
            currVizTitle.GetComponent<Text>().text = "Current Viz Type: Pie Graph Multi Var";
            UIManager.Instance.editVizGraphType = graph.PieMulti;
        }
        else if (edit.GetType().ToString() == "RangeIndicator")
        {
            currVizTitle.GetComponent<Text>().text = "Current Viz Type: Range Indicator";
            UIManager.Instance.editVizGraphType = graph.TwoDRange;
            UIManager.Instance.editRangePolicys = ((RangeIndicator)edit).GetPolicies();
            UIManager.Instance.editDShape = ((RangeIndicator)edit).GetDefaultShape();
            UIManager.Instance.editDColor = ((RangeIndicator)edit).GetDefaultColor();

        }
        //Get the robots from the current viz
        List<string> botNames = new List<string>();
        foreach (Robot r in edit.GetRobots())
        {
            botNames.Add(r.name);
        }
        UIManager.Instance.editVizRobots = botNames;
        UIManager.Instance.editVars = new List<string>(edit.GetVariables());
        UIManager.Instance.EOptions = edit.GetVariables().Count;
    }

    /// <summary>
    /// Reset the edit object.
    /// </summary>
    private void Reset()
    {
        edit = null;
    }
}
