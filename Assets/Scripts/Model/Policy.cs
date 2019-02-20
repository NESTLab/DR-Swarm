using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Policy {
    public readonly string name;

    public Policy(string name) {
        this.name = name;
    }
}

// the range can have a min and max value where the min is included and max is excluded
public class RangePolicy : Policy {
    public enum IndicatorShape {
        Square,
        Circle,
        Triangle,
        Plus,
        Check,
        Exclamation
    }

    public readonly Vector2 range;
    public Color color;
    public IndicatorShape shape;

    public RangePolicy(string name, float minValue, float maxValue) : base(name) {
        this.range = new Vector2(minValue, maxValue);
    }

    
}

public class MapPolicy : Policy {
    public enum MapPolicyType {
        color,
        orientation
    }

    public readonly string variableName;
    public readonly MapPolicyType type;

    public MapPolicy(string name, string variableName, MapPolicyType type) : base(name) {
        this.variableName = variableName;
        this.type = type;
    }
}
