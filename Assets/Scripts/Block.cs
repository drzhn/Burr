using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    Vector3 targetPosition;
    Vector3 oldTargetPosition;

    Quaternion targetRotation, oldTargetRotation;
    bool isMoving, isRotate;
    GameManager gm;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        isMoving = false;
        isRotate = false;
        targetPosition = transform.position;
        oldTargetPosition = transform.position;
        targetRotation = transform.rotation;
        oldTargetRotation = transform.rotation;
    }

    void Update()
    {
        if (isMoving && !isRotate)
        {
            Move();
        }
        if (isRotate && !isMoving)
        {
            Rotate();
        }
    }

    void Rotate()
    {
        if ((transform.rotation.eulerAngles - targetRotation.eulerAngles).magnitude >= 10f)
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 50f * Time.deltaTime);
        else
        {
            transform.rotation = targetRotation;
            oldTargetRotation = targetRotation;
            isRotate = false;
            gm.free = true;
        }
    }

    void Move()
    {
        if ((transform.position - targetPosition).magnitude >= 0.01f)
            transform.position = Vector3.Lerp(transform.position, targetPosition, 20f * Time.deltaTime);
        else
        {
            transform.position = targetPosition;
            oldTargetPosition = targetPosition;
            isMoving = false;
            gm.free = true;
            //после каждого перемещения будем проверять, а не завершен ли уровень
            if (gm.pos.CheckAssemble())
            {
                gm.GetComponent<SceneUI>().EndLevel();
            }
        }
    }

    void SetTargetPosition(Vector3 target)
    {
        oldTargetPosition = targetPosition;
        targetPosition = target;

        isMoving = true;
    }

    void SetTargetRotation(Quaternion target)
    {
        oldTargetRotation = targetRotation;
        targetRotation = target;

        isRotate = true;
    }

    void OnTriggerEnter(Collider col)
    {
        targetPosition = oldTargetPosition;
    }
}

