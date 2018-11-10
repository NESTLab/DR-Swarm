using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableDict {

    Dictionary<string, object> variables;

    public VariableDict()
    {
        variables = new Dictionary<string, object>();
    }

    public object Get(string name)
    {
        if (Has(name)) return variables[name];
        return null;
    }

    public void Set(string name, object value)
    {
        variables[name] = value;
    }

    public bool Has(string name)
    {
        return variables.ContainsKey(name);
    }

    public List<string> GetVariables<T>()
    {
        List<string> varsOfType = new List<string>();
        foreach (string name in variables.Keys)
        {
            if (typeof(T) == variables[name].GetType())
            {
                varsOfType.Add(name);
            }
        }

        return varsOfType;
    }
}
