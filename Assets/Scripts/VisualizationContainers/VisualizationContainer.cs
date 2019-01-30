using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UniRx;

public abstract class VisualizationContainer<T> : MonoBehaviour where T : IVisualization
{
    public RectTransform container;

    private IDisposable _visualizationSubscription;
    private IDisposable _visualizationDataSubscription;
    private IObservable<IVisualization> _visualizationObs;

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

    protected abstract void UpdateData(Dictionary<Robot, List<float>> data);
    public abstract void Draw();
}
