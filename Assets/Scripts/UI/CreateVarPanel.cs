using System.Collections;
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
    int namei = 1;
    public Button addVarButton;
    public Button rmvVarButton;
    public Button sendVarsButton;

    public GameObject overallPanel;


    void Start()
    {   // this.data.Keys.ToArray();        
        GameObject dropdownPrefab = (GameObject)Instantiate(Resources.Load("VarDropDownPanel"), transform); 
        dropdownPrefab.transform.SetParent(varPanel.transform, false);
        RectTransform t = dropdownPrefab.GetComponent<RectTransform>();
        t.sizeDelta = new Vector2(0, 50f);
        t.anchorMax = new Vector2(0f, 1f);
        t.anchorMin = new Vector2(0f, 1f);
        t.anchoredPosition = new Vector2(1f, initpos);
        t.pivot = new Vector2(.5f, .5f);
        t.localScale = Vector3.one;
        t.localRotation = new Quaternion(0, 0, 0, 0);

        Dropdown td = dropdownPrefab.transform.Find("DropdownVar1").GetComponent<Dropdown>();
        td.ClearOptions();
        td.AddOptions(m_DropOptions);
  
        Robot r1 = DataManager.Instance.GetRobot("RobotTarget1");
        allDropdownObjects.Add(td);
        int options = UIManager.Instance.Options;

        addVarButton.onClick.AddListener(AddVar);
        rmvVarButton.onClick.AddListener(RemoveVar);
        sendVarsButton.onClick.AddListener(sendVars);

        if (options > 1) {
            for (int i = 0; i < options - 1 ; i++) {
                GameObject dropdownPrefab2 = (GameObject)Instantiate(Resources.Load("VarDropDownPanel"), varPanel.transform);
                //dropdownPrefab2.transform.position = new Vector3(dropdownPrefab.transform.position.x, (dropdownPrefab.transform.position.y + offset), dropdownPrefab.transform.position.z);
                dropdownPrefab2.transform.SetParent(varPanel.transform, false);
                RectTransform dropTransform = dropdownPrefab2.GetComponent<RectTransform>();
                dropTransform.sizeDelta = new Vector2(0, 50f);
                dropTransform.anchorMax = new Vector2(0f, 1f);
                dropTransform.anchorMin = new Vector2(0f, 1f);
                dropTransform.anchoredPosition = new Vector2(1f, initpos+offset);
                dropTransform.pivot = new Vector2(.5f, .5f);
                dropTransform.localScale = Vector3.one;
                dropTransform.localRotation = new Quaternion(0, 0, 0, 0);

                Dropdown d2 = dropdownPrefab2.transform.Find("DropdownVar1").GetComponent<Dropdown>();
                d2.ClearOptions();
                d2.AddOptions(m_DropOptions);
                Text t2 = dropdownPrefab2.transform.Find("TextV1").GetComponent<Text>();
                t2.text = "Variable " + (i + 2);

                offset = offset + offset;
                totalDropdowns++;
                allDropdownObjects.Add(d2);
                namei = i+2;

            }

            currOptions = options;
        }
    }

    // Update is called once per frame
    void Update()
    { if (overallPanel != null)
        {
            if (overallPanel.activeSelf)
            {
                int options = UIManager.Instance.Options;
                if (totalDropdowns > options || options > totalDropdowns)
                {
                    foreach (Transform child in varPanel.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                    offset = 0f;
                    totalDropdowns = 0;
                    allDropdownObjects.Clear();
                    if (options > 0)
                    {
                        for (int i = 0; i < options; i++)
                        {
                            GameObject dropdownPrefab2 = (GameObject)Instantiate(Resources.Load("VarDropDownPanel"), varPanel.transform);
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
                            Text t2 = dropdownPrefab2.transform.Find("TextV1").GetComponent<Text>();
                            t2.text = "Variable " + (i + 1);
                            offset = offset + -50f;
                            totalDropdowns++;
                            allDropdownObjects.Add(d2);
                            namei = i;
                        }

                        currOptions = options;
                    }
                }
            }
        }


    }

    void AddVar()
    {
        GameObject dropdownPrefab2 = (GameObject)Instantiate(Resources.Load("VarDropDownPanel"), varPanel.transform);
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
        Text t2 = dropdownPrefab2.transform.Find("TextV1").GetComponent<Text>();
        t2.text = "Variable " + (namei + 1);
        offset = offset + -50f;
        totalDropdowns++;
        allDropdownObjects.Add(d2);
        UIManager.Instance.Options++;
        namei++;
    }

    void RemoveVar()
    {
        int total = totalDropdowns - 1;
        foreach (Transform child in varPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        offset = 0f;
        totalDropdowns = 0;
        allDropdownObjects.Clear();
        UIManager.Instance.Options = total ;


        for (int i = 0; i < total; i++)
        {
            GameObject dropdownPrefab2 = (GameObject)Instantiate(Resources.Load("VarDropDownPanel"), varPanel.transform);
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
            Text t2 = dropdownPrefab2.transform.Find("TextV1").GetComponent<Text>();
            t2.text = "Variable " + (i + 1);
            offset = offset + -50f;
            totalDropdowns++;
            allDropdownObjects.Add(d2);
            namei = i + 1;

        }
    }
    public List<string> var = new List<string>();

    void sendVars()
    {
        foreach (Dropdown d in allDropdownObjects){
            List<Dropdown.OptionData> list = d.options;
            int value = d.value;
            var.Add(list[d.value].text);
        }
        Debug.Log(var);
        UIManager.Instance.wantedVars = var;
    }


}
