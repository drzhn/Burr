using UnityEngine;
using System.Collections;

public class LerpMove : MonoBehaviour
{
    public bool isRectTransform = true;
    public Vector3 hidePosition;
    public Vector3 showPosition;
    public float t = 3.7f;
    public float minDistance = 1f;

    RectTransform rt;
    Vector3 target;
    bool isMoving;
    GameManager gm;

    CameraController CameraController;
    void Start()
    {
        try
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        catch { }
        CameraController = Camera.main.GetComponent<CameraController>();
        try
        {
            rt = gameObject.GetComponent<RectTransform>();
        }
        catch
        { }
    }

    void Update()
    {
        if (isMoving)
            if (!isRectTransform)
            {
                transform.localPosition = Move(transform.localPosition);
            }
            else
            {
                rt.anchoredPosition3D = Move(rt.anchoredPosition3D);
            }
    }

    Vector3 Move(Vector3 currentPosition)
    {
        Vector3 ret = Vector3.Lerp(currentPosition, target, t * Time.deltaTime);
        if (Vector3.Distance(currentPosition, target) <= minDistance)
        {
            isMoving = false;
            ret = target;
            try
            {
                gm.FinishMove(gameObject.name);
            }
            catch { }
        }
        return ret;
    }

    public void Action(string action)
    {
        if (action == "show")
        {
            target = showPosition;
        }
        else if (action == "hide")
        {
            target = hidePosition;
        }
        else return;
        isMoving = true;
    }
}

