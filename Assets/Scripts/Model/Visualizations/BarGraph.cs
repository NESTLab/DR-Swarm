using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

// TODO: Implement this class.
// Anything and everything can be changed. Comments can be removed,
// they're just here to explain everything as best I can
public class BarGraph : IVisualization
{
    // Feel free to use any data type to store the intermittent data
    IObservable<Dictionary<Robot, float>> dataSource;
    HashSet<Robot> robotList;

    public BarGraph(string variableName, Robot firstRobot, params Robot[] robots)
    {
        robotList = new HashSet<Robot>(robots);
        robotList.Add(firstRobot);

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
        throw new NotImplementedException();
    }

    public ParameterCount GetNumDataSources()
    {
        return ParameterCount.One;
    }

    public ParameterCount GetNumRobots()
    {
        return ParameterCount.One;
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
