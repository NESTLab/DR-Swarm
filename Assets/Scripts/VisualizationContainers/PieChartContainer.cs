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

    //private List<float> data;
    //public List<Color> wedgeColors; //randomize this for now
    public Dictionary<Robot, Color> wedgeColors;

    private Image wedgePrefab;
    //private List<Image> wedges;
    private Dictionary<Robot, Image> wedges;

    private float total; // sum of all data in pie chart
    private float zRotation = 0f;

    // Initialize things
    protected override void Start() 
    {
        base.Start(); 

        // make a bunch of dictionaries?
        wedgeColors = new Dictionary<Robot, Color>();
        //wedges = new List<Image>();
        wedges = new Dictionary<Robot, Image>();

        foreach (Robot r in robots) {
            // I think having it this way prevents adding any new robots, but dunno how else to do it
            Image blankWedge = Instantiate(wedgePrefab) as Image; 
            wedges[r] = blankWedge;
            Color randColor = new Color(Random.Range(0f, 1f),
                                  Random.Range(0f, 1f),
                                  Random.Range(0f, 1f));
            wedgeColors[r] = randColor;
        }
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() 
    {
        zRotation = 0f;
        foreach (Robot r in robots) {
            wedges[r].transform.SetParent(transform, false);
            wedges[r].color = wedgeColors[r];
            wedges[r].fillAmount = dataDict[r]/total;
            wedges[r].transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= wedges[r].fillAmount * 360f;
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

            //probably want to update the total at this point too
            newTotal += data[r][0];
        }

        total = newTotal;
    }
}