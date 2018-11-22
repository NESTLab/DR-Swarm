using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class LineGraph : IVisualization
{
    IObservable<Dictionary<Robot, float>> xAxisObs, yAxisObs;

    // TODO: Hmm, not sure about passing in variable names here ... maybe change later?
    public LineGraph(string xAxisName, string yAxisName, Robot firstRobot, params Robot[] robots)
    {
        List<Robot> robotList = new List<Robot>(robots);
        robotList.Insert(0, firstRobot);

        // TODO: Maybe make this a helper function somewhere
        xAxisObs = robotList.ToObservable().SelectMany(r =>
        {
            return r.GetObservableVariable<float>(xAxisName).Select(v => new Dictionary<Robot, float> { { r, v } });
        });

        yAxisObs = robotList.ToObservable().SelectMany(r =>
        {
            return r.GetObservableVariable<float>(yAxisName).Select(v => new Dictionary<Robot, float> { { r, v } });
        });
    }

    public ParameterCount getNumDataSources()
    {
        return ParameterCount.Two;
    }

    public ParameterCount getNumRobots()
    {
        return ParameterCount.N;
    }

    public IObservable<Dictionary<Robot, List<float>>> getObservableData()
    {
        return Observable.CombineLatest(xAxisObs, yAxisObs).Select(values =>
        {
            Dictionary<Robot, List<float>> returnDict = new Dictionary<Robot, List<float>>();
            Dictionary<Robot, float> xAxis = values[0];
            Dictionary<Robot, float> yAxis = values[1];

            foreach (Robot r in xAxis.Keys)
            {
                returnDict[r] = new List<float> { xAxis[r], yAxis[r] };
            }

            return returnDict;
        });
    }
}
