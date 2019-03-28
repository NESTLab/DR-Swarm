using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealDataTest : MonoBehaviour
{

    Robot r1, r2;

    // Start is called before the first frame update
    void Start()
    {
        r1 = DataManager.Instance.GetRobot("RobotTarget1");
        r2 = DataManager.Instance.GetRobot("RobotTarget2");
    }

    int i = 0;
    float t1 = 0, t2 = 0;

    bool added_vis = false;

    // Update is called once per frame
    void Update()
    {
        if ((i++ % 60) == 0)
        {
            r1.SetVariable("t1", t1);
            r2.SetVariable("t1", t1);

            float rand_range_1 = Random.Range(0, 5);
            float rand_range_2 = Random.Range(0, 5);
            float rand_range_3 = Random.Range(0, 2);
            float rand_range_4 = Random.Range(0, 2);

            r1.SetVariable("test_one", rand_range_1);
            r2.SetVariable("test_one", rand_range_2);

            r1.SetVariable("test_two", rand_range_3);
            r2.SetVariable("test_two", rand_range_4);

            t1++;
        }

        r1.SetVariable("t2", t2);
        r2.SetVariable("t2", t2);

        r1.SetVariable("test_sin", Mathf.Sin(t2 / 10));
        r2.SetVariable("test_sin", Mathf.Sin(t2 / 10));

        r1.SetVariable("test_cos", Mathf.Cos(t2 / 10));
        r2.SetVariable("test_cos", Mathf.Cos(t2 / 10));

        t2++;
    }
}
