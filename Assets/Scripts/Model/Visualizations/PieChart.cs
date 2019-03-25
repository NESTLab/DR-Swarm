using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

/// <summary>
/// Class representing the pie chart visualization.
/// </summary>
/// <remarks>
/// This visualization is designed for pie charts that compare data points from a single source across multiple robots.
/// As an example, this visualization can display the amount of food collected by each robot.
/// </remarks>
public class PieChart : IVisualization 
{
    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
    HashSet<Robot> robotSet;
    HashSet<string> varSet;

    /// <summary>
    /// The class contructor.
    /// </summary>
    /// <param name="variableName"> The name of the observed data. </param>
    /// <param name="firstRobot"> A robot to attach to the visualization. </param>
    /// <param name="secondRobot"> Another robot to attach to the visualization. </param>
    /// <param name="robots"> Any other robots to attach to the visualization. </param>
    public PieChart(string variableName, Robot firstRobot, Robot secondRobot, params Robot[] robots) 
    {
        robotSet = new HashSet<Robot>(robots);
        robotSet.Add(firstRobot);
        robotSet.Add(secondRobot);

        varSet = new HashSet<string>();
        varSet.Add(variableName);

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
        return ParameterCount.TwoPlus;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetObservableData"/>
    /// </summary>
    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData() 
    {
        return dataSource;
    }
}
