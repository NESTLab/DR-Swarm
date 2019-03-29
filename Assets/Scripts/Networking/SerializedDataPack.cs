using System.Collections.Generic;
using DrSwarm.Model;

namespace DrSwarm.Networking
{
    public class SerializedDataPack
    {
        public Dictionary<string, Dictionary<string, float>> variables;

        public SerializedDataPack()
        {
            variables = new Dictionary<string, Dictionary<string, float>>();
        }

        public void AssignVariables()
        {
            foreach (string robotName in variables.Keys)
            {
                Robot robot = DataManager.Instance.GetRobot(robotName);

                foreach (string variableName in variables[robotName].Keys)
                {
                    robot.SetVariable(variableName, variables[robotName][variableName]);
                }
            }
        }
    }
}