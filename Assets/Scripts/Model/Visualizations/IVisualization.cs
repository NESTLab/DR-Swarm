using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public interface IVisualization
{
    IObservable<Dictionary<Robot, List<float>>> getObservableData();

    ParameterCount getNumDataSources();
    ParameterCount getNumRobots();
}