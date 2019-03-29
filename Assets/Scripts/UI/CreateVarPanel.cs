using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateVarPanel : MonoBehaviour
{
    List<string> m_DropOptions = new List<string> {"x", "y"};
    public GameObject varPanel;
    // Start is called before the first frame update
    private int currOptions = 1;
    float offset = -50f;
    //public GameObject dropdownPrefab;
    float initpos = -30f;
    int totalDropdowns = 1;
    public List<Dropdown> allDropdownObjects = new List<Dropdown>();
    public int namei = 1;
    public Button addVarButton;
    public Button sendVarsButton;

    public GameObject overallPanel;
    public List<int> selectedVars = new List<int>();
    public List<string> editvars = new List<string>();
    bool edit = false;


    void Start()
    {   
        UIManager.Instance.updateTotalVars();

        m_DropOptions = UIManager.Instance.wantedVars;
       
        int options = UIManager.Instance.Options;
        if (UIManager.Instance.EOptions > 0)
        {
            options = UIManager.Instance.EOptions;
            UIManager.Instance.Options = options;
            //Debug.Log("Update options");
        }

        addVarButton.onClick.AddListener(AddVar);
        sendVarsButton.onClick.AddListener(sendVars);
        offset = 0f;
        editvars = UIManager.Instance.editVars;
        for (int i = 0; i < options; i++)
        {
            addVarPrefab(i);
        }
        //selectedVars.Clear();

        currOptions = options;
    }

    // Update is called once per frame
    void Update()
    { if (overallPanel != null && varPanel != null)
        {
            if (overallPanel.activeSelf)
            {
                int options = UIManager.Instance.Options;
                //Debug.Log("Options " + options);
                if (UIManager.Instance.EOptions > 0)
                {
                    options = UIManager.Instance.EOptions;
                    UIManager.Instance.Options = options;
                    //Debug.Log("Update options");
                }
                if (totalDropdowns > options || options > totalDropdowns)
                {
                    
                    editvars = UIManager.Instance.editVars;
                    UIManager.Instance.updateTotalVars();
                    m_DropOptions = UIManager.Instance.wantedVars;
                    foreach (Transform child in varPanel.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                    offset = 0f;
                    totalDropdowns = 0;
                    allDropdownObjects.Clear();
                    editvars = UIManager.Instance.editVars;
                    if (options > 0)
                    {
                        for (int i = 0; i < options; i++)
                        {
                            addVarPrefab(i);
                        }
                        //selectedVars.Clear();

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

    void addVarPrefab(int i)
    {
        //Debug.Log("In adding");
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
        

        Text t2 = dropdownPrefab2.transform.Find("TextV1").GetComponent<Text>();
        t2.text = "Variable " + (i + 1);
        Button remv = dropdownPrefab2.transform.Find("rmvButton").GetComponent<Button>();
        int x = i;
        remv.onClick.AddListener(() => { getSelectedVars(x); Destroy(dropdownPrefab2); UIManager.Instance.Options--; });


        offset = offset + -50f;
        totalDropdowns++;
        allDropdownObjects.Add(d2);
        namei = i;
    }

    void getSelectedVars(int i)
    {
        selectedVars.Clear();
        foreach (Dropdown d in allDropdownObjects)
        {
            int value = d.value;
            selectedVars.Add(d.value);
        }
        //Debug.Log("Removing " + i);
        //Debug.Log("THE VARS ARE" + selectedVars.Count);

        selectedVars.RemoveAt(i);
    }

    void AddVar()
    {
        Debug.Log("Adding");
        edit = false;
        UIManager.Instance.Options++;
        selectedVars.Clear();

        //if (totalOptions > currOptions)
       // {
         //   addVarPrefab(currOpps);
       // }
    }

    
    public List<string> var = new List<string>();

    void sendVars()
    {
        var.Clear();
        foreach (Dropdown d in allDropdownObjects){
            List<Dropdown.OptionData> list = d.options;
            int value = d.value;
            var.Add(list[d.value].text);
        }
        Debug.Log(var);
        foreach(string s in var)
        {
           // Debug.Log("Adding " + s);
        }
        UIManager.Instance.wantedVars = var;
        UIManager.Instance.addGraph = true;

    }


    int getValueFromVars(string var)
    {
        int val = 0;
        for( int i = 0; i < m_DropOptions.Count; i++)
        {
            if (m_DropOptions[i] == var)
            {
                val = i;
            }
        }
        return val;
    }


}
