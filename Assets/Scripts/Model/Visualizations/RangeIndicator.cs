using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class RangeIndicator : IVisualization
{
    HashSet<Robot> robotList;
    HashSet<string> varSet;

    List<RangePolicy> policies;

    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;

    public RangeIndicator(string variableName, Robot firstRobot, params Robot[] robots) {
        robotList = new HashSet<Robot>(robots);
        robotList.Add(firstRobot);

        varSet = new HashSet<string>();
        varSet.Add(variableName);

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

    public void AddPolicy(RangePolicy P) {
        // verify that the policy works with the others
        foreach (RangePolicy policy in this.policies) {
            // if new policy's min is in between a current policy's min and max, new one is incompatible
            if ((policy.range[0] <= P.range[0] && P.range[0] < policy.range[1]) || (P.range[0] <= policy.range[0] && policy.range[0] < P.range[1])) { // are these the only cases?
                throw new Exception(String.Format("Policy range is incompatible"));
            }
            else {
                this.policies.Add(P);
            }
        }
    }

    public List<RangePolicy> GetPolicies() {
        return this.policies;
    }

    public ISet<Robot> GetRobots() {
        return robotList;
    }

    public ISet<string> GetVariables() {
        return varSet;
    }

    public ParameterCount GetNumDataSources() {
        return ParameterCount.One;
    }

    public ParameterCount GetNumRobots() {
        return ParameterCount.N;
    }

    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData() {
        // Take the Dictionary<Robot, float> and transform it
        // into a Dictionary<Robot, Dictionary<string, float>>
        // This is a dictionary that maps robots to a dict
        // which maps variable name (string) to value (float)

        return dataSource;
    }
}
