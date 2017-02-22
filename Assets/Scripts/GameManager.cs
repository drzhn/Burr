using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //каждое действие записывается как объект класса State в наш стек
    //при нажатии на undo, объект достается из стека
    public Stack<State> actionStack;
    //все state попадают в очередь обработки:
    //пока не будет обработан предыдущий объект (блок должен сообщить об этом)
    //следующее действие начинаться не будет
    public Queue<State> stateQueue;
    //свободно ли для выполнения?
    public bool free = true;

    //пауза ли сейчас
    public bool isPaused = true;

    //возможные значения координат объектов.
    public Vector3 size = new Vector3(3, 3, 3),
    center = new Vector3(1, 1, 1);
    public int unitSize = 12;

    public Positions pos;
    State oldState;

    GameObject Assembled;
    CameraController CameraController;
    void Start()
    {
        Assembled = GameObject.FindGameObjectWithTag("Assembled");
        actionStack = new Stack<State>();
        stateQueue = new Queue<State>();
        pos = new Positions(size, center, unitSize);
        CameraController = Camera.main.GetComponent<CameraController>();
    }
    void Update()
    {
        if (isPaused)
            Assembled.transform.rotation *= Quaternion.Euler(40f * Time.deltaTime, 40f * Time.deltaTime, 0f);
        if (stateQueue.Count != 0)
        {
            if (free)
            {
                free = false;
                State s = stateQueue.Dequeue();
                s.SetState();
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<SceneUI>().EndLevel();
        }
    }

    // данные о вращении и перемещении блоков попадают сюда
    //преобразуются в структуру save и отправляются в очередь выполнения, а затем в стек действий
    //тут же проходит проверка на завершение уровня.
    public void MoveObject(ref GameObject obj, Vector3 target)
    {
        if (target == Vector3.zero || (pos.ExsistKey(target) && !pos.TargetHasObject(target)))
        {
            State oldState = new State(obj);
            State newState = new State(obj.name, target, obj.transform.rotation);
            actionStack.Push(oldState);
            stateQueue.Enqueue(newState);
            return;
        }
    }
    public void RotateObject(ref GameObject obj, Quaternion target)
    {
        State newState = new State(obj.name, obj.transform.position, target);
        stateQueue.Enqueue(newState);
    }

    public void Save()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        int currentLevelProgress = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name);
        Debug.Log(currentLevelProgress + 1);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, currentLevelProgress + 1);
        PlayerPrefs.Save();

        //for (int i = 1; i <= 9; i++)
        //{
        //    int LevelValue = PlayerPrefs.GetInt("level" + i.ToString());
        //    Debug.Log("level" + i.ToString() + " - " + LevelValue.ToString());
        //}
    }

    //костыль: начать уровень тогда, когда камера доедет до своего начального положения
    public void FinishMove(string objectName)
    {
        if (objectName == "Main Camera")
        {
            if (SceneManager.GetActiveScene().name == "level1")
            {
                GameObject.Find("TutorialManager").GetComponent<Tutorial>().SetButtonsInteractible(false);
                GameObject.Find("TutorialManager").GetComponent<Tutorial>().OKbutton();
                MonoBehaviour sceneUI = GetComponent<SceneUI>();
                sceneUI.enabled = false;
            }
            else
            {
                isPaused = false;
                CameraController.SetState(1);
            }
        }
    }
}