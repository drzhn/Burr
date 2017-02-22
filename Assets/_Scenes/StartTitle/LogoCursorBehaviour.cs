using UnityEngine;
using System.Collections;

public class LogoCursorBehaviour : MonoBehaviour {

    public float t = 0.4f;

    float CurrentTime;
    GameObject Cursor;
	void Start () {
        CurrentTime = Time.time;
        Cursor = GameObject.Find("Cursor");
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.time - CurrentTime > t)
        {
            CurrentTime = Time.time;
            Cursor.SetActive(!Cursor.activeSelf);
        }
	}
}
