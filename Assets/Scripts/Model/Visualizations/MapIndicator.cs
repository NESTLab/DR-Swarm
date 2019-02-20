using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MapIndicator : IVisualization 
{
    HashSet<Robot> robotList;
    HashSet<string> varSet;

    List<MapPolicy> policies;

    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;

    public MapIndicator(List<string> variables, Robot firstRobot, params Robot[] robots)
    {
        robotList = new HashSet<Robot>(robots);
        robotList.Add(firstRobot);

        varSet = new HashSet<string>(variables);

        dataSource = robotList.ToObservable().SelectMany(robot => {
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

    public void AddPolicy(MapPolicy P)
    {
        // verify that the policy works with the others
        foreach (MapPolicy policy in this.policies) {
            if (P.type == policy.type) {
                // throw some kind of error
                throw new Exception(String.Format("Policy of type ('{0}') already exists.", P.type));
            }
            else {
                this.policies.Add(P);
            }
        }
    }
    
    public List<MapPolicy> GetPolicies() {
        return this.policies;
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

    // TODO: decide if datasource should be defined inside this function or outside, then stardardize across all vis classes
    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData()
    {
        // Take the Dictionary<Robot, float> and transform it
        // into a Dictionary<Robot, Dictionary<string, float>>
        // This is a dictionary that maps robots to a dict
        // which maps variable name (string) to value (float)

        return dataSource;
    }
}
