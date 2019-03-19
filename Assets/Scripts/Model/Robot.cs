using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Robot
{
    private string _name;
    public string name
    {
        get { return _name;  }
    }

    public Color color;

    private VariableDict data;

    public Robot(string name)
    {
        this._name = name;
        this.color = new Color(155f / 255f, 0, 179f / 255f);
        this.data = new VariableDict();
    }

    public T GetVariable<T>(string name)
    {
        return data.GetValue<T>(name);
    }

    public void SetVariable<T>(string name, T value)
    {
        data.SetValue(name, value);
    }

    public IObservable<T> GetObservableVariable<T>(string name)
    {
        return data.GetObservableValue<T>(name);
    }

    public List<string> GetVariables() { 
        return data.GetVariables();
    } 
}