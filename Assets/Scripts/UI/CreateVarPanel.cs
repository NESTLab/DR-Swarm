using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Class that creates the var panel and that behaviour.
/// </summary>
public class CreateVarPanel : MonoBehaviour
{
    List<string> m_DropOptions = new List<string> { "No Variables Found" };
    private int currOptions = 1;
    float offset = -50f; //offest per var added
    float initpos = -30f;
    int totalDropdowns = 1; //number of dropdowns added
    public List<Dropdown> allDropdownObjects = new List<Dropdown>(); //list of all dropdowns from the prefabs
    public int namei = 1; //used for naming conventions

    public List<int> selectedVars = new List<int>();  //list of selected vars for updating panel
    public List<string> editvars = new List<string>(); //list of vars that were from the edit viz
    public List<string> var = new List<string>(); //vars to send

    bool edit = false;  //if its editting a viz

    //game objects
    public Button addVarButton;
    public Button sendVarsButton;
    public GameObject varPanel; //panel that holds the prefabs
    public GameObject overallPanel; //panel that contains the var panel

    /// <summary>
    /// Start is called before the first frame update.
    /// Initializes many vars
    /// </summary>
    void Start()
    {
        UIManager.Instance.updateTotalVars();
        m_DropOptions = UIManager.Instance.wantedVars;
        int options = UIManager.Instance.Options;
        if (UIManager.Instance.EOptions > 0)
        {
            options = UIManager.Instance.EOptions;
            UIManager.Instance.Options = options;
        }
        addVarButton.onClick.AddListener(AddVar);
        sendVarsButton.onClick.AddListener(sendVars);
        offset = 0f;
        editvars = UIManager.Instance.editVars;
        for (int i = 0; i < options; i++)
        {
            addVarPrefab(i);
        }
        currOptions = options;
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        if (overallPanel != null && varPanel != null) //Check if the panel is active
        {
            if (overallPanel.activeSelf)
            {
                int options = UIManager.Instance.Options;
                if (UIManager.Instance.EOptions > 0) //checks if were going to be editing
                {
                    options = UIManager.Instance.EOptions;
                    UIManager.Instance.Options = options;
                }
                if (totalDropdowns > options || options > totalDropdowns) //need to update the panel
                {
                    editvars = UIManager.Instance.editVars;
                    UIManager.Instance.updateTotalVars();
                    m_DropOptions = UIManager.Instance.wantedVars;
                    foreach (Transform child in varPanel.transform) //remove the child objects in the panel
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                    //reset some vars
                    offset = 0f;
                    totalDropdowns = 0;
                    allDropdownObjects.Clear();
                    editvars = UIManager.Instance.editVars;
                    if (options > 0)
                    {
                        for (int i = 0; i < options; i++) //add prefabs
                        {
                            addVarPrefab(i);
                        }
                        currOptions = options;
                        if (editvars.Count > 0)
                        {
                            UIManager.Instance.EOptions = 0;
                            UIManager.Instance.editVars = new List<string>();
                        }
                        edit = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Add one var prefab.
    /// </summary>
    /// <param name="i"> number in the list itll be </param>
    void addVarPrefab(int i)
    {
        //Setup the prefab
        GameObject dropdownPrefab2 = (GameObject)Instantiate(Resources.Load("UI/VarDropDownPanel"), varPanel.transform);
        dropdownPrefab2.transform.SetParent(varPanel.transform, false);
        RectTransform dropTransform = dropdownPrefab2.GetComponent<RectTransform>();
        dropTransform.sizeDelta = new Vector2(0, 50f);
        dropTransform.anchorMax = new Vector2(0f, 1f);
        dropTransform.anchorMin = new Vector2(0f, 1f);
        dropTransform.anchoredPosition = new Vector2(1f, initpos + offset);
        dropTransform.pivot = new Vector2(.5f, .5f);
        dropTransform.localScale = Vector3.one;
        dropTransform.localRotation = new Quaternion(0, 0, 0, 0);
        //setup dropdown
        Dropdown d2 = dropdownPrefab2.transform.Find("DropdownVar1").GetComponent<Dropdown>();
        d2.ClearOptions();
        d2.AddOptions(m_DropOptions);
        if (selectedVars.Count > 0)
        {
            d2.value = selectedVars[i];
        }
        if (editvars.Count > 0 && edit)
        {
            int v = getValueFromVars(editvars[i]);
            d2.value = v;
        }
        //setup up text and button
        Text t2 = dropdownPrefab2.transform.Find("TextV1").GetComponent<Text>();
        t2.text = "Variable " + (i + 1);
        Button remv = dropdownPrefab2.transform.Find("rmvButton").GetComponent<Button>();
        int x = i;
        remv.onClick.AddListener(() => { getSelectedVars(x); Destroy(dropdownPrefab2); UIManager.Instance.Options--; });
        //update vars
        offset = offset + -50f;
        totalDropdowns++;
        allDropdownObjects.Add(d2);
        namei = i;
    }

    /// <summary>
    /// Get vars from a dropdown .
    /// </summary>
    /// <param name="i">spot i in the list</param>
    void getSelectedVars(int i)
    {
        selectedVars.Clear();
        foreach (Dropdown d in allDropdownObjects)
        {
            int value = d.value;
            selectedVars.Add(d.value);
        }
        selectedVars.RemoveAt(i);
    }

    /// <summary>
    /// Added to the add var button.
    /// </summary>
    void AddVar()
    {
        edit = false;
        UIManager.Instance.Options++;
        selectedVars.Clear();
    }

    /// <summary>
    /// Send vars from dropdowns to the UIManager.
    /// </summary>
    void sendVars()
    {
        var.Clear();
        foreach (Dropdown d in allDropdownObjects) //get the selected one from the dropdown 
        {
            List<Dropdown.OptionData> list = d.options;
            int value = d.value;
            var.Add(list[d.value].text);
        }
        UIManager.Instance.wantedVars = var;
        UIManager.Instance.addGraph = true;
    }

    /// <summary>
    /// Get the value of the string from list of dropdown options.
    /// </summary>
    /// <param name="var">string to get the value i in the list</param>
    /// <returns>An int that indicated in which part of the dropdowns the string is.</returns>
    int getValueFromVars(string var)
    {
        int val = 0;
        for (int i = 0; i < m_DropOptions.Count; i++)
        {
            if (m_DropOptions[i] == var) { val = i; }
        }
        return val;
    }


}
