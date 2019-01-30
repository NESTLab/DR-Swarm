using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

// IVisualization is the interface for data about visualizations
// Each visualization should implement this, and a separate class for
// the VisualizationContainer
public interface IVisualization
{
    // getObservableData returns an observable Dictionary<Robot, List<float>>
    // The reason for using this data type is to be able to encode one or more
    // data points for every robot. For example, a Line Graph's observable data
    // will have the latest (x, y) coordinate for each robot. A bar graph will
    // instead have the latest single value for each robot.
    IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData();

    // Get the set of robots providing data to this visualization
    ISet<Robot> GetRobots();

    // Get the set of variables used in this visualization
    ISet<string> GetVariables();

    // Return the (constant) number of data sources and robots for this
    // visualization type. This data can be used for the visualization
    // adding and editing interface.
    ParameterCount GetNumDataSources();
    ParameterCount GetNumRobots();
}