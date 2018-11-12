using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshCollider))]
public class MoveDrag : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    public int clickState;
    public Transform target;


    void OnMouseDown()
    {   if (clickState == 0)
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            clickState = 1;
            transform.SetParent(null);
        } else if (clickState == 1 )
        {
            transform.SetParent(target);
            //transform.transform.parent = target.transform;
            //transform.transform.localPosition = target.GetComponent<PositionReferences>().GetNextPosition();

            clickState = 0;
        }

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
        
    }



    // Use this for initialization
    void Start()
    {
        clickState = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
 class PositionReferences : MonoBehaviour
{

    public Transform[] positions;
    private int index = 0;

    public Vector3 GetNextPosition()
    {
        Vector3 result = positions[index].localPosition;
        index = index + 1;
        return result;
    }
}