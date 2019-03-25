using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using shapeNamespace;

/// <summary>
/// Class representing the map indicator visualization.
/// </summary>
/// <remarks>
/// The map indicator is used to display an indicator that can map color, shape, and fill amount to the observed data.
/// This is accomplished through a Policy. As an example, this visualization can be used to display the charge level of a battery
/// as a bar that shrinks and changes color from green to red as the battery depletes.
/// </remarks>
public class MapIndicator : IVisualization
{
    IObservable<Dictionary<Robot, Dictionary<string, float>>> dataSource;
    HashSet<Robot> robotSet;
    HashSet<string> varSet;

    List<MapPolicy> policyList;

    Color defaultColor;
    IndicatorShape defaultShape;

    /// <summary>
    /// The class constructor.
    /// </summary>
    /// <remarks>
    /// variables[0] will correspond to policies[0].
    /// </remarks>
    /// <param name="policies"> The policies assigned to this visualization. </param>
    /// <param name="color"> The default color of the indicator. </param>
    /// <param name="shape"> The default shape of the indicator. </param>
    /// <param name="firstRobot"> A robot to attach to the visualization. </param>
    /// <param name="robots"> Any other robots to attach to the visualization. </param>
    public MapIndicator(List<MapPolicy> policies, Color color, IndicatorShape shape, Robot firstRobot, params Robot[] robots)
    {
        robotSet = new HashSet<Robot>(robots);
        robotSet.Add(firstRobot);

        varSet = new HashSet<string>();
        foreach (MapPolicy p in policies)
        {
            varSet.Add(p.variableName);
        }

        policyList = new List<MapPolicy>(policies);

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
    /// Adds a new MapPolicy to policyList if its compatible with the current policies.
    /// </summary>
    /// <param name="P"> The policy being added. </param>
    /// <param name="var"> The variable name that is mapped to the indicator. </param>
    public void AddPolicy(MapPolicy P, string var)
    {
        // Verify that the new policy works with the existing ones.
        foreach (MapPolicy policy in policyList)
        {
            if (P.type == policy.type)
            {
                throw new Exception(String.Format("Policy of type ('{0}') already exists.", P.type));
            }
            else
            {
                policyList.Add(P);
                varSet.Add(var);
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
    /// Returns a list of MapPolicies
    /// </returns>
    public List<MapPolicy> GetPolicies()
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
        return ParameterCount.N; 
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
