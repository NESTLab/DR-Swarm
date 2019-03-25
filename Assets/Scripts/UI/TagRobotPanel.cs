using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that added the tags prefabs to the robot panel.
/// </summary>
public class TagRobotPanel : MonoBehaviour
{
    public GameObject tagPanel; //Parent Panel, set when adding script
    float initpos = -37f; //Position offset for the prefabs

    public List<Tag> allTags = new List<Tag>();//All Visualizations, IViz
    public List<string> all = new List<string>(); //Visualization names
    public int totalViz = 0; // Total prefabs the script needs to add
    float offset = 0f; // Offset for when a prefab gets added
    public int i = 0;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    { }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        allTags = UIManager.Instance.allTags; //Get the vizs
        if (tagPanel != null) //Checking if the panel is active (not null)
        {
            if (allTags.Count != totalViz) // Only Update if we need to add/remove prefabs from the panel
            {
                i = 0;
                foreach (Transform child in tagPanel.transform) //Get everything out of the panel
                {
                    Destroy(child.gameObject);
                }
                offset = 0; //Reset the Offset
                if (allTags.Count > 0)
                {
                    foreach (Tag tag in allTags)
                    {
                        GameObject tagPrefab = (GameObject)Instantiate(Resources.Load("UI/TagRobotPanelPrefab"), transform); //Initialize the prefab
                        tagPrefab.transform.SetParent(tagPanel.transform); //All the prefabs must have the same parent
                        RectTransform t = tagPrefab.GetComponent<RectTransform>(); //Set the position
                        t.sizeDelta = new Vector2(0, 75f);
                        t.anchorMax = new Vector2(1f, 1f);
                        t.anchorMin = new Vector2(0f, 1f);
                        t.anchoredPosition = new Vector2(1f, initpos + offset);
                        t.pivot = new Vector2(.5f, .5f);
                        //Set the componenets of the prefab
                        Toggle t2 = tagPrefab.transform.Find("TagToggle").GetComponent<Toggle>();
                        t2.GetComponentInChildren<Text>().text = allTags[i].name;
                        string name = allTags[i].name;
                        List<string> bots = new List<string>();

                        //Maitance variables
                        totalViz++;
                        i++;
                        offset = offset + -50f;
                    }
                    totalViz = allTags.Count;
                }
            }
        }
    }
}
