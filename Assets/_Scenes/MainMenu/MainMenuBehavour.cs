using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MainMenuBehavour : MonoBehaviour
{
    public int tapTrigger = 13;
    public Material lockMaterial;
    public Material[] materials;

    Ray cameraRay;
    RaycastHit hit;
    Vector3 previousCursorPosition,
previousRotationPoint,
startCursorPosition;
    int tapCount;
    bool isSwipe;

    GameObject Models;
    GameObject ButtonPanel;
    RectTransform rtButtonPanel;

    float ScreenRatio;
    float ScreenToWorldRatio; //отношение размеров экрана к разрешающей способности камеры
    float ScreenToCanvasRatio; // отношение размеров экрана к разрешающей способности canvas

    bool MoveModels = false;
    bool swipeStart = false;
    void Start()
    {
        Models = GameObject.Find("Models");

        float width = Screen.width;
        float height = Screen.height;
        float ScreenRatio = width / height;
        

        float CameraSize = Camera.main.orthographicSize;
        float widthBorder = CameraSize * ScreenRatio;
        ScreenToWorldRatio = width / 2f / widthBorder;
        float golden = 2 * widthBorder * 0.62f + widthBorder * 0.38f - widthBorder;
        GameObject.Find("Levels").transform.position = new Vector3(golden, 0, 0);
        GameObject.Find("Levels").GetComponent<LerpMove>().showPosition = new Vector3(golden, 0, 0);

        float CanvasReferenceResolution = GameObject.Find("MainCanvas").GetComponent<CanvasScaler>().referenceResolution.x;
        ScreenToCanvasRatio = width / CanvasReferenceResolution;
        RectTransform rt = GameObject.Find("BURRlogo").GetComponent<RectTransform>();
        rt.anchoredPosition3D = new Vector3(CanvasReferenceResolution * 0.62f / 2f, 0, 0);
    }
    void Update()
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
            swipeStart = true;
        }
        if (isSwipe) //Вызывается каждый фрейм от начала свайпа
        {
            Swipe();
        }
    }

    void SwipeStart()
    {
        if (Physics.Raycast(cameraRay, out hit))
        {
            MoveModels = true;
            previousCursorPosition = Input.mousePosition;
        }
        else
        { MoveModels = false; }
    }
    void Swipe()
    {
        if (MoveModels)
        {
            Debug.Log(Input.mousePosition);
            Vector3 CursorDirection = Input.mousePosition - previousCursorPosition;
            CursorDirection = new Vector3(0, CursorDirection.y, 0);
            Vector3 targetModelsPosition = Models.transform.position + CursorDirection / ScreenToWorldRatio;
            if (targetModelsPosition.y > 0f && targetModelsPosition.y < 19.1f)
                Models.transform.position = targetModelsPosition;

            previousCursorPosition = Input.mousePosition;
        }
    }
    void SwipeEnd()
    {

    }
    public void Tap()
    {
        if (Physics.Raycast(cameraRay, out hit))
        {
            if (hit.collider.tag == "Assembled")
            {
                string level = "level"+hit.collider.name.Substring(9);
                GetComponent<SaveManager>().LoadLevel(level);
                
            }
        }
    }
}
