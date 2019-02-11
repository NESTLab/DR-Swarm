using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

abstract public class Policy {
    public readonly string name;

    public Policy(string name)
    {
        this.name = name;
    }

    public abstract void Accept<T>(Indicator<T> indicator) where T : Policy;
}

public class RangePolicy : Policy
{
    public enum IndicatorShape
    {
        Square,
        Circle,
        Triangle,
        Plus,
        Check,
        Exclaimation
    }

    public readonly Vector2 range;
    public Color color;
    public IndicatorShape shape;

    public RangePolicy(string name, float minValue, float maxValue) : base(name)
    {
        this.range = new Vector2(minValue, maxValue);
    }

    public override void Accept<T>(Indicator<T> indicator)
    {
        indicator.VisitRangePolicy(this);
    }
}

public class MapPolicy : Policy
{
    public enum MapPolicyType
    {
        color,
        orientation
    }

    public readonly string variableName;
    public readonly MapPolicyType type;

    public MapPolicy(string name, string variableName, MapPolicyType type) : base(name)
    {
        this.variableName = variableName;
        this.type = type;
    }

    public override void Accept<T>(Indicator<T> indicator)
    {
        indicator.VisitMapPolicy(this);
    }
}

public class Indicator<T> : IVisualization where T : Policy
{
    IObservable<Dictionary<Robot, float>> dataSource;
    HashSet<Robot> robotList;

    List<T> policies;

    public Indicator(string variableName, Robot firstRobot, params Robot[] robots)
    {
        robotList = new HashSet<Robot>(robots);
        robotList.Add(firstRobot);
        
        // Create a single data source observable by creating
        // an observable for each robot, and then combining them
        // (this is what select many does)
        dataSource = robotList.ToObservable().SelectMany(r =>
        {
            // get the observable variable from the robot
            // then transform the values from the observable
            // into a Dictionary<Robot, float>
            return r.GetObservableVariable<float>(variableName).Select(v => new Dictionary<Robot, float> { { r, v } });
        });
    }

    public void AddPolicy(T P)
    {
        P.Accept(this);
    }

    public void VisitRangePolicy(RangePolicy policy)
    {

    }

    public void VisitMapPolicy(MapPolicy policy)
    {

    }

    public List<T> GetPolicies() {
        return this.policies;
    }

    public ISet<Robot> GetRobots()
    {
        return robotList;
    }

    public ISet<string> GetVariables()
    {
        throw new NotImplementedException();
    }

    public ParameterCount GetNumDataSources()
    {
        return ParameterCount.One;
    }

    public ParameterCount GetNumRobots()
    {
        return ParameterCount.One;
    }

    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData()
    {
        // Take the Dictionary<Robot, float> and transform it
        // into a Dictionary<Robot, Dictionary<string, float>>
        // This is a dictionary that maps robots to a dict
        // which maps variable name (string) to value (float)

        throw new NotImplementedException();
    }
}
