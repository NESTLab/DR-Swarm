using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class MapIndicatorContainer : VisualizationContainer<MapIndicator>
{
    // Instances of VisualizationContainer have access to the container
    // RectTransform container: the RectTransform of the drawable area in the
    // canvas. NOT the same as canvas.GetComponent<RectTransform>()

    List<Robot> robots = new List<Robot>();
    List<string> variables = new List<string>();
    Dictionary<string, float> dataDict = new Dictionary<string, float>();

    private List<MapPolicy> policies;

    private Dictionary<Robot, GameObject> indicators;
    /*
    private Sprite square;
    private Sprite circle;
    private Sprite triangle;
    private Sprite check;
    private Sprite exclamation;
    private Sprite plus;
    */

    //private Dictionary<MapPolicy.IndicatorShape, Sprite> sprites; // shouldn't the map indicator have a shape?

    // Initialize things
    protected override void Start()
    {
        // TODO: maybe remove
        base.Start();

        MapIndicator vis = (MapIndicator)visualization;
        policies = vis.GetPolicies();

        indicators = new Dictionary<Robot, GameObject>();
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw()
    {
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
    protected override void UpdateData(Dictionary<Robot, Dictionary<string, float>> data)
    {
        if (data.ContainsKey(this.robot)) {
            foreach (string var in data[robot].Keys) {
                dataDict[var] = data[robot][var];

                if (!variables.Contains(var)) {
                    variables.Add(var);
                }
            }
        }
    }
}