using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class PieChart : IVisualization 
{
    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
    HashSet<Robot> robotSet;
    HashSet<string> varSet;

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

    public ISet<Robot> GetRobots() 
    {
        return robotSet;
    }

    public ISet<string> GetVariables() 
    {
        return varSet;
    }

    public ParameterCount GetNumDataSources() 
    {
        return ParameterCount.One;
    }

    public ParameterCount GetNumRobots() 
    {
        // TODO: make a two or more option
        return ParameterCount.N;
    }

    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData() 
    {
        return dataSource;
    }
}
