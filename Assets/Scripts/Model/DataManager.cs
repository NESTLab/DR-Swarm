using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrSwarm.Model
{
    public class DataManager : MonoBehaviour
    {

        Dictionary<string, Robot> robots;

        public DataManager()
        {
            robots = new Dictionary<string, Robot>();
        }

        #region SINGLETON PATTERN
        // Thanks https://answers.unity.com/questions/891380/unity-c-singleton.html
        private static DataManager _instance;
        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<DataManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("DataModel");
                        _instance = container.AddComponent<DataManager>();
                    }
                }

                return _instance;
            }
        }
        #endregion

        public Robot GetRobot(string robotName)
        {
            if (!robots.ContainsKey(robotName))
            {
                robots[robotName] = new Robot(robotName);
            }

            return robots[robotName];
        }
        //Return List of all robots that there is data for
        public List<Robot> GetAllRobots()
        {
            return new List<Robot>(robots.Values);
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}