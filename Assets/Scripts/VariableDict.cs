using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class VariableDict {

    Dictionary<string, BehaviorSubject<object>> variables;

    public VariableDict()
    {
        variables = new Dictionary<string, BehaviorSubject<object>>();
    }

    public object GetValue(string name)
    {
        if (Has(name)) return variables[name].Value;
        return null;
    }

    public IObservable<object> GetObservableValue(string name)
    {
        if (!Has(name))
            variables[name] = new BehaviorSubject<object>(null);

        return variables[name];
    }

    public void SetValue(string name, object value)
    {
        if (Has(name))
        {
            variables[name].OnNext(value);
        } else
        {
            variables[name] = new BehaviorSubject<object>(value);
        }
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
