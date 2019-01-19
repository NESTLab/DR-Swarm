using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class LineGraph : IVisualization
{
    IObservable<Dictionary<Robot, float>> xAxisObs, yAxisObs;
    HashSet<Robot> robotList;

    // TODO: Hmm, not sure about passing in variable names here ... maybe change later?
    public LineGraph(string xAxisName, string yAxisName, Robot firstRobot, params Robot[] robots)
    {
        robotList = new HashSet<Robot>(robots);
        robotList.Add(firstRobot);

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

    public ISet<Robot> GetRobots()
    {
        return robotList;
    }

    public ParameterCount GetNumDataSources()
    {
        return ParameterCount.Two;
    }

    public ParameterCount GetNumRobots()
    {
        return ParameterCount.N;
    }

    public IObservable<Dictionary<Robot, IList<float>>> GetObservableData()
    {
        // TODO: Zip probably isn't right here, consider options
        return Observable.Zip(xAxisObs, yAxisObs).Select(values =>
        {
            Dictionary<Robot, IList<float>> returnDict = new Dictionary<Robot, IList<float>>();
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
