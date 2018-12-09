﻿using System;
using UnityEngine;

public class Robot
{
    private string name;
    private VariableDict data;

    private Color _color;
    public Color color
    {
        get
        {
            return _color;
        }
    }

    public Robot(string name)
    {
        this.name = name;
        this.data = new VariableDict();
        this._color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
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