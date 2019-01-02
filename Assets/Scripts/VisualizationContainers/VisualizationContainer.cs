using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UniRx;

public abstract class VisualizationContainer<T> : MonoBehaviour {

    protected GameObject canvas;
    protected RectTransform container;
    
    private IVisualization _visualization;
    public IVisualization visualization
    {
        get
        {
            return _visualization;
        }

        set
        {
            if (value.GetType() != typeof(T))
                throw new System.Exception("Assigned visualization type must be equal to container's specified visualization type");

            _visualization = value;
            _visualization.getObservableData().Subscribe(v => {
                this.UpdateData(v);
            });
        }
    }

	// Use this for initialization
	protected virtual void Start() {
        canvas = (GameObject)Instantiate(Resources.Load("VisualizationCanvas"), transform);
        container = canvas.transform.Find("Container").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    protected virtual void Update () {
        canvas.transform.LookAt(GameObject.Find("Camera").transform);

        // Rotate 180 around Y axis, because LookAt points the Z axis at the camera
        // when instead we want it pointing away from the camera
        canvas.transform.Rotate(new Vector3(0, 180, 0), Space.Self);

        this.Draw();
    }
    
    protected abstract void UpdateData(Dictionary<Robot, List<float>> data);
    protected abstract void Draw();
}
