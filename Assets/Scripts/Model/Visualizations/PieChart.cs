using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace DrSwarm.Model.Visualizations
{
    public class PieChart : IVisualization
    {
        // Feel free to use any data type to store the intermittent data
        IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
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
            // into a Dictionary<Robot, Dictionary<string, float>>
            return r.GetObservableVariable<float>(variableName).Select(v =>
                {
                    Dictionary<Robot, Dictionary<string, float>> dict = new Dictionary<Robot, Dictionary<string, float>>();
                    dict.Add(r, new Dictionary<string, float>() { { variableName, v } });
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

            return dataSource;
        }
    }
}