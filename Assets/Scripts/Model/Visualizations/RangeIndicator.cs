using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using shapeNamespace;

public class RangeIndicator : IVisualization
{
    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
    HashSet<Robot> robotSet;
    HashSet<string> varSet;

    List<RangePolicy> policyList;

    Color defaultColor;
    IndicatorShape defaultShape;

    public RangeIndicator(string variableName, List<RangePolicy> policies, Color color, IndicatorShape shape, Robot firstRobot, params Robot[] robots) {
        robotSet = new HashSet<Robot>(robots);
        robotSet.Add(firstRobot);

        varSet = new HashSet<string>();
        varSet.Add(variableName);

        policyList = new List<RangePolicy>(policies);

        defaultColor = color;
        defaultShape = shape;

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

    public void AddPolicy(RangePolicy P) {
        // verify that the policy works with the others
        foreach (RangePolicy policy in policyList) {
            // if new policy's min is in between a current policy's min and max, new one is incompatible
            if ((policy.range[0] <= P.range[0] && P.range[0] < policy.range[1]) || (P.range[0] <= policy.range[0] && policy.range[0] < P.range[1])) { //TODO: are these the only cases?
                throw new Exception(String.Format("Policy range is incompatible"));
            }
            else {
                policyList.Add(P);
            }
        }
    }
    
    public IndicatorShape GetDefaultShape() {
        return defaultShape;
    }

    public Color GetDefaultColor() {
        return defaultColor;
    }

    public List<RangePolicy> GetPolicies() {
        return policyList;
    }

    public ISet<Robot> GetRobots() {
        return robotSet;
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
        return dataSource;
    }
}
