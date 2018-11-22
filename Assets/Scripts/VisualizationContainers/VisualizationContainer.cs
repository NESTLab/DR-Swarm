using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public abstract class VisualizationContainer : MonoBehaviour {

    [SerializeField] private GameObject canvasPrefab;

    protected GameObject canvas;
    protected RectTransform container;
    protected IVisualization visualization;
    
    public VisualizationContainer(IVisualization visualization) {
        this.visualization = visualization;
        this.visualization.getObservableData().Subscribe(value => {
            this.UpdateData(value);
            this.Draw();
        });
    }

	// Use this for initialization
	void Start () {
        canvas = Instantiate(canvasPrefab, transform);
        container = canvas.transform.Find("Container").GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        canvas.transform.LookAt(GameObject.Find("ARCamera").transform);

        // Rotate 180 around Y axis, because LookAt points the Z axis at the camera
        // when instead we want it pointing away from the camera
        canvas.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
    }
    
    protected abstract void UpdateData(Dictionary<Robot, List<float>> data);
    protected abstract void Draw();
}
