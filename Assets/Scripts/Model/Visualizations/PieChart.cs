using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

// TODO: Implement this class.
// Anything and everything can be changed. Comments can be removed,
// they're just here to explain everything as best I can
public class PieChart : IVisualization
{
    // Feel free to use any data type to store the intermittent data
    IObservable<Dictionary<Robot, float>> dataSource;

    public PieChart(string variableName, Robot firstRobot, params Robot[] robots)
    {
        List<Robot> robotList = new List<Robot>(robots);
        robotList.Insert(0, firstRobot);

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

    public ParameterCount getNumDataSources()
    {
        return ParameterCount.One;
    }

    public ParameterCount getNumRobots()
    {
        return ParameterCount.One;
    }

    public IObservable<Dictionary<Robot, List<float>>> getObservableData()
    {
        // Take the Dictionary<Robot, float> and transform it
        // into a Dictionary<Robot, List<float>> by taking the
        // current value for each robot and adding it to each
        // robot's own list

        // This encodes the data source into a dictionary containing
        // one or more values per robot
        return dataSource.Select(dict =>
        {
            Dictionary<Robot, List<float>> dataDict = new Dictionary<Robot, List<float>>();

            foreach (Robot r in dict.Keys)
            {
                dataDict[r] = new List<float> { dict[r] };
            }

            return dataDict;
        });
    }
}
