using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndBehaviour : MonoBehaviour
{
    public bool isEnded = false;
    Vector3 axis;
    GameObject ExitLevelButton;
    float currentTime;
    bool showEndButton = false;
    void Start()
    {
        //убираем кнопку завершения уровня
        ExitLevelButton = GameObject.Find("End");
        ExitLevelButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);
        ExitLevelButton.SetActive(false);
    }
    void Update()
    {
        if (isEnded)
        {
            axis = new Vector3(Mathf.Cos(Time.time), Mathf.Sin(Time.time));
            Camera.main.transform.RotateAround(Vector3.zero, axis, 150f * Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.LookRotation(Vector3.zero - Camera.main.transform.position, Camera.main.transform.up);
        }
        if (Time.time - currentTime > 1.7f && showEndButton)
        {
            ExitLevelButton.GetComponent<TransparentBehaviour>().Action("show");
        }
    }

    public void End()
    {

        ExitLevelButton.SetActive(true);
        currentTime = Time.time;
        showEndButton = true;
        isEnded = true;
    }
}
