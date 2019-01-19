using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Operators;

// TODO: Implement this class.
// Anything and everything can be changed. Comments can be removed,
// they're just here to explain everything as best I can

// TODO: make another pie chart class for one robot, many variables

public class PieChartMultiVar : IVisualization
{
    // Feel free to use any data type to store the intermittent data
    IObservable<Dictionary<Robot, List<float>>> dataSource;
    HashSet<Robot> robotList;
    HashSet<string> varSet;

    public PieChartMultiVar(Robot robot, string variableOne, params string[] variables)
    {
        // TODO: Jerry needs to rename this to robotSet
        robotList = new HashSet<Robot>();
        robotList.Add(robot);

        varSet = new HashSet<string>(variables);
        varSet.Add(variableOne);

        List<IObservable<float>> observableVarList = new List<IObservable<float>>();
        foreach (string var in varSet) {
            observableVarList.Add(robot.GetObservableVariable<float>(var));
        }

        // Create a single data source observable by creating
        // an observable for each robot, and then combining them
        // (this is what select many does)
        
        // TODO: Jerry refactor for IList
        dataSource = Observable.CombineLatest(observableVarList).Select(list => new Dictionary<Robot, List<float>> { { robot, new List<float>(list) } });
    }

    public ISet<Robot> GetRobots()
    {
        return robotList;
    }

    public ParameterCount GetNumDataSources()
    {
        // TODO: make a two or more option
        return ParameterCount.N;
    }

    public ParameterCount GetNumRobots()
    {
        return ParameterCount.One;
    }

    public IObservable<Dictionary<Robot, List<float>>> GetObservableData()
    {
        // Take the Dictionary<Robot, float> and transform it
        // into a Dictionary<Robot, List<float>> by taking the
        // current value for each robot and adding it to each
        // robot's own list

        // This encodes the data source into a dictionary containing
        // one or more values per robot
        return dataSource;
    }
}
