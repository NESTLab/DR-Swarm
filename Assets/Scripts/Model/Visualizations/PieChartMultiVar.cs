using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Operators;

public class PieChartMultiVar : IVisualization {
    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
    HashSet<Robot> robotSet;
    HashSet<string> varSet;

    public PieChartMultiVar(List<string> variables, List<Robot> robots) {
        robotSet = new HashSet<Robot>(robots);
        varSet = new HashSet<string>(variables);

        dataSource = robotSet.ToObservable().SelectMany(robot => {
            List<IObservable<Dictionary<string, float>>> variableList = new List<IObservable<Dictionary<string, float>>>();
            foreach (string variable in varSet) {
                variableList.Add(robot.GetObservableVariable<float>(variable).Select(v => {
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

    public ISet<Robot> GetRobots() {
        return robotSet;
    }

    public ISet<string> GetVariables() {
        return varSet;
    }

    public ParameterCount GetNumDataSources() {
        // TODO: make a two or more option
        return ParameterCount.N;
    }

    public ParameterCount GetNumRobots() {
        return ParameterCount.One;
    }

    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData() {
        return dataSource;
    }
}
