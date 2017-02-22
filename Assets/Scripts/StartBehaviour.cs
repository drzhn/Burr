using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartBehaviour : MonoBehaviour
{
    GameObject PauseButton, UndoButton;
    GameObject StartButton, Assembled;

    TransparentBehaviour tb;

    CameraController CameraController;

    float currentTime;

    bool CameraBack = false;
    bool showStartButton = false;
    void Start()
    {
        //убираем кнопки "отменить" и "пауза"
        PauseButton = GameObject.Find("Pause");
        UndoButton = GameObject.Find("Undo");
        RectTransform rt = PauseButton.GetComponent<RectTransform>();
        rt.anchoredPosition3D = new Vector3(-40, 40, 0);
        rt = UndoButton.GetComponent<RectTransform>();
        rt.anchoredPosition3D = new Vector3(-40, -40, 0);
        //делаем прозрачной кнопку старта
        StartButton = GameObject.Find("Start");
        StartButton.GetComponent<Image>().color = new Color(1f, 1f,1f,0);
        //StartButton.SetActive(false);
        //опускаем камеру
        Camera.main.transform.position = Camera.main.GetComponent<LerpMove>().hidePosition;
        CameraController = Camera.main.GetComponent<CameraController>();
        CameraController.SetState(2);
        //
        Assembled = GameObject.FindGameObjectWithTag("Assembled");
        Assembled.transform.localPosition = new Vector3(0, 20, 11);
        Assembled.GetComponent<LerpMove>().Action("show");

        currentTime = Time.time;
    }

    void Update()
    {
        if (Time.time - currentTime > 1.7f && !showStartButton)
        {
            showStartButton = true;
            //StartButton.SetActive(true);
            tb = StartButton.GetComponent<TransparentBehaviour>();
            tb.Action("show");
        }
    }

    public void StartGame()
    {
        Camera.main.GetComponent<LerpMove>().Action("show");
        //CameraController.SetState(1);
        //
        tb = StartButton.GetComponent<TransparentBehaviour>();
        tb.Action("hide");
        //
        Assembled.GetComponent<LerpMove>().Action("hide");
        //
        PauseButton.GetComponent<LerpMove>().Action("show");
        UndoButton.GetComponent<LerpMove>().Action("show");
    }
}
