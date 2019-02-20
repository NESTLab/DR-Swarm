using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using shapeNamespace;

public class MapIndicator : IVisualization 
{
    HashSet<Robot> robotList;
    //List<string> varList; // this needs to be a list this time for the case where one variable corresponds with multiple policies
    HashSet<string> varSet; // I think we need this too for making the observable dictionary

    List<MapPolicy> policyList;

    Dictionary<MapPolicy, string> policyDict;

    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;

    // variables[0] needs to correspond to policies[0]
    public MapIndicator(Dictionary<MapPolicy, string> policies, IndicatorShape shape, Robot firstRobot, params Robot[] robots)
    {
        robotList = new HashSet<Robot>(robots);
        robotList.Add(firstRobot);

        //varList = new List<string>(variables);
        //varSet = new HashSet<string>(variables);
        varSet = new HashSet<string>();
        foreach (MapPolicy p in policies.Keys) {
            varSet.Add(policies[p]);
        }

        //policyList = new List<MapPolicy>(policies);
        policyList = new List<MapPolicy>(policies.Keys);

        policyDict = new Dictionary<MapPolicy, string>(policies);

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
                policyDict[P] = var;
            }
        }
    }
    
    public List<MapPolicy> GetPolicies() {
        return policyList;
    }

    public ISet<Robot> GetRobots()
    {
        return robotList;
    }

    public ISet<string> GetVariables()
    {
        return varSet;
    }

    /*
    // NEW -- for pairing variables with policies
    public List<string> GetVarList() {
        return varList;
    }
    */

    public ParameterCount GetNumDataSources()
    {
        return ParameterCount.N; // this maybe should be N
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
