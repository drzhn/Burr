using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour
{

    float CurrentTime;
    bool isStarted = false;
    string level;
    void Update()
    {
        if (Time.time - CurrentTime > 1f && isStarted)
        {
            GetComponent<SaveManager>().LoadLevel(level);
            isStarted = false;
        }
    }

    public void _StartLevel(string _level)
    {
        level = _level;
        GameObject.Find("BlackoutPanel").GetComponent<TransparentBehaviour>().Action("show");
        GetComponent<SaveManager>().LoadLevel(level);
        CurrentTime = Time.time;
        isStarted = true;
    }
}
