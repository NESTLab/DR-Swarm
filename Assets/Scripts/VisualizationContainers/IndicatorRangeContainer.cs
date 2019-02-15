using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorRangeContainer : VisualizationContainer<Indicator<RangePolicy>> {
    // Instances of VisualizationContainer have access to the container
    // RectTransform container: the RectTransform of the drawable area in the
    // canvas. NOT the same as canvas.GetComponent<RectTransform>()

    // Initialize things
    protected override void Start() {
    }

    // Update stuff in Unity scene. Called automatically each frame update
    public override void Draw() {
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
    protected override void UpdateData(Dictionary<Robot, Dictionary<string, float>> data) {

    } 
}
