using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class IndicatorContainer : VisualizationContainer<Indicator>
{
    // Instances of VisualizationContainer have access to two member variables
    // GameObject canvas: the canvas object
    // RectTransform container: the RectTransform of the drawable area in the
    // canvas. NOT the same as canvas.GetComponent<RectTransform>()

    // Initialize things
    protected override void Start()
    {
        // This line calls the start method in VisualizationContainer
        // Keep it cause it does things, and make sure its the first thing
        // called in Start()
        base.Start();
    }

    // Update stuff in Unity scene. Called automatically each frame update
    protected override void Draw()
    {
    }

    // Update internal storage of data. Called automatically when data in
    // corresponding Visualization class
    protected override void UpdateData(Dictionary<Robot, List<float>> data)
    {
    }
}