using System;

public class Robot
{
    private string name;
    private VariableDict data;

    public Robot(string name)
    {
        this.name = name;
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