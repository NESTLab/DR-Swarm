using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using shapeNamespace;

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
    
    private Sprite square;
    private Sprite circle;
    private Sprite triangle;
    private Sprite check;
    private Sprite exclamation;
    private Sprite plus; 

    private Dictionary<IndicatorShape, Sprite> sprites; // shouldn't the map indicator have a shape?
    private MapIndicator vis;
    private Dictionary<MapPolicy, string> policyDict;

    // Initialize things
    protected override void Start()
    {
        // TODO: maybe remove
        base.Start();

        vis = (MapIndicator)visualization;
        policies = vis.GetPolicies();
        policyDict = vis.GetPolicyDict();

        indicators = new Dictionary<Robot, GameObject>();

        square = Resources.Load<Sprite>("Sprites/square");
        circle = Resources.Load<Sprite>("Sprites/circle");
        triangle = Resources.Load<Sprite>("Sprites/triangle");
        check = Resources.Load<Sprite>("Sprites/check");
        exclamation = Resources.Load<Sprite>("Sprites/exclamation");
        plus = Resources.Load<Sprite>("Sprites/plus");

        sprites = new Dictionary<IndicatorShape, Sprite>();
        sprites[IndicatorShape.Check] = check;
        sprites[IndicatorShape.Circle] = circle;
        sprites[IndicatorShape.Exclamation] = exclamation;
        sprites[IndicatorShape.Plus] = plus;
        sprites[IndicatorShape.Square] = square;
        sprites[IndicatorShape.Triangle] = triangle;
    }

    private Color SetColor(float v) {
        v = (float)(v % 1.0);
        Color c = Color.HSVToRGB(v, 1f, 1f);
        return c;
    }
    
    private GameObject CreateIndicator() {
        GameObject indicator = new GameObject("indicator", typeof(Image));
        indicator.GetComponent<Image>().sprite = sprites[vis.GetDefaultShape()];
        indicator.GetComponent<Image>().color = vis.GetDefaultColor();
        indicator.GetComponent<Image>().type = Image.Type.Filled;  // this is so we can change the amount it's filled
                                                                   // We're gonna have to change how it's filled, but that's a later problem

        string var;
        float val;

        foreach (MapPolicy p in policies) {
            if (p.type == MapPolicy.MapPolicyType.color) {
                // TODO: set the color to the associated value
                var = policyDict[p];
                val = dataDict[var];
                indicator.GetComponent<Image>().color = SetColor(val);
            }
            else if (p.type == MapPolicy.MapPolicyType.fillAmount) {
                // TODO: set the fill amount to the associated value
            }
            else if (p.type == MapPolicy.MapPolicyType.orientation) {
                // TODO: set the orientation to the associated value
            }
        }

        return indicator;
    }
    
    private GameObject GetIndicator(Robot robot) { // I think this is wrong
        if (!indicators.ContainsKey(robot)) {
            GameObject indicator = CreateIndicator();
            indicator.transform.SetParent(transform, false);
            indicators[robot] = indicator;
        }

        return indicators[robot];
    }
    

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw()
    {
        GameObject indicator = GetIndicator(this.robot);
        indicator.GetComponent<Image>().sprite = sprites[vis.GetDefaultShape()];
        indicator.GetComponent<Image>().color = vis.GetDefaultColor();
        // probably gonna need more of these for fill default and orientation default
        string var;
        float val;

        foreach (MapPolicy p in policies) {
            if (p.type == MapPolicy.MapPolicyType.color) {
                // TODO: set the color to the associated value
                var = policyDict[p];
                val = dataDict[var];
                indicator.GetComponent<Image>().color = SetColor(val);
            }
            else if (p.type == MapPolicy.MapPolicyType.fillAmount) {
                // TODO: set the fill amount to the associated value
            }
            else if (p.type == MapPolicy.MapPolicyType.orientation) {
                // TODO: set the orientation to the associated value
            }
        }
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