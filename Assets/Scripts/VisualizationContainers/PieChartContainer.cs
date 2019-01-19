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
    //private List<float> data;
    public List<Color> wedgeColors; //randomize this for now

    private Image wedgePrefab;
    private List<Image> wedges;

    private float total; //sum of all data in pie chart
    private float zRotation = 0f;

    //NEW STUFF
    Dictionary<Robot, float> dataDict = new Dictionary<Robot, float>();

    // Initialize things
    protected override void Start()
    {
        //wedges = new Image[2];

        //initialize the wedges
        for (int i = 0; i < data.Length; i++) {
            Debug.Log("making new wedge");
            Image newWedge = Instantiate(wedgePrefab) as Image;
            wedges.Add(newWedge);
        }

        string robotName = transform.parent.parent.gameObject.name; //name of the image target
        robot = DataManager.Instance.GetRobot(robotName);
        robot.SetVariable("percent", 0f);
        robot.GetObservableVariable<float>("percent").Subscribe(percent => { //assume float for now
            data[0] = percent;
            data[1] = total - data[0];
            MakeGraph();
        });
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw()
    {
        zRotation = 0f;
        for (int i = 0; i < data.Length; i++) {
            Debug.Log("updating wedge");
            Image wedge = wedges[i];
            wedge.transform.SetParent(transform, false);
            wedge.color = wedgeColors[i];
            wedge.fillAmount = data[i] / total;
            wedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= wedge.fillAmount * 360f;
            wedges[i] = wedge; //this probably isn't necessary, but might as well
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