using UnityEngine;
using System.Collections;

public class CameraPauseController : MonoBehaviour
{

    Ray cameraRay;
    RaycastHit hit;
    Vector3 previousCursorPosition,
    previousRotationPoint,
    startCursorPosition;

    bool isSwipe = false;
    bool rotateObject = false;
    GameObject objAssembled;
    void Start()
    {

    }

    public void CameraUpdate()
    {
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(cameraRay.origin, cameraRay.origin + cameraRay.direction * 10, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("mouseDown");
            startCursorPosition = Input.mousePosition;
            SwipeStart();
            isSwipe = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("mouseUp");
            if (isSwipe)
            {
                isSwipe = false;
                rotateObject = false;
            }
        }

        if (isSwipe) //Вызывается каждый фрейм от начала свайпа
        {
            Swipe();
        }
    }

    void SwipeStart()
    {
        //Debug.Log("SwipeStart");
        if (Physics.Raycast(cameraRay, out hit))
        {
            if (hit.collider.tag == "Assembled")
            {
                rotateObject = true;
                objAssembled = hit.collider.gameObject;
                previousCursorPosition = Input.mousePosition;
            }
        }
    }
    void Swipe()
    {
        if (rotateObject) //вращение объекта
        {
            Vector3 cursorDirection = previousCursorPosition - Input.mousePosition;
            Vector3 axis = -Vector3.Cross(
                transform.TransformVector(cursorDirection), 
                transform.TransformVector(cursorDirection + Vector3.forward));
            objAssembled.transform.Rotate(axis, cursorDirection.magnitude * 40f * Time.deltaTime, Space.World);
            previousCursorPosition = Input.mousePosition;
        }
    }
}
