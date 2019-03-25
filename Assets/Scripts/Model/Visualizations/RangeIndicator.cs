using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using shapeNamespace;

// TODO: fill out the documentation
/// <summary>
/// Class level summary documentation goes here.
/// </summary>
/// <remarks>
/// Longer comments can be associated with a type or member through
/// the remarks tag.
/// </remarks>
public class RangeIndicator : IVisualization
{
    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
    HashSet<Robot> robotSet;
    HashSet<string> varSet;

    List<RangePolicy> policyList;

    Color defaultColor;
    IndicatorShape defaultShape;

    /// <summary>
    /// The class constructor.
    /// </summary>
    public RangeIndicator(string variableName, List<RangePolicy> policies, Color color, IndicatorShape shape, Robot firstRobot, params Robot[] robots) 
    {
        robotSet = new HashSet<Robot>(robots);
        robotSet.Add(firstRobot);

        varSet = new HashSet<string>();
        varSet.Add(variableName);

        policyList = new List<RangePolicy>(policies);

        defaultColor = color;
        defaultShape = shape;

        dataSource = robotSet.ToObservable().SelectMany(robot => 
        {
            List<IObservable<Dictionary<string, float>>> variableList = new List<IObservable<Dictionary<string, float>>>();
            foreach (string variable in varSet) 
            {
                variableList.Add(robot.GetObservableVariable<float>(variable).Select(v => 
                {
                    return new Dictionary<string, float>() { { variable, v } };
                }));
            }

            return Observable.CombineLatest(variableList).Select(varList => 
            {
                Dictionary<Robot, Dictionary<string, float>> dict = new Dictionary<Robot, Dictionary<string, float>>();
                foreach (Dictionary<string, float> varDict in varList) 
                {
                    if (!dict.ContainsKey(robot)) 
                    {
                        dict[robot] = new Dictionary<string, float>();
                    }

                    foreach (string key in varDict.Keys) 
                    {
                        dict[robot][key] = varDict[key];
                    }
                }

                return dict;
            });
        });
    }

    /// <summary>
    /// Some other method.
    /// </summary>
    /// <returns>
    /// Return values are described through the returns tag.
    /// </returns>
    public void AddPolicy(RangePolicy P) 
    {
        // Verify that the new policy works with the existing ones.
        foreach (RangePolicy policy in policyList) 
        {
            // If the new policy's min or max is in between a current policy's min and max, it is incompatible and cannot be added.
            // TODO: are these the only cases?
            if ((policy.range[0] <= P.range[0] && P.range[0] < policy.range[1]) || (P.range[0] <= policy.range[0] && policy.range[0] < P.range[1])) 
            { 
                throw new Exception(String.Format("Policy range is incompatible"));
            }
            else 
            {
                policyList.Add(P);
            }
        }
    }

    /// <summary>
    /// Some other method.
    /// </summary>
    /// <returns>
    /// Return values are described through the returns tag.
    /// </returns>
    /// <seealso cref="SomeMethod(string)">
    /// Notice the use of the cref attribute to reference a specific method.
    /// </seealso>
    public IndicatorShape GetDefaultShape() 
    {
        return defaultShape;
    }

    /// <summary>
    /// Some other method.
    /// </summary>
    /// <returns>
    /// Return values are described through the returns tag.
    /// </returns>
    /// <seealso cref="SomeMethod(string)">
    /// Notice the use of the cref attribute to reference a specific method.
    /// </seealso>
    public Color GetDefaultColor() 
    {
        return defaultColor;
    }

    /// <summary>
    /// Some other method.
    /// </summary>
    /// <returns>
    /// Return values are described through the returns tag.
    /// </returns>
    /// <seealso cref="SomeMethod(string)">
    /// Notice the use of the cref attribute to reference a specific method.
    /// </seealso>
    public List<RangePolicy> GetPolicies() 
    {
        return policyList;
    }

    /// <seealso cref="SomeMethod(string)">
    /// Notice the use of the cref attribute to reference a specific method.
    /// </seealso>
    public ISet<Robot> GetRobots() 
    {
        return robotSet;
    }

    /// <seealso cref="SomeMethod(string)">
    /// Notice the use of the cref attribute to reference a specific method.
    /// </seealso>
    public ISet<string> GetVariables() 
    {
        return varSet;
    }

    /// <seealso cref="SomeMethod(string)">
    /// Notice the use of the cref attribute to reference a specific method.
    /// </seealso>
    public ParameterCount GetNumDataSources() 
    {
        return ParameterCount.One;
    }

    /// <seealso cref="SomeMethod(string)">
    /// Notice the use of the cref attribute to reference a specific method.
    /// </seealso>
    public ParameterCount GetNumRobots() 
    {
        return ParameterCount.N;
    }

    /// <seealso cref="SomeMethod(string)">
    /// Notice the use of the cref attribute to reference a specific method.
    /// </seealso>
    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData() 
    {
        return dataSource;
    }
}
