using UnityEngine;
using System.Collections;

public class ColliderCast : MonoBehaviour
{
    Transform parent;

    Vector3 oldPosition;
    bool isMoving, isCasting;
    void Start()
    {
        isMoving = false;
        isCasting = false;
        parent = transform.parent;
    }

    void Update()
    {

    }

    void SetTarget(Vector3 target)
    {
        int distance = (int)Vector3.Distance(transform.position, target);
        Vector3 direction = (target - transform.position).normalized;
        for (int i = 0; i < distance; i++)
        {
            transform.position += direction;
        }
        if (isCasting)
        {
            Debug.Log("CASTING");
            //isCasting = false;
        }
        else
        {
            //послать сигнал мешу перемещаться
            transform.position = parent.position;
            parent.gameObject.SendMessage("SetTarget", target);
            
        }
        //isCasting = false;
    }

    void SetMoving(bool value)
    {
        isMoving = value;
        if (value)
        {
            oldPosition = transform.position;
        }
        else
        {
            transform.position = parent.position;
            isCasting = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        isCasting = true;
        Debug.Log("enter");
    }
}
