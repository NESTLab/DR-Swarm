using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

// TODO: Implement this class.

// TODO: implement another class for multivariable?

// Anything and everything can be changed. Comments can be removed,
// they're just here to explain everything as best I can
public class BarGraph : IVisualization
{
    // Feel free to use any data type to store the intermittent data
    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
    HashSet<Robot> robotList;
    HashSet<string> varSet;

    // subscribe like line chart 
    // xaxis, yaxis?
    public BarGraph(Robot firstRobot, HashSet<Robot> robots, string firstVar, HashSet<string> variables) // don't think this can change to include more variables
    {
        // TODO: Jerry needs to rename to robotSet
        robotList = new HashSet<Robot>(robots);
        robotList.Add(firstRobot);
        //robotList.Add(firstRobot);

        varSet = variables;
        varSet.Add(firstVar);
        //varSet.Add(variableName);

        IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
        dataSource = robotList.ToObservable().SelectMany(robot => {
            List<IObservable<Dictionary<string, float>>> variableList = new List<IObservable<Dictionary<string, float>>>();
            foreach (string variable in variables) {
                variableList.Add(robot.GetObservableVariable<float>(variable).Select(v =>
                {
                    return new Dictionary<string, float>() { { variable, v } };
                }));
            }

            return Observable.CombineLatest(variableList).Select(varList => {
                Dictionary<Robot, Dictionary<string, float>> dict = new Dictionary<Robot, Dictionary<string, float>>();
                foreach (Dictionary<string, float> varDict in varList) {
                    if (!dict.ContainsKey(robot)) {
                        dict[robot] = new Dictionary<string, float>();
                    }

                    foreach (string key in varDict.Keys) {
                        dict[robot][key] = varDict[key];
                    }
                }

                return dict;
            });
        });
    }

    public ISet<Robot> GetRobots()
    {
        return robotList;
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
        return ParameterCount.N;
    }

    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData()
    {
        // Take the Dictionary<Robot, float> and transform it
        // into a Dictionary<Robot, Dictionary<string, float>>
        // This is a dictionary that maps robots to a dict
        // which maps variable name (string) to value (float)

        // TODO: 
        // the datasource needs to be a dictionary<Robot, dictionary<string, float>>
        // so we need to loop through each robot and create a dictionary of strings to observable floats
        // then we need to make a dictionary of all of those observables

        return dataSource;
    }
}
