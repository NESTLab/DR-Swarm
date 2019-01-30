using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

// Anything and everything can be changed. Comments can be removed,
// they're just here to explain everything as best I can

// TODO: make another pie chart class for one robot, many variables

public class PieChart : IVisualization
{
    // Feel free to use any data type to store the intermittent data
    IObservable<Dictionary<Robot, float>> dataSource;
    HashSet<Robot> robotList;
    HashSet<string> varSet;

    public PieChart(string variableName, Robot firstRobot, Robot secondRobot, params Robot[] robots)
    {
        // TODO: Jerry needs to rename this to robotSet
        robotList = new HashSet<Robot>(robots);
        robotList.Add(firstRobot);
        robotList.Add(secondRobot);

        varSet = new HashSet<string>();
        varSet.Add(variableName);

        // Create a single data source observable by creating
        // an observable for each robot, and then combining them
        // (this is what select many does)
        dataSource = robotList.ToObservable().SelectMany(r =>
        {
            // get the observable variable from the robot
            // then transform the values from the observable
            // into a Dictionary<Robot, float>
            return r.GetObservableVariable<float>(variableName).Select(v => new Dictionary<Robot, float> { { r, v } });
        });
    }

    public ISet<Robot> GetRobots()
    {
        return robotList;
    }

    public ISet<string> GetVariables()
    {
        //throw new NotImplementedException();
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
        // Take the Dictionary<Robot, float> and transform it
        // into a Dictionary<Robot, Dictionary<string, float>>
        // This is a dictionary that maps robots to a dict
        // which maps variable name (string) to value (float)

        throw new NotImplementedException();
    }
}
