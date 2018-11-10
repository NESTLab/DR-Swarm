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
        if (robotVariables.ContainsKey(robotName)) return robotVariables[robotName];
        return null;
    }

    // Use this for initialization
    void Start () {
        VariableDict vd = new VariableDict();
        vd.Set("x", 1.0f);
        vd.Set("y", 2.0f);

        robotVariables = new Dictionary<string, VariableDict>();
        robotVariables.Add("1", vd);
	}

    // Update is called once per frame
    float lx = 0, ly = 0;
	void Update () {
        float xval = Input.GetAxis("Horizontal") + 1.0f;
        float yval = Input.GetAxis("Vertical") + 1.0f;

        if (Mathf.Abs(lx - xval) > 0.05) robotVariables["1"].Set("x", xval);
        if (Mathf.Abs(ly - yval) > 0.05) robotVariables["1"].Set("y", yval);

        lx = xval;
        ly = yval;
    }
}
