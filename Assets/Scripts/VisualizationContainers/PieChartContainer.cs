using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

// TODO: add another for PieChartMultiVar
public class PieChartContainer : VisualizationContainer<PieChart>
{
    // Instances of VisualizationContainer have access to the container
    // RectTransform container: the RectTransform of the drawable area in the
    // canvas. NOT the same as canvas.GetComponent<RectTransform>()
    List<Robot> robots = new List<Robot>();
    Dictionary<Robot, float> dataDict = new Dictionary<Robot, float>();

    private Image wedgePrefab;
    private Dictionary<Robot, GameObject> wedges;

    private float total; // sum of all data in pie chart
    private float zRotation = 0f;

    // Initialize things
    protected override void Start() 
    {
        // TODO: maybe remove
        base.Start(); 
        
        wedges = new Dictionary<Robot, GameObject>();
    }

    private GameObject GetWedge(Robot robot) {
        if (!wedges.ContainsKey(robot)) {
            GameObject blankWedge = (GameObject)Instantiate(Resources.Load("Wedge"), transform); 
            blankWedge.transform.SetParent(container, false); 
            wedges[robot] = blankWedge;
        }

        return wedges[robot];
    } 

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() 
    {
        zRotation = 0f;
        foreach (Robot r in robots) {
            GameObject wedge = GetWedge(r);
            wedge.transform.SetParent(container.transform, false);
            wedge.GetComponent<Image>().color = r.color;
            wedge.GetComponent<Image>().fillAmount = dataDict[r]/total;
            wedge.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));

            zRotation -= wedge.GetComponent<Image>().fillAmount * 360f;
        }
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
    protected override void UpdateData(Dictionary<Robot, List<float>> data)
    {
        float newTotal = 0;
        foreach (Robot r in data.Keys) {
            if (!robots.Contains(r)) {
                robots.Add(r);
            }

            dataDict[r] = data[r][0]; 
        }

        // update the total 
        foreach (Robot r in dataDict.Keys) {
            newTotal += dataDict[r]; 
        }

        total = newTotal;
    }
}