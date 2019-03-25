using shapeNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for the Map Policy Panel
/// </summary>
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
    public Dropdown defaultColor; //dropdown for color

    private List<string> colors = new List<string> { "Red", "Blue", "Green", "Yellow", "Magenta", "White", "Black" }; // How the User sees the colors, will be converted to hex values when sending
    public List<string> shapes = new List<string> { "Check", "Circle", "!", "Plus", "Square", "Triangle", "Arrow" };//List of shape options
    private bool mapC = false; //To tell we need to update
    private bool mapO = false; //To tell we need to update
    private bool mapF = false; //To tell we need to update
    public GameObject mapColorPolicy;// = new GameObject();
    public GameObject mapFillPolicy;// = new GameObject();
    public GameObject mapOrientPolicy;// = new GameObject();

    public List<int> selectedVarsC = new List<int>(); //Used when updating panel
    public List<int> selectedVarsO = new List<int>(); //Used when updating panel
    public List<int> selectedVarsF = new List<int>(); //Used when updating panel
    List<MapPolicy> editPolicies = new List<MapPolicy>();

    public bool editAdded = false;


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
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
        editPolicies = UIManager.Instance.editMapPolicys;

        addColorButton.onClick.AddListener(AddPolicyColor);
        addOrientationButton.onClick.AddListener(AddPolicyOrientation);
        addFillButton.onClick.AddListener(AddPolicyFill);
        sendVarsButton.onClick.AddListener(sendPolicies);
        if (editPolicies.Count > 0)
        {
            int sd = getiValueFromList(UIManager.Instance.editDShape.ToString(), shapes);
            defaultShape.value = sd;
            string col = getColorString(UIManager.Instance.editDColor);
            int cd = getiValueFromList(col, colors);
            defaultColor.value = cd;
        }

    }

    /// <summary>
    /// Update is called once per frame
    /// , Updates the panel as needed
    /// </summary>
    void Update()
    {
        UIManager.Instance.updateTotalVars();
        var_DropOptions = UIManager.Instance.wantedVars;
        if (overallPanel != null)
        {
            if (optionPanel.activeSelf)
            {
                editPolicies = UIManager.Instance.editMapPolicys;
                if (mapC || mapO || mapF)
                {
                    //Clears the panel
                    foreach (Transform child in optionPanel.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                    offset = 0;
                    allPolicies.Clear();

                    if (mapColorPolicy.transform.childCount > 0)
                    {
                        AddPolicyColor();
                    }
                    if (mapOrientPolicy.transform.childCount > 0)
                    {
                        AddPolicyOrientation();
                    }
                    if (mapFillPolicy.transform.childCount > 0)
                    {
                        AddPolicyFill();
                    }
                    mapC = false;
                    mapO = false;
                    mapF = false;
                }

                if (!editAdded && editPolicies.Count > 0) //If in edit mode
                {
                    for (int i = 0; i < editPolicies.Count; i++)
                    {
                        if (editPolicies[i].name == "color")
                        {
                            AddPolicyColor();
                        }
                        if (editPolicies[i].name == "orientation")
                        {
                            AddPolicyOrientation();
                        }
                        if (editPolicies[i].name == "fill")
                        {
                            AddPolicyFill();
                        }
                    }
                    editAdded = true;
                }
            }
        }
    }

    /// <summary>
    /// Add a single color prefab at spot i 
    /// </summary>
    /// <param name="i"></param>
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

        Dropdown varD = policyPrefab.transform.Find("DropdownVar").GetComponent<Dropdown>();
        varD.ClearOptions();
        varD.AddOptions(var_DropOptions);
        if (selectedVarsC.Count > 0)
        {
            varD.value = selectedVarsC[0];
        }
        if (editPolicies.Count > 0)
        {
            for (int j = 0; j < editPolicies.Count; j++)
            {
                if (editPolicies[j].name == "color")
                {
                    string v = editPolicies[j].variableName;
                    varD.value = getiValueFromList(v, var_DropOptions);
                }
            }
        }

        Button remv = policyPrefab.transform.Find("rmvButton").GetComponent<Button>();
        int x = i;
        remv.onClick.AddListener(() => { mapColorPolicy = new GameObject(); mapC = true; UIManager.Instance.Options--; addColorButton.interactable = true; getSelectedVarsO(x); getSelectedVarsF(x); });
        mapColorPolicy = policyPrefab;
    }

    /// <summary>
    /// Add a single orientation prefab at spot i 
    /// </summary>
    /// <param name="i"></param>
    private void addOneOrientationPrefab(int i)
    {
        GameObject policyPrefab = (GameObject)Instantiate(Resources.Load("UI/MapOrientationPolicyPrefab"), optionPanel.transform);
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
        if (selectedVarsO.Count > 0)
        {
            varD.value = selectedVarsO[0];
        }
        if (editPolicies.Count > 0)
        {
            for (int j = 0; j < editPolicies.Count; j++)
            {
                if (editPolicies[j].name == "orientation")
                {
                    string v = editPolicies[j].variableName;
                    varD.value = getiValueFromList(v, var_DropOptions);
                }
            }
        }

        Button remv = policyPrefab.transform.Find("rmvButton").GetComponent<Button>();
        int x = i;
        remv.onClick.AddListener(() => { mapOrientPolicy = new GameObject(); mapO = true; UIManager.Instance.Options--; addOrientationButton.interactable = true; getSelectedVarsC(x); getSelectedVarsF(x); });
        mapOrientPolicy = policyPrefab;
    }

    /// <summary>
    /// Add a single fill prefab at spot i 
    /// </summary>
    /// <param name="i"></param>
    private void addOneFillPrefab(int i)
    {
        GameObject policyPrefab = (GameObject)Instantiate(Resources.Load("UI/MapFillPolicyPrefab"), optionPanel.transform);
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
        if (editPolicies.Count > 0)
        {
            for (int j = 0; j < editPolicies.Count; j++)
            {
                if (editPolicies[j].name == "fill")
                {
                    string v = editPolicies[j].variableName;
                    varD.value = getiValueFromList(v, var_DropOptions);
                }
            }
        }

        Button remv = policyPrefab.transform.Find("rmvButton").GetComponent<Button>();
        int x = i;
        remv.onClick.AddListener(() => { mapFillPolicy = new GameObject(); mapF = true; UIManager.Instance.Options--; addFillButton.interactable = true; getSelectedVarsC(x); getSelectedVarsO(x); });

        mapFillPolicy = policyPrefab;
    }


    /// <summary>
    /// Get the vars from the color prefab at spot I
    /// </summary>
    /// <param name="i"></param>
    private void getSelectedVarsC(int i)
    {
        selectedVarsC.Clear();

        GameObject g = mapColorPolicy;
        if (g.transform.childCount > 0)
        {
            Dropdown varD = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
            int valueD = varD.value;
            selectedVarsC.Add(valueD);
        }

    }

    /// <summary>
    /// Get the vars from the orientation prefab at spot I
    /// </summary>
    /// <param name="i"></param>
    private void getSelectedVarsO(int i)
    {
        selectedVarsO.Clear();
        if (i == 1) { i = 0; }
        else if (i == 0) { i = 1; }
        GameObject g = mapOrientPolicy;
        if (g.transform.childCount > 0)
        {
            //Toggle orient = g.transform.Find("OrientToggle").GetComponent<Toggle>();
            //toggleClicked = orient.isOn;
            Dropdown varD = g.transform.Find("DropdownVar").GetComponent<Dropdown>();
            int valueD = varD.value;
            selectedVarsO.Add(valueD);
        }
    }

    /// <summary>
    /// Get the vars from the fill prefab at spot I
    /// </summary>
    /// <param name="i"></param>
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

    /// <summary>
    /// Added to Add Color Policy button, will add a policy at the bottom of the list
    /// </summary>
    private void AddPolicyColor()
    {

        int currOpps = allPolicies.Count;
        int totalOptions = UIManager.Instance.TotalOptions;
        if (totalOptions > currOpps)
        {
            addOneColorPrefab(currOpps);
            UIManager.Instance.Options++;
            addColorButton.interactable = false;
        }
    }

    /// <summary>
    /// Added to Add orientation Policy button, will add a policy at the bottom of the list
    /// </summary>
    private void AddPolicyOrientation()
    {

        int currOpps = allPolicies.Count;
        int totalOptions = UIManager.Instance.TotalOptions;
        if (totalOptions > currOpps)
        {
            addOneOrientationPrefab(currOpps);
            UIManager.Instance.Options++;
            addOrientationButton.interactable = false;
        }
    }

    /// <summary>
    /// Added to Add fill Policy button, will add a policy at the bottom of the list
    /// </summary>
    private void AddPolicyFill()
    {
        int currOpps = allPolicies.Count;
        int totalOptions = UIManager.Instance.TotalOptions;
        if (totalOptions > currOpps)
        {
            addOneFillPrefab(currOpps);
            UIManager.Instance.Options++;
            addFillButton.interactable = false;
        }
    }

    /// <summary>
    /// Sends the selected policies to the UIManagers as well as the defaults
    /// Also signals to add the graph
    /// </summary>
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
        UIManager.Instance.sentShape = GetShape(defaultShapeString);
        UIManager.Instance.addGraph = true;
    }

    /// <summary>
    /// Get the Inidicator shape from the string
    /// </summary>
    /// <param name="d"> string from the default dropdown</param>
    /// <returns></returns>
    public IndicatorShape GetShape(string d)
    {
        if (d == "Check") { return IndicatorShape.Check; }
        else if (d == "Circle") { return IndicatorShape.Circle; }
        else if (d == "!") { return IndicatorShape.Exclamation; }
        else if (d == "Plus") { return IndicatorShape.Plus; }
        else if (d == "Triangle") { return IndicatorShape.Triangle; }
        else if (d == "Square") { return IndicatorShape.Square; }
        else if (d == "Arrow") { return IndicatorShape.Arrow; }
        else { return IndicatorShape.Check; }
    }

    /// <summary>
    /// With a string and a list, get the position value. Used for the dropdowns
    /// </summary>
    /// <param name="var">string </param>
    /// <param name="s">string list</param>
    /// <returns>int value from the list</returns>
    int getiValueFromList(string var, List<string> s)
    {
        int val = 0;
        for (int i = 0; i < s.Count; i++)
        {
            if (s[i] == var)
            {
                val = i;
            }
        }
        return val;
    }

    /// <summary>
    /// get string of color from the Color.
    /// </summary>
    /// <param name="c"></param>
    /// <returns>string of a color</returns>
    string getColorString(Color c)
    {
        string color = "";

        if (c == Color.red) { return "Red"; }
        else if (c == Color.blue) { return "Blue"; }
        else if (c == Color.green) { return "Green"; }
        else if (c == Color.yellow) { return "Yellow"; }
        else if (c == Color.magenta) { return "Magenta"; }
        else if (c == Color.black) { return "Black"; }
        else if (c == Color.white) { return "White"; }
        return color;
    }

}