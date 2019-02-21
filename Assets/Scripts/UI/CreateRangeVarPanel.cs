using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using shapeNamespace;


public class CreateRangeVarPanel : MonoBehaviour
{
    public GameObject optionPanel; //Parent Panel, set when adding script
    public GameObject overallPanel; //Panel for the entire screen
    float initpos = -42f; //Position offset for the prefabs
    public List<GameObject> allPolicies = new List<GameObject>();//All Visualizations, IViz
    public int totalPolicies = 0; // Total prefabs the script needs to add
    float offset = 0f; // Offset for when a prefab gets added
    float offsetAddition = -50f;//Value to add to the offset after each policy is added
    private List<string> colors = new List<string> { "Default", "Red", "Blue", "Green", "Yellow", "Orange", "Pink", "Purple", "White", "Black" }; // How the User sees the colors, will be converted to hex values when sending
    public List<string> shapes = new List<string> { "Default", "Check", "Circle", "!", "Plus", "Square", "Triangle" };//List of shape options
    public List<string> shapesD = new List<string> { "Check", "Circle", "!", "Plus", "Square", "Triangle" };//List of shape options
    public List<string> colorsD = new List<string> { "Red", "Blue", "Green", "Yellow", "Orange", "Pink", "Purple", "White", "Black" }; // How the User sees the colors, will be converted to hex values when sending

    public List<int> selectedColors = new List<int>();
    public List<int> selectedShapes = new List<int>();
    public List<string> selectedMin = new List<string>();
    public List<string> selectedMax = new List<string>();

    List<string> var_DropOptions = new List<string>();
    public Button addVarButton;
    public Button sendVarsButton;
    public Dropdown variableDrop;
    public Dropdown defaultColor;
    public Dropdown defaultShape;


    // Start is called before the first frame update
    void Start()
    {
        addVarButton.onClick.AddListener(AddPolicy);
        sendVarsButton.onClick.AddListener(sendPolicies);
        int options = UIManager.Instance.Options;
        if (optionPanel != null)
        {
            UIManager.Instance.updateTotalVars();
            var_DropOptions = UIManager.Instance.wantedVars;
            variableDrop.ClearOptions();
            variableDrop.AddOptions(var_DropOptions);
            defaultColor.ClearOptions();
            defaultColor.AddOptions(colorsD);
            defaultShape.ClearOptions();
            defaultShape.AddOptions(shapesD);

            for (int i = 0; i < options; i++)
            {
                addOnePrefab(i);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (overallPanel != null && overallPanel.activeSelf)
        {
            int options = UIManager.Instance.Options;
            if (allPolicies.Count > options || options > allPolicies.Count)
            {
                if (optionPanel != null)
                {
                    UIManager.Instance.updateTotalVars();
                    var_DropOptions = UIManager.Instance.wantedVars;
                    variableDrop.ClearOptions();
                    variableDrop.AddOptions(var_DropOptions);
                    defaultColor.ClearOptions();
                    defaultColor.AddOptions(colorsD);
                    defaultShape.ClearOptions();
                    defaultShape.AddOptions(shapesD);

                    foreach (Transform child in optionPanel.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                    allPolicies.Clear();
                    offset = 0f;
                    UIManager.Instance.updateTotalVars();
                    var_DropOptions = UIManager.Instance.wantedVars;

                    for (int i = 0; i < options; i++)
                    {
                        addOnePrefab(i);
                    }
                }
            }
        }
    }

    //Add a single prefab at spot i 
    private void addOnePrefab(int i)
    {
        GameObject policyPrefab = (GameObject)Instantiate(Resources.Load("RangePolicyPrefab"), optionPanel.transform);
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

        InputField min = policyPrefab.transform.Find("InputMin").GetComponent<InputField>();
        InputField max = policyPrefab.transform.Find("InputMax").GetComponent<InputField>();
        Dropdown color = policyPrefab.transform.Find("DropdownColor").GetComponent<Dropdown>();
        Dropdown shape = policyPrefab.transform.Find("DropdownShape").GetComponent<Dropdown>();
        color.ClearOptions();
        color.AddOptions(colors);
        shape.ClearOptions();
        shape.AddOptions(shapes);
        if (selectedColors.Count > 0)
        {
            color.value = selectedColors[i];
            shape.value = selectedShapes[i];
            min.text = selectedMin[i]; //What if no text was added?
            max.text = selectedMax[i];
        }

        Button remv = policyPrefab.transform.Find("rmvButton").GetComponent<Button>();
        int x = i;
        remv.onClick.AddListener(() => { getSelectedVars(x); Destroy(policyPrefab); UIManager.Instance.Options--; });
    }

    //get current selected vars to remove the prefab at spot i
    private void getSelectedVars(int i)
    {
        selectedColors.Clear();
        selectedShapes.Clear();
        selectedMin.Clear();
        selectedMax.Clear();
        foreach (GameObject g in allPolicies)
        {
            Dropdown color = g.transform.Find("DropdownColor").GetComponent<Dropdown>();
            Dropdown shape = g.transform.Find("DropdownShape").GetComponent<Dropdown>();
            int valueC = color.value;
            int valueS = shape.value;
            selectedColors.Add(valueC);
            selectedShapes.Add(valueS);
            InputField min = g.transform.Find("InputMin").GetComponent<InputField>();
            InputField max = g.transform.Find("InputMax").GetComponent<InputField>();
            selectedMin.Add(min.text);
            selectedMax.Add(max.text);

        }
        //Debug.Log("Removing " + i + " which is " + selectedMin[i]);
        selectedColors.RemoveAt(i);
        selectedShapes.RemoveAt(i);
        selectedMax.RemoveAt(i);
        selectedMin.RemoveAt(i);

    }

    //Added to Add Policy button, will add a policy at the bottom of the list
    private void AddPolicy()
    {
        if (offset == 0)
        {
            offset = offsetAddition;
        }
        int currOpps = allPolicies.Count;
        int totalOptions = UIManager.Instance.TotalOptions;
        if (totalOptions > currOpps)
        {
            selectedColors.Clear();
            selectedShapes.Clear();
            selectedMin.Clear();
            selectedMax.Clear();
            addOnePrefab(currOpps);
            UIManager.Instance.Options++;
        }
    }

    //Send policies to UIManager
    public void sendPolicies()
    {
        List<string> colorsSelected = new List<string>();
        List<string> shapesSelected = new List<string>();
        List<string> maxSelected = new List<string>();
        List<string> minSelected = new List<string>();
        List<RangePolicy> policies = new List<RangePolicy>();
        string defaultShapeString = defaultShape.options[defaultShape.value].text;
        string defaultColorString = defaultColor.options[defaultColor.value].text; //Color.Red 
        string variableString = variableDrop.options[variableDrop.value].text;
        int i = 0;
        Color defaultColored = Color.white;
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

            string name = "Range" + i;
            float minnum = (float.Parse(min.text));
            float maxnum = float.Parse(max.text);
            RangePolicy policy = new RangePolicy(name, minnum, maxnum);
            if (ColorUtility.TryParseHtmlString(defaultColorString, out defaultColored)) { }
            else { defaultColored = Color.red; }
            policy.color = GetColor(listC[color.value].text, defaultColored);
            IndicatorShape sh = GetShape(listS[shape.value].text, defaultShapeString);
            policy.shape = sh;
            policies.Add(policy);
            i++;

            float x = float.Parse(max.text); 
            //Debug.Log(i+ "   " + policy.color +  policy.shape + "nums" + (float.Parse(min.text)) + " "+ x );
            

        }
        if (ColorUtility.TryParseHtmlString(defaultColorString, out defaultColored)) { }
        else { defaultColored = Color.red; }
        //Debug.Log("COLOR " + defaultColored);
        UIManager.Instance.wantedVars = new List<string> { variableString };
        UIManager.Instance.allRPolicies = policies;
        UIManager.Instance.sentColor = defaultColored;
        UIManager.Instance.sentShape = GetShape("default", defaultShapeString) ;
        //Debug.Log("Shape d" + GetShape("default", defaultShapeString));
        UIManager.Instance.addGraph = true;

    }

    public Color GetColor(string c, Color d)
    {
        if (c == "Default") { return d; }
        else {
            Color newCol;
            if (ColorUtility.TryParseHtmlString(c, out newCol))
                return newCol;
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
        else
        {
            if (d == "Check") { return IndicatorShape.Check; }
            else if (d == "Circle") { return IndicatorShape.Circle; }
            else if (d == "!") { return IndicatorShape.Exclamation; }
            else if (d == "Plus") { return IndicatorShape.Plus; }
            else if (d == "Triangle") { return IndicatorShape.Triangle; }
            else if (d == "Square") { return IndicatorShape.Square; }
        }
        return IndicatorShape.Check;
    }
    

}


