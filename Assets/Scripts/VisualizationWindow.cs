using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Vuforia;
using Image = UnityEngine.UI.Image; // since we have 2 images, maybe this is the best way

public class VisualizationWindow : MonoBehaviour {
    // Vuforia code was grabbed from the Vuforia Developer Library
    private Robot robot;
    private GameObject canvas;
    private RectTransform container;

    private HashSet<string> visualizationContainers;

    private long startTime;
    private int darkFlag = 0;
    private int threshold = 130; // I dunno, about halfway

    private Vuforia.Image.PIXEL_FORMAT mPixelFormat = Vuforia.Image.PIXEL_FORMAT.GRAYSCALE;

    private bool mAccessCameraImage = true;
    private bool mFormatRegistered = false;

    // Use this for initialization
    void Start() {
        canvas = (GameObject)Instantiate(Resources.Load("VisualizationCanvas"), transform);
        container = canvas.transform.Find("Container").GetComponent<RectTransform>();
        visualizationContainers = new HashSet<string>();
        robot = DataManager.Instance.GetRobot(name);

        startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        // Subscribe to the set of visualizations for this robot
        // A callback gets called if a visualization was added or removed for the robot
        VisualizationManager.Instance.GetObservableVisualizationsForRobot(robot).Subscribe(VisualizationListUpdated);

        // For now, set canvas color based on startup - eventually update every now and then
        // TESTING

        //CameraDevice.Instance.SetFrameFormat(Vuforia.Image.PIXEL_FORMAT.GRAYSCALE, true); // only need greyscale for contrast

        // Register Vuforia life-cycle callbacks:
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnPause);
    }

    void OnVuforiaStarted() {

        // Try register camera image format
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true)) {
            Debug.Log("Successfully registered pixel format " + mPixelFormat.ToString());

            mFormatRegistered = true;
        }
        else {
            Debug.LogError(
                "\nFailed to register pixel format: " + mPixelFormat.ToString() +
                "\nThe format may be unsupported by your device." +
                "\nConsider using a different pixel format.\n");

            mFormatRegistered = false;
        }

    }

    /// <summary>
    /// Called each time the Vuforia state is updated
    /// </summary>
    void OnTrackablesUpdated() {
        long currTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (mFormatRegistered) {
            if (mAccessCameraImage) {
                if (currTime - startTime > 2000) {  // check every 2 seconds, for now
                    Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
                    startTime = currTime;

                    if (image != null) {
                        Debug.Log(
                            "\nImage Format: " + image.PixelFormat +
                            "\nImage Size:   " + image.Width + "x" + image.Height +
                            "\nBuffer Size:  " + image.BufferWidth + "x" + image.BufferHeight +
                            "\nImage Stride: " + image.Stride + "\n"
                        );

                        byte[] pixels = image.Pixels;

                        if (pixels != null && pixels.Length > 0) {
                            /*
                            Debug.Log(
                                "\nImage pixels: " +
                                pixels[0] + ", " +
                                pixels[1] + ", " +
                                pixels[2] + ", ...\n"
                            );
                            */
                            System.Random random = new System.Random();
                            int counter = 0;
                            int total = 0;

                            while (counter < 100) {
                                int index = random.Next(0, pixels.Length);
                                byte data = pixels[index];
                                total += data;
                                counter++;
                            }

                            int avg = total / counter;
                            Debug.Log("average brightness: " + avg);
                             
                            if (avg < threshold) { // some threshold to trip the canvas color flag
                                darkFlag = 0; // white canvas 
                            }
                            else {
                                darkFlag = 1; // black canvas
                            }
                            SetWindowColor(darkFlag);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called when app is paused / resumed
    /// </summary>
    void OnPause(bool paused) {
        if (paused) {
            Debug.Log("App was paused");
            UnregisterFormat();
        }
        else {
            Debug.Log("App was resumed");
            RegisterFormat();
        }
    }

    /// <summary>
    /// Register the camera pixel format
    /// </summary>
    void RegisterFormat() {
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true)) {
            Debug.Log("Successfully registered camera pixel format " + mPixelFormat.ToString());
            mFormatRegistered = true;
        }
        else {
            Debug.LogError("Failed to register camera pixel format " + mPixelFormat.ToString());
            mFormatRegistered = false;
        }
    }

    /// <summary>
    /// Unregister the camera pixel format (e.g. call this when app is paused)
    /// </summary>
    void UnregisterFormat() {
        Debug.Log("Unregistering camera pixel format " + mPixelFormat.ToString());
        CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
        mFormatRegistered = false;
    }

    private void SetWindowColor(int dark) {
        GameObject background = canvas.transform.Find("Background").gameObject;
        GameObject containerBackground = canvas.transform.Find("ContainerBackground").gameObject;
        if (dark == 1) {
            Debug.Log("make dark");
            background.GetComponent<Image>().color = new Color((30f/255f), (30f / 255f), (30f / 255f));            
            containerBackground.GetComponent<Image>().color = new Color((64 / 255f), (64 / 255f), (64 / 255f));
        }
        if (dark == 0) {
            Debug.Log("make light");
            background.GetComponent<Image>().color = Color.grey;
            containerBackground.GetComponent<Image>().color = Color.white;
        }
    }

    private void VisualizationListUpdated(ISet<string> visualizationSet)
    { 
        // Compute the set of visualizations added and removed
        HashSet<string> addedVisualizations = new HashSet<string>(visualizationSet);
        addedVisualizations.ExceptWith(visualizationContainers);

        HashSet<string> removedVisualizations = new HashSet<string>(visualizationContainers);
        removedVisualizations.ExceptWith(visualizationSet);

        // Create and destory containers for added and removed visualizations
        foreach (string visualizationName in removedVisualizations)
        {
            Destroy(container.transform.Find(visualizationName).gameObject);
            visualizationContainers.Remove(visualizationName);
        }

        foreach (string visualizationName in addedVisualizations)
        {
            CreateVisualizationContainer(visualizationName);
            visualizationContainers.Add(visualizationName);
        }
    }

    private void CreateVisualizationContainer(string visualizationName)
    {
        // Create an empty 2D gameobject for the container to be stored in
        GameObject gameObject = new GameObject(visualizationName, typeof(Image));
        gameObject.transform.SetParent(container);
        gameObject.GetComponent<Image>().color = Color.clear;

        // Set the origin of the container to be in the bottom left
        RectTransform transform = gameObject.GetComponent<RectTransform>();
        transform.sizeDelta = container.GetComponent<RectTransform>().sizeDelta;
        transform.anchorMin = Vector2.zero;
        transform.anchorMax = Vector2.zero;
        transform.pivot = Vector2.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = new Quaternion(0, 0, 0, 0);

        // Create a container based off of the visualization type
        // TODO: Try using "visitor" pattern here
        Type visualizationType = VisualizationManager.Instance.GetVisualizationType(visualizationName);
        if (visualizationType == typeof(LineGraph))
        {
            LineGraphContainer container = gameObject.AddComponent<LineGraphContainer>();
            container.visualizationName = visualizationName;
            container.container = transform;
        }
        else if (visualizationType == typeof(PieChart))
        {
            PieChartContainer container = gameObject.AddComponent<PieChartContainer>();
            container.visualizationName = visualizationName;
            container.container = transform;
        }
        else if (visualizationType == typeof(PieChartMultiVar)) 
        {
            PieChartMultiVarContainer container = gameObject.AddComponent<PieChartMultiVarContainer>();
            container.visualizationName = visualizationName;
            container.container = transform;
        }
        else if (visualizationType == typeof(Indicator))
        {
            IndicatorContainer container = gameObject.AddComponent<IndicatorContainer>();
            container.visualizationName = visualizationName;
            container.container = transform;
        }
        else if (visualizationType == typeof(BarGraph))
        {
            BarGraphContainer container = gameObject.AddComponent<BarGraphContainer>();
            container.visualizationName = visualizationName;
            container.container = transform;
        } else
        {
            throw new Exception("Invalid visualization type.");
        }
    }

    // Update is called once per frame
    void Update() {
        canvas.transform.LookAt(GameObject.Find("Camera").transform);

        // Rotate 180 around Y axis, because LookAt points the Z axis at the camera
        // when instead we want it pointing away from the camera
        canvas.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
    }
}
