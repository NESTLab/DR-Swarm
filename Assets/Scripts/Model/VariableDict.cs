using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class VariableDict {

    HashSet<string> variableNames;
    Dictionary<Type, IDictionary> dictionaries;

    public VariableDict()
    {
        variableNames = new HashSet<string>();
        dictionaries = new Dictionary<System.Type, IDictionary>();
    }

    private Dictionary<string, BehaviorSubject<T>> GetDictionary<T>()
    {
        if (!dictionaries.ContainsKey(typeof(T)))
        {
            dictionaries.Add(typeof(T), new Dictionary<string, BehaviorSubject<T>>());
        }

        System.Type type = typeof(T);
        return (Dictionary<string, BehaviorSubject<T>>) dictionaries[type];
    }

    public T GetValue<T>(string name)
    {
        if (Has<T>(name)) return GetDictionary<T>()[name].Value;
        return default(T);
    }

    public IObservable<T> GetObservableValue<T>(string name)
    {
        if (!Has<T>(name))
            SetValue(name, default(T));

        return GetDictionary<T>()[name];
    }

    public void SetValue<T>(string name, T value)
    {
        if (Has<T>(name))
        {
            GetDictionary<T>()[name].OnNext(value);
        } else
        {
            if (!variableNames.Contains(name))
            {
                GetDictionary<T>().Add(name, new BehaviorSubject<T>(value));
                variableNames.Add(name);
            } else
            {
                throw new System.ArgumentException("Parameter already exists as a different type", "name");
            }
        }
    }

    public bool Has<T>(string name)
    {
        return GetDictionary<T>().ContainsKey(name);
    }

    public List<string> GetVariables()
    {
        return new List<string>(variableNames);
    }

    public List<string> GetVariables<T>()
    {
        return new List<string>(GetDictionary<T>().Keys);
    }
}
