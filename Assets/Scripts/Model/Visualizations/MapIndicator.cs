using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using shapeNamespace;

public class MapIndicator : IVisualization 
{
    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
    HashSet<Robot> robotSet;
    HashSet<string> varSet;

    List<MapPolicy> policyList;

    Color defaultColor;
    IndicatorShape defaultShape;

    // variables[0] needs to correspond to policies[0]
    public MapIndicator(List<MapPolicy> policies, Color color, IndicatorShape shape, Robot firstRobot, params Robot[] robots)
    {
        robotSet = new HashSet<Robot>(robots);
        robotSet.Add(firstRobot);

        varSet = new HashSet<string>();
        foreach (MapPolicy p in policies) {
            varSet.Add(p.variableName);
        }

        policyList = new List<MapPolicy>(policies);

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

    public void AddPolicy(MapPolicy P, string var) // need to also add the corresponding variable
    {
        // verify that the policy works with the others
        foreach (MapPolicy policy in policyList) {
            if (P.type == policy.type) {
                // throw some kind of error
                throw new Exception(String.Format("Policy of type ('{0}') already exists.", P.type));
            }
            else {
                policyList.Add(P);
                varSet.Add(var);
            }
        }
    }

    public IndicatorShape GetDefaultShape() {
        return defaultShape;
    }

    public Color GetDefaultColor() {
        return defaultColor;
    }

    public List<MapPolicy> GetPolicies() {
        return policyList;
    }

    public ISet<Robot> GetRobots()
    {
        return robotSet;
    }

    public ISet<string> GetVariables()
    {
        return varSet;
    }

    public ParameterCount GetNumDataSources()
    {
        return ParameterCount.N; 
    }

    public ParameterCount GetNumRobots()
    {
        return ParameterCount.N;
    }

    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData()
    {
        return dataSource;
    }
}
