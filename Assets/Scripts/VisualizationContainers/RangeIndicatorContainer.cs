using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

// TODO: figure out if you can change the sprite attached to a game object
public class RangeIndicatorContainer : VisualizationContainer<RangeIndicator> {
    // Instances of VisualizationContainer have access to the container
    // RectTransform container: the RectTransform of the drawable area in the
    // canvas. NOT the same as canvas.GetComponent<RectTransform>()

    List<Robot> robots = new List<Robot>();
    List<string> variables = new List<string>();
    Dictionary<Robot, Dictionary<string, float>> dataDict = new Dictionary<Robot, Dictionary<string, float>>();

    private List<RangePolicy> policies;

    // ok, so I'm thinking of making an indicator for each new shape that's been activated, and then just hiding all the others
    private Dictionary<Robot, GameObject> indicators;
    private Dictionary<RangePolicy.IndicatorShape, GameObject> shapes;

    // Initialize things
    protected override void Start() {
        // TODO: maybe remove
        base.Start();

        RangeIndicator vis = (RangeIndicator)visualization; // is this actually how to do this?
        policies = vis.GetPolicies();

        indicators = new Dictionary<Robot, GameObject>();
        shapes = new Dictionary<RangePolicy.IndicatorShape, GameObject>();
        // I think this is causing big problems -- all of the shapes are showing up
        shapes[RangePolicy.IndicatorShape.Check] = (GameObject)Instantiate(Resources.Load("Check2"), transform);
        shapes[RangePolicy.IndicatorShape.Check].SetActive(false);
        shapes[RangePolicy.IndicatorShape.Circle] = (GameObject)Instantiate(Resources.Load("Wedge"), transform);
        shapes[RangePolicy.IndicatorShape.Circle].SetActive(false);
        shapes[RangePolicy.IndicatorShape.Exclamation] = (GameObject)Instantiate(Resources.Load("Exclamation2"), transform);
        shapes[RangePolicy.IndicatorShape.Exclamation].SetActive(false);
        shapes[RangePolicy.IndicatorShape.Plus] = (GameObject)Instantiate(Resources.Load("Plus2"), transform);
        shapes[RangePolicy.IndicatorShape.Plus].SetActive(false);
        shapes[RangePolicy.IndicatorShape.Square] = (GameObject)Instantiate(Resources.Load("Square"), transform); // need to get the transform right on this
        shapes[RangePolicy.IndicatorShape.Square].SetActive(false);
        shapes[RangePolicy.IndicatorShape.Triangle] = (GameObject)Instantiate(Resources.Load("Triangle"), transform);
        shapes[RangePolicy.IndicatorShape.Triangle].SetActive(false);
    }

    private GameObject CreateIndicator(float value) {
        //RangePolicy.IndicatorShape shape = RangePolicy.IndicatorShape.Square; // may not be the way we should do it
        GameObject indicator;

        foreach (RangePolicy p in policies) {
            if (p.range.x <= value && value < p.range.y) { // have the right policy
                RangePolicy.IndicatorShape shape = p.shape;

                indicator = shapes[shape]; // dunno if this is legal
                //indicator.SetActive(true);

                return indicator;

                /*
                switch (shape) {
                    case RangePolicy.IndicatorShape.Square:
                        indicator = new GameObject("Square", typeof(Image));
                        return indicator;
                        break;
                    case RangePolicy.IndicatorShape.Circle:
                        indicator = (GameObject)Instantiate(Resources.Load("Wedge"), transform);
                        return indicator;
                        break;
                    case RangePolicy.IndicatorShape.Triangle:
                        indicator = (GameObject)Instantiate(Resources.Load("Triangle"), transform);
                        return indicator;
                        break;
                    case RangePolicy.IndicatorShape.Plus:
                        indicator = (GameObject)Instantiate(Resources.Load("Plus2"), transform);
                        return indicator;
                        break;
                    case RangePolicy.IndicatorShape.Check:
                        indicator = (GameObject)Instantiate(Resources.Load("Check2"), transform);
                        return indicator;
                        break;
                    case RangePolicy.IndicatorShape.Exclamation:
                        indicator = (GameObject)Instantiate(Resources.Load("Exclamation2"), transform);
                        return indicator;
                        break;
                }
                */
            }
        }

        return new GameObject("Square", typeof(Image)); // ok I know this is wrong, but I'm lazy atm
    }

    private GameObject GetIndicator(Robot robot, float value) {
        if (!indicators.ContainsKey(robot)) {
            //GameObject blankKey = (GameObject)Instantiate(Resources.Load("LegendKey"), transform);
            GameObject indicator = CreateIndicator(value);
            indicator.transform.SetParent(transform, false); // is this right?
            indicators[robot] = indicator;
        }

        return indicators[robot];
    }

    private void UpdateIndicator(GameObject indicator, float value) {
        // update the shape and color based on policy
        foreach (RangePolicy p in policies) {
            if (p.range.x <= value && value < p.range.y) { // have the right policy
                // for the shape: maybe have a dictionary of all previously used shapes that we can just pull from
                    // would I need a different one for each robot? I really hope not
                RangePolicy.IndicatorShape shape = p.shape;
                indicator = shapes[shape]; // not sure this is right
                foreach (RangePolicy.IndicatorShape s in shapes.Keys) {
                    shapes[s].SetActive(false);
                }
                indicator.SetActive(true);

                Color color = p.color;
                indicator.GetComponent<Image>().color = p.color;
            }
        }
    }

    // TODO: simplify this now that we have robot field
    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() {
        Robot r = this.robot;
        GameObject indicator = GetIndicator(r, dataDict[r][variables[0]]); 

        UpdateIndicator(indicator, dataDict[r][variables[0]]); 
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
    protected override void UpdateData(Dictionary<Robot, Dictionary<string, float>> data) {
        foreach (Robot r in data.Keys) {
            if (!robots.Contains(r)) {
                robots.Add(r);
                dataDict[r] = new Dictionary<string, float>();
            }

            foreach (string var in data[r].Keys) {
                dataDict[r][var] = data[r][var];

                // this is not the best way to do it, but not seeing another option at the moment
                if (!variables.Contains(var)) {
                    variables.Add(var);
                }
            }
        }
    } 
}
