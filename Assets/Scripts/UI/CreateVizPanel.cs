using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateVizPanel : MonoBehaviour
{
    public GameObject vizPanel;
    float initpos = -37f;

    public List<IVisualization> allVizs = new List<IVisualization>();
    public List<string> allVizsNames = new List<string>();
    public int totalViz = 0;
    float offset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        allVizs = UIManager.Instance.allVizs;
        allVizsNames = UIManager.Instance.allVizsNames;
        if (vizPanel != null)
        {
            if (allVizs.Count > 0)
            {
                foreach (IVisualization viz in allVizs)
                {
                    GameObject vizPrefab = (GameObject)Instantiate(Resources.Load("SingleVizUIPrefab"), transform);
                    vizPrefab.transform.SetParent(vizPanel.transform, false);
                    RectTransform t = vizPrefab.GetComponent<RectTransform>();
                    t.sizeDelta = new Vector2(0, 75f);
                    t.anchorMax = new Vector2(1f, 1f);
                    t.anchorMin = new Vector2(0f, 1f);
                    t.anchoredPosition = new Vector2(1f, initpos);
                    t.pivot = new Vector2(.5f, .5f);
                    totalViz++;
                    offset = offset + -70f;

                }
            }
        }
    }

    // Update is called once per frame
    public int i = 0;
    void Update()
    {
        allVizs = UIManager.Instance.allVizs;
        allVizsNames = UIManager.Instance.allVizsNames;
        if (vizPanel != null)
        {
            if (allVizs.Count != totalViz)
            {
                i = 0;
                foreach (Transform child in vizPanel.transform)
                {
                    Destroy(child.gameObject);
                }
                offset = 0;
                if (allVizs.Count > 0 )
                {
                    foreach (IVisualization viz in allVizs)
                    {
                        GameObject vizPrefab = (GameObject)Instantiate(Resources.Load("SingleVizUIPrefab"), transform);
                        vizPrefab.transform.SetParent(vizPanel.transform);
                        RectTransform t = vizPrefab.GetComponent<RectTransform>();
                        t.sizeDelta = new Vector2(0, 75f);
                        t.anchorMax = new Vector2(1f, 1f);
                        t.anchorMin = new Vector2(0f, 1f);
                        t.anchoredPosition = new Vector2(1f, initpos + offset);
                        t.pivot = new Vector2(.5f, .5f);
                        totalViz++;
                        Text t2 = vizPrefab.transform.Find("Name").GetComponent<Text>();
                        t2.text = allVizsNames[i];
                        Button remv = vizPrefab.transform.Find("rmvViz").GetComponent<Button>();
                        string name = allVizsNames[i];
                        remv.onClick.AddListener(delegate { removeViz(name, i, viz); });
                        i++;
                        offset = offset + -70f;

                    }
                    totalViz = allVizs.Count;
                }
            }
        }
    }

    void removeViz(string title, int i, IVisualization vz)
    {
        allVizs.Remove(vz);
        allVizsNames.Remove(title);
        VisualizationManager.Instance.RemoveVisualization(title);
        UIManager.Instance.allVizs = allVizs;
        UIManager.Instance.allVizsNames = allVizsNames;
        Debug.Log("Viz count to " + UIManager.Instance.allVizs.Count);
    }

}
