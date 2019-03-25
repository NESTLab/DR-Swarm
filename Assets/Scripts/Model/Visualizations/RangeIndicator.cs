using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using shapeNamespace;

/// <summary>
/// Class representing the range indicator visualization.
/// </summary>
/// <remarks>
/// The range indicator is used to display an indicator that can change color or shape for different set ranges of the observed data.
/// This is accomplished through a Policy, which defines those ranges and expected results.
/// As an example, this visualization can be used to display a red circle when it's idle, a green square when it's exploring, and a blue triangle when it's
/// gathering food.
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
    /// <param name="variableName"> The name of the observed data. </param>
    /// <param name="policies"> The policies assigned to this visualization. </param>
    /// <param name="color"> The default color of the indicator. </param>
    /// <param name="shape"> The default shape of the indicator. </param>
    /// <param name="firstRobot"> A robot to attach to the visualization. </param>
    /// <param name="robots"> Any other robots to attach to the visualization. </param>
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
    /// Adds a new RangePolicy to policyList if its compatible with the current policies.
    /// </summary>
    /// <param name="P"> The policy being added. </param>
    public void AddPolicy(RangePolicy P) 
    {
        // Verify that the new policy works with the existing ones.
        foreach (RangePolicy policy in policyList) 
        {
            // If the new policy's min or max is in between a current policy's min and max, it is incompatible and cannot be added.
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
    /// Gets the default shape of the indicator.
    /// </summary>
    /// <returns>
    /// Returns an IndicatorShape.
    /// </returns>
    public IndicatorShape GetDefaultShape() 
    {
        return defaultShape;
    }

    /// <summary>
    /// Gets the default color of the indicator.
    /// </summary>
    /// <returns>
    /// Returns a Color.
    /// </returns>
    public Color GetDefaultColor() 
    {
        return defaultColor;
    }

    /// <summary>
    /// Gets the current policies attached to this indicator.
    /// </summary>
    /// <returns>
    /// Returns a list of RangePolicies
    /// </returns>
    public List<RangePolicy> GetPolicies() 
    {
        return policyList;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetRobots"/>
    /// </summary>
    public ISet<Robot> GetRobots() 
    {
        return robotSet;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetVariables"/>
    /// </summary>
    public ISet<string> GetVariables() 
    {
        return varSet;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetNumDataSources"/>
    /// </summary>
    public ParameterCount GetNumDataSources() 
    {
        return ParameterCount.One;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetNumRobots"/>
    /// </summary>
    public ParameterCount GetNumRobots() 
    {
        return ParameterCount.N;
    }

    /// <summary>
    /// See <see cref="IVisualization.GetObservableData"/>
    /// </summary>
    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData() 
    {
        return dataSource;
    }
}
