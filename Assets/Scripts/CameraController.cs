using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    //состояния контроллера камеры: 0 - нет контроллера, 1 - котроллер игры, 2 - контроллер паузы
    public int state;

    CameraGameController game;
    CameraPauseController pause;
	void Start () {
        game = GetComponent<CameraGameController>();
        pause = GetComponent<CameraPauseController>();
	}
	
	// Update is called once per frame
	void Update () {
	    switch (state)
        {
            case 0:
                //ничего не делать
                break;
            case 1:
                game.CameraUpdate();
                break;
            case 2:
                pause.CameraUpdate();
                break;
            default:
                break;
        }
	}
    
    public void SetState(int value)
    {
        state = value;
    }
}
