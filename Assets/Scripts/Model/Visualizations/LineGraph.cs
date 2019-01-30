using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class LineGraph : IVisualization
{
    IObservable<Dictionary<Robot, float>> xAxisObs, yAxisObs;
    HashSet<Robot> robotSet;

    string xAxisName, yAxisName;

    // TODO: Hmm, not sure about passing in variable names here ... maybe change later?
    public LineGraph(string xAxisName, string yAxisName, Robot firstRobot, params Robot[] robots)
    {
        robotSet = new HashSet<Robot>(robots);
        robotSet.Add(firstRobot);

        this.xAxisName = xAxisName;
        this.yAxisName = yAxisName;

        // TODO: Maybe make this a helper function somewhere
        xAxisObs = robotSet.ToObservable().SelectMany(r =>
        {
            return r.GetObservableVariable<float>(xAxisName).Select(v => new Dictionary<Robot, float> { { r, v } });
        });

        yAxisObs = robotSet.ToObservable().SelectMany(r =>
        {
            return r.GetObservableVariable<float>(yAxisName).Select(v => new Dictionary<Robot, float> { { r, v } });
        });
    }

    public ISet<Robot> GetRobots()
    {
        return robotSet;
    }

    public ISet<string> GetVariables()
    {
        return new HashSet<string>() { xAxisName, yAxisName }; ;
    }

    public ParameterCount GetNumDataSources()
    {
        return ParameterCount.Two;
    }

    public ParameterCount GetNumRobots()
    {
        return ParameterCount.N;
    }

    public IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData()
    {
        // TODO: Zip probably isn't right here, consider options
        return Observable.Zip(xAxisObs, yAxisObs).Select(values =>
        {
            Dictionary<Robot, Dictionary<string, float>> returnDict = new Dictionary<Robot, Dictionary<string, float>>();
            Dictionary<Robot, float> xAxis = values[0];
            Dictionary<Robot, float> yAxis = values[1];

            foreach (Robot r in xAxis.Keys)
            {
                returnDict[r] = new Dictionary<string, float>();
                returnDict[r].Add("x,"+xAxisName, xAxis[r]);
                returnDict[r].Add("y,"+yAxisName, yAxis[r]);
            }

            return returnDict;
        });
    }
}
