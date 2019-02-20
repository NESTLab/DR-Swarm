using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using shapeNamespace;

public class RangeIndicatorContainer : VisualizationContainer<RangeIndicator> {
    // Instances of VisualizationContainer have access to the container
    // RectTransform container: the RectTransform of the drawable area in the
    // canvas. NOT the same as canvas.GetComponent<RectTransform>()

    List<Robot> robots = new List<Robot>();
    List<string> variables = new List<string>();
    Dictionary<string, float> dataDict = new Dictionary<string, float>();

    private List<RangePolicy> policies;

    private Dictionary<Robot, GameObject> indicators;

    private Sprite square;
    private Sprite circle;
    private Sprite triangle;
    private Sprite check;
    private Sprite exclamation;
    private Sprite plus;

    private Dictionary<IndicatorShape, Sprite> sprites;

    // Initialize things
    protected override void Start() {
        // TODO: maybe remove
        base.Start();

        RangeIndicator vis = (RangeIndicator)visualization; 
        policies = vis.GetPolicies();

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

    private GameObject CreateIndicator(float value) {
        GameObject indicator = new GameObject("indicator", typeof(Image));

        foreach (RangePolicy p in policies) {
            if (p.range.x <= value && value < p.range.y) { // have the right policy
                IndicatorShape shape = p.shape;

                indicator.GetComponent<Image>().sprite = sprites[shape];

                return indicator;
            }
        }

        return indicator;
    }

    private GameObject GetIndicator(Robot robot, float value) {
        if (!indicators.ContainsKey(robot)) {
            GameObject indicator = CreateIndicator(value);
            indicator.transform.SetParent(transform, false); 
            indicators[robot] = indicator;
        }

        return indicators[robot];
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() {
        GameObject indicator = GetIndicator(this.robot, dataDict[variables[0]]);
        float value = dataDict[variables[0]];  // TODO: might be able to simplify this even more

        foreach (RangePolicy p in policies) {
            if (p.range.x <= value && value < p.range.y) { // have the right policy
                IndicatorShape shape = p.shape;
                indicator.GetComponent<Image>().sprite = sprites[shape];

                Color color = p.color;
                indicator.GetComponent<Image>().color = p.color;
            }
        }
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
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
