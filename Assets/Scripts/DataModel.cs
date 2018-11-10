using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataModel : MonoBehaviour {

    Dictionary<string, VariableDict> robotVariables;

    public DataModel()
    {
        robotVariables = new Dictionary<string, VariableDict>();
    }

    #region SINGLETON PATTERN
    // Thanks https://answers.unity.com/questions/891380/unity-c-singleton.html
    public static DataModel _instance;
    public static DataModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DataModel>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("DataModel");
                    _instance = container.AddComponent<DataModel>();
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
