using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendGraphType : MonoBehaviour
{       

    public void setToGraph(string i) {
        VisualizationManager.Instance.GraphType = i;
    }
}
