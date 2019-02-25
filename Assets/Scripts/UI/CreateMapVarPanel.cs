using shapeNamespace;
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
    public Button addFillButton; //Add Fill policy
    public Button sendVarsButton;
    public Dropdown defaultShape; //dropdown for shape
    public Dropdown defaultColor;

    private List<string> colors = new List<string> { "Red", "Blue", "Green", "Yellow", "Orange", "Pink",  "White", "Black" }; // How the User sees the colors, will be converted to hex values when sending
    public List<string> shapes = new List<string> { "Check", "Circle", "!", "Plus", "Square", "Triangle", "Arrow" };//List of shape options
    public int mapCPols = 0;
    public int mapOPols = 0;
    public int mapFPols = 0;
    private bool mapC = false;
    private bool mapO = false;
    private bool mapF = false;
    public GameObject mapColorPolicy;// = new GameObject();
    public GameObject mapFillPolicy;// = new GameObject();
    public GameObject mapOrientPolicy;// = new GameObject();



    public List<int> selectedColors = new List<int>();//Used when updating panel
    public List<int> selectedVarsC = new List<int>();
    public List<int> selectedVarsO = new List<int>();
    public List<int> selectedVarsF = new List<int>();

    private bool toggleClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        mapColorPolicy = new GameObject();
        mapFillPolicy = new GameObject();
        mapOrientPolicy = new GameObject();
        UIManager.Instance.updateTotalVars();
        var_DropOptions = UIManager.Instance.wantedVars;
        defaultShape.ClearOptions();
        defaultShape.AddOptions(shapes);
        defaultColor.ClearOptions();
        defaultColor.AddOptions(colors);

        addColorButton.onClick.AddListener(AddPolicyColor);
        addOrientationButton.onClick.AddListener(AddPolicyOrientation);
        addFillButton.onClick.AddListener(AddPolicyFill);
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
                if (mapC || mapO || mapF)
                {
                    foreach (Transform child in optionPanel.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                    Debug.Log("Updating");
                    offset = 0;
                    allPolicies.Clear();
                    if (mapColorPolicy.transform.childCount > 0)
                    {
                        mapCPols = 0;
                        AddPolicyColor();
                    }
                    if (mapOrientPolicy.transform.childCount > 0)
                    {
                        mapOPols = 0;
                        AddPolicyOrientation();
                    }
                    if (mapFillPolicy.transform.childCount > 0)
                    {
                        mapFPols = 0;
                        AddPolicyFill();
                    }
                    mapC = false;
                    mapO = false;
                    mapF = false;

                }
            }
        }

    }

    //Add a single prefab at spot i 
    private void addOneColorPrefab(int i)
    {
        GameObject policyPrefab = (GameObject)Instantiate(Resources.Load("UI/MapColorPolicyPrefab"), optionPanel.transform);
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
        remv.onClick.AddListener(() => { mapColorPolicy = new GameObject(); mapC = true; mapCPols--; UIManager.Instance.Options--; addColorButton.interactable = true; getSelectedVarsO(x); getSelectedVarsF(x); });

        mapColorPolicy = policyPrefab;
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
        remv.onClick.AddListener(() => { mapOrientPolicy = new GameObject(); mapO = true; mapOPols--; UIManager.Instance.Options--; addOrientationButton.interactable = true; getSelectedVarsC(x); getSelectedVarsF(x); });
        mapOrientPolicy = policyPrefab;
    }
    private void addOneFillPrefab(int i)
    {
        GameObject policyPrefab = (GameObject)Instantiate(Resources.Load("MapFillPolicyPrefab"), optionPanel.transform);
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

        Dropdown varD = policyPrefab.transform.Find("DropdownVar").GetComponent<Dropdown>();
        varD.ClearOptions();
        varD.AddOptions(var_DropOptions);
        if (selectedVarsF.Count > 0)
        {
            varD.value = selectedVarsF[0];
        }

        Button remv = policyPrefab.transform.Find("rmvButton").GetComponent<Button>();
        int x = i;
        remv.onClick.AddListener(() => { mapFillPolicy = new GameObject(); mapF = true; mapFPols--; UIManager.Instance.Options--; addFillButton.interactable = true; getSelectedVarsC(x); getSelectedVarsO(x); });

        mapFillPolicy = policyPrefab;
    }



    private void getSelectedVarsC(int i)
    {
        selectedColors.Clear();
        selectedVarsC.Clear();

        GameObject g = mapColorPolicy;
        if (g.transform.childCount > 0)
        {
            Dropdown color = g.transform.Find("DropdownColor").GetComponent<Dropdown>();
            Dropdown varD = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
            int valueC = color.value;
            int valueD = varD.value;
            selectedColors.Add(valueC);
            selectedVarsC.Add(valueD);
        }

    }
    private void getSelectedVarsO(int i)
    {
        selectedColors.Clear();
        selectedVarsO.Clear();
        if (i == 1) { i = 0; }
        else if (i == 0) { i = 1; }
        GameObject g = mapOrientPolicy;
        if (g.transform.childCount > 0)
        {
            Toggle orient = g.transform.Find("OrientToggle").GetComponent<Toggle>();
            toggleClicked = orient.isOn;
            Dropdown varD = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
            int valueD = varD.value;
            selectedVarsO.Add(valueD);
        }
    }
    private void getSelectedVarsF(int i)
    {
        selectedVarsF.Clear();
        GameObject g = mapFillPolicy;
        if (g.transform.childCount > 0)
        {
            Dropdown varD = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
            int valueD = varD.value;
            selectedVarsF.Add(valueD);
        }
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

    private void AddPolicyFill()
    {
        int currOpps = allPolicies.Count;
        int totalOptions = UIManager.Instance.TotalOptions;
        if (totalOptions > currOpps)
        {
            addOneFillPrefab(currOpps);
            UIManager.Instance.Options++;
            addFillButton.interactable = false;
            mapFPols++;
        }
    }



    public void sendPolicies()
    {
        List<string> colorsSelected = new List<string>();
        List<string> shapesSelected = new List<string>();
        Color defaultColored = Color.white;
        List<MapPolicy> policies = new List<MapPolicy>();

        if (mapColorPolicy.transform.childCount > 0)
        {
            GameObject g = mapColorPolicy;
            Dropdown var = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
            List<Dropdown.OptionData> listv = var.options;
            string varChosen = listv[var.value].text;

            MapPolicy mp1 = new MapPolicy("color", varChosen, MapPolicy.MapPolicyType.color);
            policies.Add(mp1);

        }
        if (mapOrientPolicy.transform.childCount > 0)
        {
            GameObject g = mapOrientPolicy;
            Dropdown var = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
            List<Dropdown.OptionData> listv = var.options;
            string varChosen = listv[var.value].text;

            MapPolicy mp2 = new MapPolicy("orientation", varChosen, MapPolicy.MapPolicyType.orientation);
            policies.Add(mp2);

        }
        if (mapFillPolicy.transform.childCount > 0)
        {
            GameObject g = mapFillPolicy;
            Dropdown var = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
            List<Dropdown.OptionData> listv = var.options;
            string varChosen = listv[var.value].text;

            MapPolicy mp3 = new MapPolicy("fill", varChosen, MapPolicy.MapPolicyType.fillAmount);
            policies.Add(mp3);

        }

        string defaultShapeString = defaultShape.options[defaultShape.value].text;
        string defaultColorString = defaultColor.options[defaultColor.value].text; //Color.Red
        if (ColorUtility.TryParseHtmlString(defaultColorString, out defaultColored)) { }
        else { defaultColored = Color.red; }

        UIManager.Instance.allMPolicies = policies;
        UIManager.Instance.sentColor = defaultColored;
        UIManager.Instance.sentShape = GetShape("default", defaultShapeString);
        //Debug.Log("Shape d" + GetShape("default", defaultShapeString));
        UIManager.Instance.addGraph = true;
    }

    public Color GetColor(string c, Color d)
    {
        if (c == "Default") { return d; }
        else
        {
            Color newCol;
            if (ColorUtility.TryParseHtmlString(c, out newCol))
            {
                return newCol;
            }
            else { return Color.red; }
        }
    }


    public IndicatorShape GetShape(string s, string d)
    {
        if (s == "Check") { return IndicatorShape.Check; }
        else if (s == "Circle") { return IndicatorShape.Circle; }
        else if (s == "!") { return IndicatorShape.Exclamation; }
        else if (s == "Plus") { return IndicatorShape.Plus; }
        else if (s == "Triangle") { return IndicatorShape.Triangle; }
        else if (s == "Square") { return IndicatorShape.Square; }
        else if (s == "Arrow") { return IndicatorShape.Arrow; }
        else
        {
            if (d == "Check") { return IndicatorShape.Check; }
            else if (d == "Circle") { return IndicatorShape.Circle; }
            else if (d == "!") { return IndicatorShape.Exclamation; }
            else if (d == "Plus") { return IndicatorShape.Plus; }
            else if (d == "Triangle") { return IndicatorShape.Triangle; }
            else if (d == "Square") { return IndicatorShape.Square; }
            else if (d == "Arrow") { return IndicatorShape.Arrow; }

        }
        return IndicatorShape.Check;
    }



}
