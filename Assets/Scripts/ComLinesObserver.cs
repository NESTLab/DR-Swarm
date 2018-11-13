using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComLinesObserver : MonoBehaviour
{
    // Creates one LineRenderer for each robot pair and sets it to visible if robot setting indicates com line functionality
    public Color c1 = Color.blue;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 2;
    //GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot"); //This does not work here

    // initialization of comLines
    void Start () {
        //int n = robots.Length;
        int n = 5;
        for(int i = 0; i < (n*(n-1)/2); i++)
        {
            // creates a linerenderer for each pair
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.enabled = true;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 1.2f;
            lineRenderer.positionCount = lengthOfLineRenderer;
            lineRenderer.startWidth = 0.01f;
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
	void Update () {
        GameObject[] robots = GameObject.FindGameObjectsWithTag("Robot");
        LineRenderer[] lineRenderers = gameObject.GetComponents<LineRenderer>();
        // n is the number of robots with lines
        int n = 5;
        // k is the number of lines (# of unique robot pairs)
        int k = n*(n-1)/2;
        // for the number of robots minus 1
        for (int i=0;i<n;i++)
        {
            // iterate through the number of robots, going through 1 fewer robot each iteration
            for (int j=i+1;j<n-i-1;j++)
            {
                lineRenderers[k].enabled = true;
                lineRenderers[k].SetPosition(0, robots[i].transform.position);
                lineRenderers[k].SetPosition(1, robots[j].transform.position);
                k++;
            }
        }


        /*for(int i=0; i < lineRenderers.Length/robots.Length; i++)
        {
            // change logic to say "if either of the robots settings indicate they don't want comLines..." execute
            if ( 0 == 1 || 0 == 2)
            {
                lineRenderers[i].enabled = false;
            }
            else
            {
                lineRenderers[i].enabled = true;
            }
            for( int j = 0; j < robots.Length; j++ )
            {
                lineRenderers[i+robots.Length].SetPosition(0, robots[j].transform.position);
                lineRenderers[i+robots.Length].SetPosition(1, robots[]);
            }
            
            /*
            if (i<robots.Length)
            {
                lineRenderers[i].SetPosition(0, robots[i].transform.position);
                lineRenderers[i].SetPosition(1, robots[i + 1].transform.position);
            }
            else
            {
                lineRenderers[i].SetPosition(0, robots[i].transform.position);
                lineRenderers[i].SetPosition(1, robots[0].transform.position);
            }
            
            
        }*/
		
	}
}
