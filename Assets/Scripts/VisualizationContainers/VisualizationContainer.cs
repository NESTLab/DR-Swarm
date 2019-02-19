using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UniRx;

public abstract class VisualizationContainer<T> : MonoBehaviour where T : IVisualization
{
    // The transform of the container object. All objects for this visualization should
    // be parented to this RectTransform
    public RectTransform container;

    // The robot that this visualization container is displayed above
    public Robot robot;

    // References to subscriptions for the visualization name and the visualization data
    private IDisposable _visualizationSubscription;
    private IDisposable _visualizationDataSubscription;

    // The current observable visualization associated with this visualization container
    private IObservable<IVisualization> _visualizationObs;

    // The current IVisualization object, accessible by subclasses
    protected IVisualization visualization;

    // The name of the visualization assocciated with this container
    // This should only be set by the outside, NOT by subclasses
    string _visualizationName;
    public string visualizationName
    {
        get
        {
            return _visualizationName;
        }

        set
        {
            // Do nothing
            if (_visualizationName == value)
                return;

            // Set the object name and store the value
            name = value;
            _visualizationName = value;

            // Get an Obs<IVisualization> for this name
            _visualizationObs = VisualizationManager.Instance.GetObservableVisualization(value);

            // If we had already been subscribed to a Obs<IVisualization>, dispose of it
            if (_visualizationSubscription != null)
                _visualizationSubscription.Dispose();

            // Subscribe to the Obs<IVisualization>
            _visualizationSubscription = _visualizationObs.Subscribe(visualization =>
            {
                // Make sure that the subscribed visualization is the correct type
                if (visualization.GetType() != typeof(T))
                    throw new System.Exception("Assigned visualization type must be equal to container's specified visualization type");

                // Store the visualization for reference later
                this.visualization = visualization;

                // If the visualization has changed, dispose of the subscription to the old one's data
                if (_visualizationDataSubscription != null)
                    _visualizationDataSubscription.Dispose();

                // Subscribe to the data exposed by the visualization
                _visualizationDataSubscription = visualization.GetObservableData().Subscribe(this.UpdateData);
            });
        }
    }

	// Use this for initialization
	protected virtual void Start() { }
    protected virtual void Update()
    {
        this.Draw();
    }

    protected abstract void UpdateData(Dictionary<Robot, Dictionary<string, float>> data);
    public abstract void Draw();
}
