using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

// TODO: go over the documentation
// TODO: make function for getObservable
/// <summary>
/// IVisualization is the interface for data about visualizations. 
/// Each visualization should implement this, and a separate class for the VisualizationContainer
/// </summary>
/// <remarks>
/// Details about the interface go here.
/// </remarks>
public abstract class IVisualization  // TODO: change name to Visualization
{
    // getObservableData returns an observable Dictionary<Robot, List<float>>
    // The reason for using this data type is to be able to encode one or more
    // data points for every robot. For example, a Line Graph's observable data
    // will have the latest (x, y) coordinate for each robot. A bar graph will
    // instead have the latest single value for each robot.
    /// <summary>
    /// Gets the most recent observable data from each robot and packages it up into an observable dictionary.
    /// </summary>
    /// <returns>
    /// The method returns an observable dictionary with the key of type Robot and the value a dictionary with the key of type string and value of type float.
    /// </returns>
    IObservable<Dictionary<Robot, Dictionary<string, float>>> GetObservableData();

    /// <summary>
    /// Gets the set of robots providing data to this visualization.
    /// </summary>
    /// <returns>
    /// The method returns a set of Robots.
    /// </returns>
    ISet<Robot> GetRobots();

    /// <summary>
    /// Gets the set of variables used in this visualization.
    /// </summary>
    /// <returns>
    /// The method returns a set of strings.
    /// </returns>
    ISet<string> GetVariables();

    /// <summary>
    /// Returns the (constant) number of data sources for this visualization type. 
    /// This data can be used for the visualization adding and editing interface.
    /// </summary>
    /// <returns>
    /// The method returns a ParameterCount.
    /// </returns>
    ParameterCount GetNumDataSources();

    /// <summary>
    /// Returns the (constant) number of robots for this visualization type. 
    /// This data can be used for the visualization adding and editing interface.
    /// </summary>
    /// <returns>
    /// The method returns a ParameterCount.
    /// </returns>
    ParameterCount GetNumRobots();
}