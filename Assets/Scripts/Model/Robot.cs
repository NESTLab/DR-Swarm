using System;
using UnityEngine;

public class Robot
{
    private string _name;
    public string name
    {
        get { return _name;  }
    }

    private Color _color;
    public Color color
    {
        get
        {
            return _color;
        }
    }

    private VariableDict data;

    public Robot(string name)
    {
        this._name = name;
        this._color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
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
}