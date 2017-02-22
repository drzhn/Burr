using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class SceneUI : MonoBehaviour
{
    CameraController CameraController;
    LerpMove PausePanel, ExitButton, ContinueButton, objAssembled, HelpView, HelpButton;
    GameObject PauseButton, UndoButton;
    TransparentBehaviour background;
    
    Stack<State> actionStack;
    Queue<State> stateQueue;

    //что показывать: скролл хэлпа или собранную модель
    bool helpOrAssembled = false;
    void Start()
    {
        objAssembled = GameObject.FindGameObjectWithTag("Assembled").GetComponent<LerpMove>();
        PausePanel = GameObject.Find("PausePanel").GetComponent<LerpMove>();
        ExitButton = GameObject.Find("Exit").GetComponent<LerpMove>();
        ContinueButton = GameObject.Find("Continue").GetComponent<LerpMove>();
        HelpView = GameObject.Find("Help View").GetComponent<LerpMove>();
        HelpButton = GameObject.Find("Help").GetComponent<LerpMove>();

        background = GameObject.Find("BackGround").GetComponent<TransparentBehaviour>();
        PauseButton = GameObject.Find("Pause");
        UndoButton = GameObject.Find("Undo");

        CameraController = Camera.main.GetComponent<CameraController>();

        actionStack = GameObject.Find("GameManager").GetComponent<GameManager>().actionStack;
        stateQueue = GameObject.Find("GameManager").GetComponent<GameManager>().stateQueue;


    }
    public void Undo()
    {
        Debug.Log("Undo");
        Debug.Log(actionStack.Count);
        if (actionStack.Count == 0) return;
        State state, currentState;
        do
        {
            state = actionStack.Pop();
            currentState = new State(GameObject.Find(state.name));
        }
        while (currentState == state && actionStack.Count > 0);
        stateQueue.Enqueue(state);
    }

    public void Pause()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().isPaused = true;
        CameraController.SetState(2);
        PausePanel.Action("show");
        ExitButton.Action("show");
        ContinueButton.Action("show");
        objAssembled.Action("show");
        background.gameObject.SetActive(true);
        background.Action("show");
        HelpButton.Action("show");
        PauseButton.SetActive(false);
        UndoButton.SetActive(false);
    }
    public void Continue()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().isPaused = false;
        CameraController.SetState(1);
        PausePanel.Action("hide");
        ExitButton.Action("hide");
        ContinueButton.Action("hide");
        objAssembled.Action("hide");
        background.Action("hide");
        HelpButton.Action("hide");
        HelpView.Action("hide");
        PauseButton.SetActive(true);
        UndoButton.SetActive(true);

        helpOrAssembled = false;
    }

    public void StartGame()
    {
        Debug.Log("Start");
        background.gameObject.SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().isPaused = false;
        GetComponent<StartBehaviour>().StartGame();
    }
    public void EndLevel()
    {
        //скрываем все элементы управления
        PausePanel.Action("hide");
        ExitButton.Action("hide");
        ContinueButton.Action("hide");
        objAssembled.Action("hide");
        background.Action("hide");
        UndoButton.GetComponent<LerpMove>().Action("hide");
        PauseButton.GetComponent<LerpMove>().Action("hide");
        //отключаем камеру
        CameraController.SetState(0);
        //
        //код для сохранения результата
        GameObject.Find("GameManager").GetComponent<GameManager>().Save();
        //
        // удаляем все точки
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        foreach (var p in points)
        {
            Destroy(p);
        }
        //Запускаем камеру вращаться
        GetComponent<EndBehaviour>().End();
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Help()
    {
        if (!helpOrAssembled)
        {
            objAssembled.Action("hide");
            HelpView.Action("show");
        }else
        {
            objAssembled.Action("show");
            HelpView.Action("hide");
        }
        helpOrAssembled = !helpOrAssembled;
    }
}
