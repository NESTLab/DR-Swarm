using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using shapeNamespace;

/// <summary>
/// Class responsible for drawing the map indicator visualization.
/// </summary>
/// <remarks>
/// Instances of VisualizationContainer have access to the container RectTransform container: 
/// the RectTransform of the drawable area in the canvas. 
/// This is NOT the same as canvas.GetComponent<RectTransform>()
/// </remarks>
public class MapIndicatorContainer : VisualizationContainer<MapIndicator> {
    List<Robot> robots = new List<Robot>();
    List<string> variables = new List<string>();
    Dictionary<string, float> dataDict = new Dictionary<string, float>();

    private List<MapPolicy> policies;
    private Dictionary<Robot, GameObject> indicators;
    private Dictionary<IndicatorShape, Sprite> sprites;
    private MapIndicator vis;

    private Sprite square;
    private Sprite circle;
    private Sprite triangle;
    private Sprite check;
    private Sprite exclamation;
    private Sprite plus;
    private Sprite arrow;

    /// <summary>
    /// Initializes the visualization container.
    /// </summary>
    protected override void Start() {
        // TODO: maybe remove
        base.Start();

        vis = (MapIndicator)visualization;
        policies = vis.GetPolicies();
        indicators = new Dictionary<Robot, GameObject>();

        square = Resources.Load<Sprite>("Sprites/square");
        circle = Resources.Load<Sprite>("Sprites/circle");
        triangle = Resources.Load<Sprite>("Sprites/triangle");
        check = Resources.Load<Sprite>("Sprites/check");
        exclamation = Resources.Load<Sprite>("Sprites/exclamation");
        plus = Resources.Load<Sprite>("Sprites/plus");
        arrow = Resources.Load<Sprite>("Sprites/arrow");

        sprites = new Dictionary<IndicatorShape, Sprite>();
        sprites[IndicatorShape.Check] = check;
        sprites[IndicatorShape.Circle] = circle;
        sprites[IndicatorShape.Exclamation] = exclamation;
        sprites[IndicatorShape.Plus] = plus;
        sprites[IndicatorShape.Square] = square;
        sprites[IndicatorShape.Triangle] = triangle;
        sprites[IndicatorShape.Arrow] = arrow;
    }

    /// <summary>
    /// Maps the color of the indicator to the value of the associated variable.
    /// </summary>
    /// <param name="v"> The value of the variable associated with the color field of the indicator. </param>
    /// <returns>
    /// Returns a color. 
    /// </returns>
    private Color SetColor(float v) {
        v = (float)(v % 1.0);
        Color c = Color.HSVToRGB(v, 1f, 1f);
        return c;
    }
    
    /// <summary>
    /// Creates a new map indicator.
    /// </summary>
    /// <returns>
    /// Returns a new map indicator.
    /// </returns>
    private GameObject CreateIndicator() {
        GameObject indicator = new GameObject("indicator", typeof(Image));
        indicator.GetComponent<Image>().sprite = sprites[vis.GetDefaultShape()];
        indicator.GetComponent<Image>().color = vis.GetDefaultColor();
        // This is so we can change the amount the indicator is filled.
        // TODO: Figure out how to change how it's filled
        indicator.GetComponent<Image>().type = Image.Type.Filled;

        string var;
        float val;

        foreach (MapPolicy p in policies) {
            if (p.type == MapPolicy.MapPolicyType.color) {
                var = p.variableName;
                val = dataDict[var];
                indicator.GetComponent<Image>().color = SetColor(val);
            }
            else if (p.type == MapPolicy.MapPolicyType.fillAmount) {
                // TODO: maybe give people more of a choice?
                if (vis.GetDefaultShape() == IndicatorShape.Circle) {
                    indicator.GetComponent<Image>().fillMethod = Image.FillMethod.Radial360;
                }
                else {
                    indicator.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;
                }

                var = p.variableName;
                val = dataDict[var];
                indicator.GetComponent<Image>().fillAmount = (float)(val % 1.0);
            }
            else if (p.type == MapPolicy.MapPolicyType.orientation) {  
                var = p.variableName;
                val = dataDict[var];
                indicator.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)(val % 1.0)));
            }
        }

        return indicator;
    }

    /// <summary>
    /// Get a map indicator from the indicators dictionary.
    /// </summary>
    /// <param name="robot"> The robot associated with the desired indicator. </param>
    /// <returns>
    /// Returns the Map Indicator associated with a specific robot, or a new Map Indicator if none has been assigned yet.
    /// </returns>
    private GameObject GetIndicator(Robot robot) { 
        if (!indicators.ContainsKey(robot)) {
            GameObject indicator = CreateIndicator();
            indicator.transform.SetParent(transform, false);
            indicators[robot] = indicator;
        }

        return indicators[robot];
    }

    /// <summary>
    /// Update the Unity scene. Called automatically each frame update.
    /// </summary>
    public override void Draw() {
        GameObject indicator = GetIndicator(this.robot);
        indicator.GetComponent<Image>().sprite = sprites[vis.GetDefaultShape()];
        indicator.GetComponent<Image>().color = vis.GetDefaultColor();

        string var;
        float val;

        foreach (MapPolicy p in policies) {
            if (p.type == MapPolicy.MapPolicyType.color) {
                var = p.variableName;
                val = dataDict[var];
                indicator.GetComponent<Image>().color = SetColor(val);
            }
            else if (p.type == MapPolicy.MapPolicyType.fillAmount) {
                var = p.variableName;
                val = dataDict[var];
                indicator.GetComponent<Image>().fillAmount = (float)(val % 1.0);
            }
            else if (p.type == MapPolicy.MapPolicyType.orientation) {
                var = p.variableName;
                val = dataDict[var];
                indicator.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)(val % 360.0)));
            }
        }
    }

    /// <summary>
    /// Update internal storage of data. Called automatically when data in corresponding Visualization class.
    /// </summary>
    /// <param name="data"> Dictionary of all data relevant to the visualization. </param>
    protected override void UpdateData(Dictionary<Robot, Dictionary<string, float>> data) {
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