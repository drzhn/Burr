using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonMove : MonoBehaviour {

    RectTransform rt;
    Button button;
    bool pressed = false;
    Vector3 startCusorPosition;
	void Start () {
        rt = GameObject.Find("Button").GetComponent<RectTransform>();
        button = GameObject.Find("Button").GetComponent<Button>();
	}
	
	void Update () {
        //Debug.Log(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
            pressed = true;
            startCusorPosition = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            pressed = false;
            button.interactable = true;
        }
        if (pressed)
        {
            if (Vector3.Distance(startCusorPosition, Input.mousePosition) > 3)
            {
                button.interactable = false;
            }
            rt.position = Input.mousePosition;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition)))
            {
                Debug.Log("ray");
            }
        }

	}

    public void PressButton()
    {
        Debug.Log("Pressed");
    }
}
