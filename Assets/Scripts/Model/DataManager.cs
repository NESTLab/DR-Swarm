using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    Dictionary<string, VariableDict> robotVariables;

    public DataManager()
    {
        robotVariables = new Dictionary<string, VariableDict>();
    }

    #region SINGLETON PATTERN
    // Thanks https://answers.unity.com/questions/891380/unity-c-singleton.html
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DataManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("DataModel");
                    _instance = container.AddComponent<DataManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    public VariableDict GetRobotDict(string robotName)
    {
        if (!robotVariables.ContainsKey(robotName))
        {
            robotVariables[robotName] = new VariableDict();
        }

        return robotVariables[robotName];
    }

    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
	void Update () {
    }
}
