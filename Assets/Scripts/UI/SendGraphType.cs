using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendGraphType : MonoBehaviour
{
    //Sends the graph type based on which graph button was pressed
    public void setToGraph(string i) {
        UIManager.Instance.GraphType = i; 
    }
}
