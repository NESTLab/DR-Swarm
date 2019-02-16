using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class RangeIndicatorContainer : VisualizationContainer<RangeIndicator> {
    // Instances of VisualizationContainer have access to the container
    // RectTransform container: the RectTransform of the drawable area in the
    // canvas. NOT the same as canvas.GetComponent<RectTransform>()
    List<Robot> robots = new List<Robot>();
    List<string> variables = new List<string>();
    Dictionary<Robot, Dictionary<string, float>> dataDict = new Dictionary<Robot, Dictionary<string, float>>();

    private List<RangePolicy> policies;
    private GameObject indicator; // maybe make this a dictionary

    // Initialize things
    protected override void Start() {
        RangeIndicator vis = (RangeIndicator)visualization; // is this actually how to do this?
        policies = vis.GetPolicies(); 

        // now that I think about it, all of this probably needs to go in the draw method. 
        // probably need to wrap this in a try catch
        // TODO: make prefabs for these
        // TODO: figure out how tp have them all named the same thing
        switch (policies[0].shape) {
            case RangePolicy.IndicatorShape.Square:
                indicator = new GameObject("Indicator", typeof(Image));
                break;
            case RangePolicy.IndicatorShape.Circle:
                indicator = (GameObject)Instantiate(Resources.Load("Wedge"), transform);
                break;
            case RangePolicy.IndicatorShape.Triangle:
                indicator = (GameObject)Instantiate(Resources.Load("Triangle"), transform);
                break;
            case RangePolicy.IndicatorShape.Plus:
                indicator = (GameObject)Instantiate(Resources.Load("Plus"), transform);
                break;
            case RangePolicy.IndicatorShape.Check:
                indicator = (GameObject)Instantiate(Resources.Load("Check"), transform);
                break;
            case RangePolicy.IndicatorShape.Exclamation:
                indicator = (GameObject)Instantiate(Resources.Load("Exclamation"), transform);
                break;

        }
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() {
        
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
