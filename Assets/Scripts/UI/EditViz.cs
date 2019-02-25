using graphNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditViz : MonoBehaviour
{
    IVisualization edit;
    // Start is called before the first frame update
    public Text currVizTitle;
    string title;
    public Button back;
    public Button close;
    void Start()
    {
        edit = null;
        back.onClick.AddListener(Reset);
        close.onClick.AddListener(Reset);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.EditVizBool && edit == null)
        {
            edit = UIManager.Instance.editviz;
            changeVizName();
        }

    }

    void changeVizName()
    {
        Text t = currVizTitle.GetComponent<Text>();
        Debug.Log("Changing name " + edit.GetType().ToString());
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

        List<string> botNames = new List<string>();
        foreach (Robot r in edit.GetRobots())
        {
            botNames.Add(r.name);
        }
        Debug.Log("Vars" + edit.GetVariables().Count);
        UIManager.Instance.editVizRobots = botNames;
        UIManager.Instance.editVars = new List<string>(edit.GetVariables());
        UIManager.Instance.EOptions = edit.GetVariables().Count;
    }

    private void Reset()
    {
        edit = null;
    }





}
