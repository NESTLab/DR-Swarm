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
            if (_visualizationName == value)
                return;

            name = value;
            _visualizationName = value;
            _visualizationObs = VisualizationManager.Instance.GetObservableVisualization(value);

            _visualizationObs.Subscribe(visualization =>
            {
                if (visualization.GetType() != typeof(T))
                    throw new System.Exception("Assigned visualization type must be equal to container's specified visualization type");

                if (_visualizationSubscription != null)
                    _visualizationSubscription.Dispose();
                
                _visualizationSubscription = visualization.GetObservableData().Subscribe(v => {
                    this.UpdateData(v);
                });
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
