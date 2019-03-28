using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Operators;

namespace DrSwarm.Model.Visualizations
{
    public class PieChartMultiVar : IVisualization
    {
        // Feel free to use any data type to store the intermittent data
        IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
        HashSet<Robot> robotList;
        HashSet<string> varSet;

        //public PieChartMultiVar(Robot robot, string variableOne, params string[] variables)
        public PieChartMultiVar(List<Robot> robots, List<string> variables)
        {
            // TODO: Jerry needs to rename this to robotSet
            robotList = new HashSet<Robot>(robots);
            //robotList.Add(robot);

            varSet = new HashSet<string>(variables);
            //varSet.Add(variableOne);

            /*
            List<IObservable<Dictionary<string, float>>> observableVarList = new List<IObservable<Dictionary<string, float>>>();
            foreach (string var in varSet) {
                observableVarList.Add(robot.GetObservableVariable<float>(var).Select(v => new Dictionary<string, float>() { { var, v } }));
            }

            // Create a single data source observable by creating
            // an observable for each robot, and then combining them
            // (this is what select many does)

            // TODO: Jerry refactor for IList
            dataSource = Observable.CombineLatest(observableVarList).Select(varList => {
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
            */

            dataSource = robotList.ToObservable().SelectMany(r =>
            {
                List<IObservable<Dictionary<string, float>>> variableList = new List<IObservable<Dictionary<string, float>>>();
                foreach (string variable in varSet)
                {
                    variableList.Add(r.GetObservableVariable<float>(variable).Select(v =>
                    {
                        return new Dictionary<string, float>() { { variable, v } };
                    }));
                }

                return Observable.CombineLatest(variableList).Select(varList =>
                {
                    Dictionary<Robot, Dictionary<string, float>> dict = new Dictionary<Robot, Dictionary<string, float>>();
                    foreach (Dictionary<string, float> varDict in varList)
                    {
                        if (!dict.ContainsKey(r))
                        {
                            dict[r] = new Dictionary<string, float>();
                        }

                        foreach (string key in varDict.Keys)
                        {
                            dict[r][key] = varDict[key];
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
            // TODO: make a two or more option
            return ParameterCount.N;
        }

        public ParameterCount GetNumRobots()
        {
            return ParameterCount.One;
        }

        public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData()
        {
            // This encodes the data source into a dictionary containing
            // one or more values per robot
            return dataSource;
        }
    }
}