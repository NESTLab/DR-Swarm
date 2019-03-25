using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class Robot
{
    private string _name;
    /// <summary>
    /// Name of the robot, readonly.
    /// </summary>
    public string name
    {
        get { return _name;  }
    }

    /// <summary>
    /// The color associated with the robot
    /// </summary>
    public Color color;

    /// <summary>
    /// A VariableDict which holds the observable data for robot
    /// </summary>
    /// <seealso cref="VariableDict"/>
    private VariableDict data;

    /// <summary>
    /// The constructor for Robot
    /// </summary>
    /// <param name="name">The name of the robot</param>
    public Robot(string name)
    {
        this._name = name;
        this.color = new Color(155f / 255f, 0, 179f / 255f);
        this.data = new VariableDict();
    }

    /// <summary>
    /// Calls <see cref="VariableDict.GetValue{T}(string)"/>
    /// </summary>
    public T GetVariable<T>(string name)
    {
        return data.GetValue<T>(name);
    }

    /// <summary>
    /// Calls <see cref="VariableDict.SetValue{T}(string, T)"/>
    /// </summary>
    public void SetVariable<T>(string name, T value)
    {
        data.SetValue(name, value);
    }

    /// <summary>
    /// Calls <see cref="VariableDict.GetObservableValue{T}(string)"/>
    /// </summary>
    public IObservable<T> GetObservableVariable<T>(string name)
    {
        return data.GetObservableValue<T>(name);
    }

    /// <summary>
    /// Calls <see cref="VariableDict.GetVariables"/>
    /// </summary>
    public List<string> GetVariables() { 
        return data.GetVariables();
    } 
}