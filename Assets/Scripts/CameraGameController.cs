using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MathFunctions;
public class CameraGameController : MonoBehaviour
{
    public GameObject x_Rot, y_Rot, z_Rot;
    public int tapTrigger = 10;

    RaycastHit hit;
    bool moveObject = false, moveCamera = false, rotateObject = false;
    GameObject obj;
    Ray cameraRay;
    Vector3 currentPosition, currentNormal;

    Vector3 previousCursorPosition,
        previousRotationPoint,
        startCursorPosition;

    int tapCount;


    bool isSwipe = false;
    bool swipeStart = false;

    float commonRotationAngle;

    MathFunctions.MathFunctions mf = new MathFunctions.MathFunctions();
    GameManager gm;
    void Start()
    {
        previousCursorPosition = Input.mousePosition;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        tapCount = 0;
        Debug.Log(Screen.width);
    }

    public void CameraUpdate()
    {
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(cameraRay.origin, cameraRay.origin + cameraRay.direction * 10, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("mouseDown");
            startCursorPosition = Input.mousePosition;
            tapCount = 0;
            SwipeStart();

        }
        if (Input.GetMouseButton(0))
        {
            tapCount++;
            //Debug.Log(tapCount);
            float swipeDistance = Vector3.Distance(startCursorPosition, Input.mousePosition);
            //свайп в двух случаях: когда tapcount >= tapTrigger и когда расстояние, которое прошел палец, больше определенной величины.
            if (tapCount >= tapTrigger || swipeDistance > 3f)
            {
                //Debug.Log("Swipe");
                isSwipe = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("mouseUp");
            if (isSwipe)
            {
                isSwipe = false;
                swipeStart = false;
                SwipeEnd();
            }
            else
            {
                if (tapCount <= tapTrigger)
                {
                    Debug.Log("TAP");
                    Tap();
                }
            }
        }
        if (isSwipe && !swipeStart) //Тело условия вызывается один раз в начале свайпа
        {
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
            if (hit.collider.tag == "Block")
            {
                Debug.Log("Block Start");
                rotateObject = false;
                moveObject = true;
                obj = hit.collider.gameObject;
                previousCursorPosition = Input.mousePosition;
                DestroyRotationControllers();
            }
            if (hit.collider.tag == "RotationController")
            {
                gm.actionStack.Push(new State(obj));
                moveObject = false;
                rotateObject = true;
                currentPosition = hit.collider.transform.position;
                switch (hit.collider.name.Substring(0,5))
                {
                    case "z_ROT":
                        currentNormal = obj.transform.forward;
                        Debug.Log(currentNormal);
                        break;
                    case "y_ROT":
                        currentNormal = obj.transform.up;
                        Debug.Log(currentNormal);
                        break;
                    case "x_ROT":
                        currentNormal = obj.transform.right;
                        Debug.Log(currentNormal);
                        break;
                }
                previousRotationPoint = mf.RayCrossPlane(cameraRay, currentPosition, currentNormal);
            }
        }
        else
        {
            moveCamera = true;
            previousCursorPosition = Input.mousePosition;
        }
        swipeStart = true;
    }
    void Swipe()
    {
        //Debug.Log("Swipe");
        if (moveObject && !(moveCamera || rotateObject)) //перемещение объекта пальцем
        {
        }
        if (rotateObject && !(moveCamera || moveObject)) //вращение объекта
        {
            Vector3 point = mf.RayCrossPlane(cameraRay, currentPosition, currentNormal); // получили точку в плоскости вращения
            float angle = mf.Angle(previousRotationPoint - currentPosition, point - currentPosition, currentNormal);
            obj.transform.Rotate(currentNormal, angle, Space.World);
            previousRotationPoint = point;
        }
        if (moveCamera && !(rotateObject || moveObject)) //вращение камеры вокруг (0;0;0)
        {
            Vector3 cursorDirection = previousCursorPosition - Input.mousePosition;
            Vector3 axis = Vector3.Cross(cursorDirection, cursorDirection + Vector3.forward);
            transform.RotateAround(Vector3.zero, transform.TransformDirection(axis), cursorDirection.magnitude * 20f * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(Vector3.zero - transform.position, transform.up);
            //Debug.DrawLine(Vector3.zero, oldCursorPosition);
            previousCursorPosition = Input.mousePosition;
        }
    }
    void SwipeEnd()
    {
        moveCamera = false;
        if (moveObject)
        {
            Debug.Log("Move Object END");
            moveObject = false;
            if (Input.mousePosition != previousCursorPosition)
            {
                Vector3 cursorDirection = Input.mousePosition - previousCursorPosition;
                Vector3 target = obj.transform.position + mf.CursorDirectionTo3DDirection(transform,cursorDirection) * gm.unitSize;
                gm.MoveObject(ref obj, target);
            }
        }
        if (rotateObject) // вращение объекта до угла, кратного 90 градусам.
        {
            rotateObject = false;
            Vector3 ea = obj.transform.rotation.eulerAngles;
            Quaternion target = Quaternion.Euler(new Vector3(
                //obj.transform.rotation = 
                Mathf.RoundToInt(ea.x / 90f) * 90,
                Mathf.RoundToInt(ea.y / 90f) * 90,
                Mathf.RoundToInt(ea.z / 90f) * 90));
            gm.RotateObject(ref obj, target);
        }
    }
    void Tap()
    {
        rotateObject = moveObject = moveCamera = false; // обнуляй...
        if (Physics.Raycast(cameraRay, out hit))
        {
            if (hit.collider.tag == "Block") //создание контроллеров вращения
            {
                if (obj.transform.position != Vector3.zero) //в  центре нельзя будет вращать
                {
                    GameObject x = (GameObject)Instantiate(x_Rot, obj.transform.position, obj.transform.rotation);
                    GameObject y = (GameObject)Instantiate(y_Rot, obj.transform.position, obj.transform.rotation);
                    GameObject z = (GameObject)Instantiate(z_Rot, obj.transform.position, obj.transform.rotation);
                    x.transform.parent = obj.transform;
                    y.transform.parent = obj.transform;
                    z.transform.parent = obj.transform;
                }
            }
            if (hit.collider.tag == "RotationController")
            {
            }
        }
        else // тапнули на пустом месте
        {
            DestroyRotationControllers();
        }
    }

    void DestroyRotationControllers()
    {
        GameObject[] rotContollers = GameObject.FindGameObjectsWithTag("RotationController");
        for (int i = 0; i < rotContollers.Length; i++)
        {
            Destroy(rotContollers[i]);
        }
    }
}
