using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

/// <summary>
/// Class representing the bar graph visualization.
/// </summary>
public class BarGraph : IVisualization
{
    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
    HashSet<Robot> robotSet;
    HashSet<string> varSet;

    /// <summary>
    /// The class constructor.
    /// </summary>
    /// <param name="variables"> The names of the observed data. </param>
    /// <param name="robots"> The robots to attach this visualization to. </param>
    public BarGraph(HashSet<Robot> robots, HashSet<string> variables)
    {
        robotSet = new HashSet<Robot>(robots);
        varSet = new HashSet<string>(variables);

        // TODO: make all other vis classes use this generic alg - line graph still left
        dataSource = robotSet.ToObservable().SelectMany(robot => 
        {
            List<IObservable<Dictionary<string, float>>> variableList = new List<IObservable<Dictionary<string, float>>>();
            foreach (string variable in varSet)
            {
                variableList.Add(robot.GetObservableVariable<float>(variable).Select(v => 
                {
                    return new Dictionary<string, float>() { { variable, v } };
                }));
            }

            return Observable.CombineLatest(variableList).Select(varList => 
            {
                Dictionary<Robot, Dictionary<string, float>> dict = new Dictionary<Robot, Dictionary<string, float>>();
                foreach (Dictionary<string, float> varDict in varList)
                {
                    if (!dict.ContainsKey(robot))
                    {
                        dict[robot] = new Dictionary<string, float>();
                    }

                    foreach (string key in varDict.Keys)
                    {
                        dict[robot][key] = varDict[key];
                    }
                }

                return dict;
            });
        });
    }

    /// <summary>
    /// See <see cref="IVisualization.GetRobots"/>
    /// </summary>
    public ISet<Robot> GetRobots()
    {
        return robotSet;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetVariables"/>
    /// </summary>
    public ISet<string> GetVariables()
    {
        return varSet;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetNumDataSources"/>
    /// </summary>
    public ParameterCount GetNumDataSources()
    {
        return ParameterCount.One;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetNumRobots"/>
    /// </summary>
    public ParameterCount GetNumRobots()
    {
        return ParameterCount.N;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetObservableData"/>
    /// </summary>
    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData()
    {
        return dataSource;
    }
}
