using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

/// <summary>
/// A data structure which mimics a dictionary that maps name to a value of any type
/// </summary>
namespace DrSwarm.Model
{
    public class VariableDict
    {

        /// <summary>
        /// A set containing the names of variables contained within the dictionary
        /// </summary>
        HashSet<string> variableNames;

        /// <summary>
        /// A dictionary contains dictionaries for each different <see cref="Type"/>
        /// </summary>
        Dictionary<Type, IDictionary> dictionaries;

        /// <summary>
        /// Create an empty VariableDict
        /// </summary>
        public VariableDict()
        {
            variableNames = new HashSet<string>();
            dictionaries = new Dictionary<System.Type, IDictionary>();
        }

        /// <summary>
        ///     Get the dictionary associated with a type.
        ///     Creates the dictionary if it does not exist already.
        /// </summary>
        /// <typeparam name="T">The type associated with the dictionary</typeparam>
        /// <returns>The true dictioary mapping name -> Observable Type</returns>
        private Dictionary<string, BehaviorSubject<T>> GetDictionary<T>()
        {
            if (!dictionaries.ContainsKey(typeof(T)))
            {
                dictionaries.Add(typeof(T), new Dictionary<string, BehaviorSubject<T>>());
            }

            System.Type type = typeof(T);
            return (Dictionary<string, BehaviorSubject<T>>)dictionaries[type];
        }

        /// <summary>
        /// Get the current value of a variable.
        /// </summary>
        /// <typeparam name="T">The type of the variable</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <returns>The value of the variable with name, or the default value for the type</returns>
        public T GetValue<T>(string name)
        {
            // TODO: Should this throw an exception if the variable doesn't exist?
            if (Has<T>(name)) return GetDictionary<T>()[name].Value;
            return default(T);
        }

        /// <summary>
        ///     Get the observable value of a variable.
        ///     Will create a new observable if the variable has not been set yet.
        /// </summary>
        /// <typeparam name="T">The type of the variable</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <returns>An observable of the value of the variable.</returns>
        public IObservable<T> GetObservableValue<T>(string name)
        {
            // TODO: Should this throw an exception if the variable doesn't exist?
            if (!Has<T>(name))
                SetValue(name, default(T));

            return GetDictionary<T>()[name];
        }

        /// <summary>
        ///     Set the value of the variable of type T.
        /// </summary>
        /// <typeparam name="T">The type of the variable</typeparam>
        /// <param name="name">The name of the value</param>
        /// <param name="value">The new value of the variable</param>
        /// <exception cref="ArgumentException">
        ///     If <typeparamref name="T"/> does not match the existing <see cref="Type"/> of the variable named <see cref="name"/>.
        /// </exception>
        public void SetValue<T>(string name, T value)
        {
            if (Has<T>(name))
            {
                GetDictionary<T>()[name].OnNext(value);
            }
            else
            {
                if (!variableNames.Contains(name))
                {
                    GetDictionary<T>().Add(name, new BehaviorSubject<T>(value));
                    variableNames.Add(name);
                }
                else
                {
                    throw new System.ArgumentException("Parameter already exists as a different type", "name");
                }
            }
        }

        /// <summary>
        /// Check if a variable named <paramref name="name"/> of type <typeparamref name="T"/> exists
        /// </summary>
        /// <typeparam name="T">The type of the variable</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <returns>True if the variable exists</returns>
        public bool Has<T>(string name)
        {
            return GetDictionary<T>().ContainsKey(name);
        }

        public List<string> GetVariables()
        {
            return new List<string>(variableNames);
        }

        public List<string> GetVariables<T>()
        {
            return new List<string>(GetDictionary<T>().Keys);
        }
    }
}