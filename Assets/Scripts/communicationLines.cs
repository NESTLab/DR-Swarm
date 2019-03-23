using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Vuforia;

public class communicationLines : MonoBehaviour {
    // Creates one LineRenderer for each robot pair and sets it to visible,
    // if robot setting indicates com line functionality
    public Color c1 = Color.blue;   // sets color 1 of gradient
    public Color c2 = Color.red;    // sets color 2 of gradient
    public int lengthOfLineRenderer = 2;    // sets length of line renderer (determines # of shapes on line)
    GameObject[][] spheres = new GameObject[0][];
    public float animateIndex = 0; // start at 0, determines position of spheres along line
    public float animateSpeed = 1; // this shouldn't go too high or low; recommended range: 0.5 - 2
    public DateTime initTime;
    public int comDisplayTimeDelta = 7;
    

    // runs once at the beginning
    void Start()
    {
        initTime = System.DateTime.Now;
        lengthOfLineRenderer = 2;
        //int n = robots.Length;
        int n = 5;
        for (int i = 0; i < (n * (n - 1) / 2); i++)
        {
            // create arbitrary game object
            GameObject go = new GameObject();
            go.name = "LineRendererObject" + i.ToString();
            go.transform.parent = gameObject.transform;
            // creates a linerenderer for each pair, adds it to the game object
            LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
            lineRenderer.enabled = true; // visible/enabled by default
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 1.2f;
            //Debug.Log("length of Linerenderer: " + lengthOfLineRenderer);
            lineRenderer.positionCount = lengthOfLineRenderer;
            lineRenderer.startWidth = 0.01f; // same start and end width, this is a decent size
            lineRenderer.endWidth = 0.01f;

            // A simple 2 color gradient with a fixed alpha of 0.7f.
            float alpha = 0.7f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                );
            lineRenderer.colorGradient = gradient;
        }
    }

    // Update is called once per frame
    void Update()
    {
        animateIndex = (animateIndex + animateSpeed) % 100; // percentage between 0 and 100
        GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot"); // could use a better "get all robots function"
        LineRenderer[] lineRenderers = gameObject.GetComponentsInChildren<LineRenderer>(); // could use a better "get these lineRenderers" function
        // destroy any comLine Shapes so they don't stick around
        GameObject[] comLineShapes = GameObject.FindGameObjectsWithTag("comLineShape");
        foreach (GameObject shape in comLineShapes)
        {
            Destroy(shape);
        }
        spheres = new GameObject[lineRenderers.Length][];
        // n is the number of (possible) robots with lines
        int n = 5;
        // k is the number of lines (# of unique robot pairs)
        // int k = n * (n - 1) / 2;
        // for the number of robots minus 1
        int k = 0;
        for (int i = 0; i < n - 1; i++)
        {
            // iterate through the number of robots, going through 1 fewer robot each iteration
            for (int j = 0; j < n - i - 1; j++)
            {
                //Debug.Log("(i: " + i.ToString() + ", j+i:" + (i+j).ToString() + ", k: " + k.ToString() + ")");
                TrackableBehaviour robotATracker = robots[i].GetComponent<TrackableBehaviour>();
                TrackableBehaviour robotBTracker = robots[j + i + 1].GetComponent<TrackableBehaviour>();
                // if either robot of a pair is tracked && if time is within tolerance
                DateTime latestComTime = initTime; //get the time of the most recent communication TODO: replace with gathered data
                DateTime currentTime = System.DateTime.Now; // get the current time
                if (robotATracker.CurrentStatus == TrackableBehaviour.Status.TRACKED &&
                    robotBTracker.CurrentStatus == TrackableBehaviour.Status.TRACKED &&
                    currentTime < latestComTime.AddSeconds(comDisplayTimeDelta + (k*2)) // latest + delta < current
                    )
                {
                    //then draw a line between them
                    lineRenderers[k].enabled = true;

                    // and draw shapes along that line (if desired)
                    // for each point on a linerenderer
                    // first make a sphere array
                    GameObject[] localSphereArray = new GameObject[lineRenderers[k].positionCount];
                    for (int l=0; l < lineRenderers[k].positionCount; l++)
                    {
                        // draw line at correct points
                        // take start and end points
                        Vector3 ptI = robots[i].transform.position;
                        Vector3 ptF = robots[i+j+1].transform.position;

                        float p1x = ptI.x;
                        float p1y = ptI.y;
                        float p1z = ptI.z;

                        float p2x = ptF.x;
                        float p2y = ptF.y;
                        float p2z = ptF.z;

                        // not returning fractions, causes errors with > 2 spheres
                        float ptRatio = l / lengthOfLineRenderer;
                        //Debug.Log("l = " + l.ToString());
                        //Debug.Log("LoLR = " + lengthOfLineRenderer.ToString());
                        //Debug.Log("l / LoLR = " + ptRatio.ToString());

                        float posMod = (l + ptRatio);
                        //Debug.Log("posMod = " + posMod);
                        float negMod = (1 - (l + ptRatio));

                        float xfi = ((p1x * (posMod)) + (p2x * (negMod)));
                        float yfi = ((p1y * (posMod)) + (p2y * (negMod)));
                        float zfi = ((p1z * (posMod)) + (p2z * (negMod)));

                        Vector3 ptMid = new Vector3(((ptI.x*posMod) + (ptF.x*negMod)), ((ptI.y * posMod) + (ptF.y * negMod)), ((ptI.z * posMod) + (ptF.z * negMod)));

                        lineRenderers[k].SetPosition(l, (ptMid));
                        // generate a sphere and place it at each point on the linerenderer
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.tag = "comLineShape";
                        sphere.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Specular"));
                        sphere.gameObject.transform.localScale += new Vector3((float)-1.025,(float)-1.025,(float)-1.025);
                        // position = linrenderer position
                        sphere.transform.position = lineRenderers[k].GetPosition(l); // set it to sphere origin

                        // animation math!
                        //take start and end point -> this is where to reverse direction !!
                        Vector3 lineStartPoint = lineRenderers[k].GetPosition(0);
                        Vector3 lineEndPoint = lineRenderers[k].GetPosition(lengthOfLineRenderer-1);
                        //store their coordinates
                        float x1 = lineStartPoint.x;
                        float y1 = lineStartPoint.y;
                        float z1 = lineStartPoint.z;

                        float x2 = lineEndPoint.x;
                        float y2 = lineEndPoint.y;
                        float z2 = lineEndPoint.z;

                        // calculate as sum of scaled start & end positions
                        float lScaled = l*100 / lengthOfLineRenderer % 100;
                        //Debug.Log("lScaled = " + lScaled);
                        float posScale = (animateIndex + lScaled) % 100;
                        float negScale = (100 - ((animateIndex + lScaled) % 100));

                        float xf = ((x1 * (posScale) / 100) + (x2 * (negScale)/100));
                        float yf = ((y1 * (posScale) / 100) + (y2 * (negScale)/100));
                        float zf = ((z1 * (posScale) / 100) + (z2 * (negScale)/100));
                        // set the new position
                        sphere.transform.position = new Vector3(xf,yf,zf);
                        //Debug.Log("l = " + l);
                        //Debug.Log("x, y, z" + xf + ", " + yf + ", " + zf );

                        // set shape to be visible
                        sphere.SetActive(true);
                        // add it to the local array
                        localSphereArray[l] = sphere;
                    }
                    // add individual linerenders spheres to array of all of them
                    spheres[k] = localSphereArray;

                   
                } else
                {
                    // if not tracked, don't draw linerenderers
                    lineRenderers[k].enabled = false;
                }
                // look at the next robot in the next loop
                k++;
            }
        }
    }
}