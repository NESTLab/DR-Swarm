using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateMapVarPanel : MonoBehaviour
{
    List<string> var_DropOptions = new List<string>();//Variable List
    public GameObject optionPanel; //Parent Panel, set when adding script
    public GameObject overallPanel; //Panel for the entire screen
    float initpos = -37f; //Position offset for the prefabs
    public List<GameObject> allPolicies = new List<GameObject>();//All Visualizations, IViz
    public int totalPolicies = 0; // Total prefabs the script needs to add
    float offset = 0f; // Offset for when a prefab gets added
    float offsetAddition = -50f;//Value to add to the offset after each policy is added
    public Button addColorButton; //Add color policy
    public Button addOrientationButton; //add orientation policy
    public Button sendVarsButton;
    public Dropdown defaultShape; //dropdown for shape
    private List<string> colors = new List<string> { "Red", "Blue", "Green", "Yellow", "Orange", "Pink", "Purple", "White", "Black" }; // How the User sees the colors, will be converted to hex values when sending
    public List<string> shapes = new List<string> { "Check", "Circle", "!", "Plus", "Square", "Triangle" };//List of shape options
    public int mapCPols = 0;
    public int mapOPols = 0;
    private bool mapC = false;
    private bool mapO = false;


    public List<int> selectedColors = new List<int>();//Used when updating panel
    public List<int> selectedVarsC = new List<int>();
    public List<int> selectedVarsO = new List<int>();
    private bool toggleClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.updateTotalVars();
        var_DropOptions = UIManager.Instance.wantedVars;
        defaultShape.ClearOptions();
        defaultShape.AddOptions(shapes);

        addColorButton.onClick.AddListener(AddPolicyColor);
        addOrientationButton.onClick.AddListener(AddPolicyOrientation);
        sendVarsButton.onClick.AddListener(sendPolicies);

    }

    // Update is called once per frame
    void Update()
    {
        UIManager.Instance.updateTotalVars();
        var_DropOptions = UIManager.Instance.wantedVars;
        //HOW TO UPDATE????
        if (overallPanel != null)
        {
            if (optionPanel.activeSelf)
            {
                if (mapC || mapO)
                {
                    foreach (Transform child in optionPanel.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                    Debug.Log("Updating");
                    offset = 0;
                    if (mapCPols > 0)
                    {
                        mapCPols = 0;
                        allPolicies.Clear();
                        AddPolicyColor();
                    }
                    else
                    {
                        mapOPols = 0;
                        allPolicies.Clear();
                        AddPolicyOrientation();
                    }
                    mapC = false;
                    mapO = false;


                }
            }
        }

    }

    //Add a single prefab at spot i 
    private void addOneColorPrefab(int i)
    {
        GameObject policyPrefab = (GameObject)Instantiate(Resources.Load("MapColorPolicyPrefab"), optionPanel.transform);
        policyPrefab.transform.SetParent(optionPanel.transform, false);
        RectTransform t = policyPrefab.GetComponent<RectTransform>();
        t.sizeDelta = new Vector2(0, 75f);
        t.anchorMax = new Vector2(1f, 1f);
        t.anchorMin = new Vector2(0f, 1f);
        t.anchoredPosition = new Vector2(1f, initpos + offset);
        t.localScale = Vector3.one;
        t.pivot = new Vector2(.5f, .5f);
        offset = offset + offsetAddition;
        allPolicies.Add(policyPrefab);

        Dropdown color = policyPrefab.transform.Find("DropdownColor").GetComponent<Dropdown>();
        Dropdown varD = policyPrefab.transform.Find("DropdownVar").GetComponent<Dropdown>();
        color.ClearOptions();
        color.AddOptions(colors);
        varD.ClearOptions();
        varD.AddOptions(var_DropOptions);
        if (selectedColors.Count > 0)
        {
            color.value = selectedColors[0];
            varD.value = selectedVarsC[0];
        }

        Button remv = policyPrefab.transform.Find("rmvButton").GetComponent<Button>();
        int x = i;
        remv.onClick.AddListener(() => { mapC = true; mapCPols--; UIManager.Instance.Options--; addColorButton.interactable = true; getSelectedVarsO(x); });
    }
    private void addOneOrientationPrefab(int i)
    {
        GameObject policyPrefab = (GameObject)Instantiate(Resources.Load("MapOrientationPolicyPrefab"), optionPanel.transform);
        policyPrefab.transform.SetParent(optionPanel.transform, false);
        RectTransform t = policyPrefab.GetComponent<RectTransform>();
        t.sizeDelta = new Vector2(0, 75f);
        t.anchorMax = new Vector2(1f, 1f);
        t.anchorMin = new Vector2(0f, 1f);
        t.anchoredPosition = new Vector2(1f, initpos + offset);
        t.localScale = Vector3.one;
        t.pivot = new Vector2(.5f, .5f);
        offset = offset + offsetAddition;
        allPolicies.Add(policyPrefab);

        Toggle orient = policyPrefab.transform.Find("OrientToggle").GetComponent<Toggle>();
        Dropdown varD = policyPrefab.transform.Find("DropdownVar").GetComponent<Dropdown>();
        varD.ClearOptions();
        varD.AddOptions(var_DropOptions);
        if (selectedVarsO.Count > 0)
        {
            varD.value = selectedVarsO[0];
            orient.isOn = toggleClicked;
        }

        Button remv = policyPrefab.transform.Find("rmvButton").GetComponent<Button>();
        int x = i;
        remv.onClick.AddListener(() => { mapO = true; mapOPols--; UIManager.Instance.Options--; addOrientationButton.interactable = true; getSelectedVarsC(x); });
    }


    private void getSelectedVarsC(int i)
    {
        selectedColors.Clear();
        selectedVarsC.Clear();
        if (i == 1) { i = 0; }
        else if (i == 0) { i = 1; }
        GameObject g = allPolicies[i];
        Dropdown color = g.transform.Find("DropdownColor").GetComponent<Dropdown>();
        Dropdown varD = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
        int valueC = color.value;
        int valueD = varD.value;
        selectedColors.Add(valueC);
        selectedVarsC.Add(valueD);

    }
    private void getSelectedVarsO(int i)
    {
        selectedColors.Clear();
        selectedVarsO.Clear();
        if (i == 1) { i = 0; }
        else if (i == 0) { i = 1; }
        GameObject g = allPolicies[i];
        Toggle orient = g.transform.Find("OrientToggle").GetComponent<Toggle>();
        toggleClicked = orient.isOn;
        Dropdown varD = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
        int valueD = varD.value;
        selectedVarsO.Add(valueD);


    }

    //Added to Add Color Policy button, will add a policy at the bottom of the list
    private void AddPolicyColor()
    {

        int currOpps = allPolicies.Count;
        int totalOptions = UIManager.Instance.TotalOptions;
        if (totalOptions > currOpps)
        {
            addOneColorPrefab(currOpps);
            UIManager.Instance.Options++;
            addColorButton.interactable = false;
            mapCPols++;
        }
    }

    private void AddPolicyOrientation()
    {

        int currOpps = allPolicies.Count;
        int totalOptions = UIManager.Instance.TotalOptions;
        if (totalOptions > currOpps)
        {
            addOneOrientationPrefab(currOpps);
            UIManager.Instance.Options++;
            addOrientationButton.interactable = false;
            mapOPols++;
        }
    }



    public void sendPolicies()
    {
        List<string> colorsSelected = new List<string>();
        List<string> shapesSelected = new List<string>();
        List<string> maxSelected = new List<string>();
        List<string> minSelected = new List<string>();

        foreach (GameObject g in allPolicies)
        {
            Dropdown color = g.transform.Find("DropdownColor").GetComponent<Dropdown>();
            Dropdown shape = g.transform.Find("DropdownShape").GetComponent<Dropdown>();
            List<Dropdown.OptionData> listC = color.options;
            List<Dropdown.OptionData> listS = shape.options;
            colorsSelected.Add(listC[color.value].text);
            shapesSelected.Add(listS[shape.value].text);
            InputField min = g.transform.Find("InputMin").GetComponent<InputField>();
            InputField max = g.transform.Find("InputMax").GetComponent<InputField>();
            minSelected.Add(min.text);
            maxSelected.Add(max.text);

        }
        string defaultShapeString = defaultShape.options[defaultShape.value].text;

        // TODO get colors to hex values 
        // TODO Update UI Manager with values


        UIManager.Instance.addGraph = true;
    }


}
