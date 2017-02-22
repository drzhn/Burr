using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    GameObject TutorialPanel;
    GameObject TutorialBackground;
    GameObject BackGround;
    TutorialPanel tp;
    int okcount = 4;
    RectTransform tpRect;
    void Start()
    {
        TutorialPanel = GameObject.Find("TutorialPanel");
        tp = TutorialPanel.GetComponent<TutorialPanel>();
        TutorialPanel.SetActive(false);
        tpRect = TutorialPanel.GetComponent<RectTransform>();

        TutorialBackground = GameObject.Find("TutorialBackgound");
        TutorialBackground.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        TutorialBackground.SetActive(false);

        BackGround = GameObject.Find("BackGround");
    }
    public void OKbutton()
    {
        Debug.Log("OK Button");
        if (okcount > 0)
        {
            switch (okcount)
            {
                case 4:
                    {
                        TutorialPanel.SetActive(true);
                        TutorialBackground.SetActive(true);
                        TutorialBackground.GetComponent<TransparentBehaviour>().Action("show");
                        tp.SetText("UNDO");
                        tpRect.anchoredPosition3D = new Vector3(162.5f, -47.6f, 0);
                        break;
                    }
                case 3:
                    {
                        tpRect.anchorMax = new Vector2(0, 0);
                        tpRect.anchorMin = new Vector2(0, 0);
                        tpRect.anchoredPosition3D = new Vector3(162.5f, 47.6f, 0);

                        tp.SetText("PAUSE");
                        break;
                    }
                case 2:
                    {
                        GameObject.Find("GameManager").GetComponent<SceneUI>().Pause();
                        BackGround.SetActive(false);
                        tpRect.anchorMax = new Vector2(0, 0.5f);
                        tpRect.anchorMin = new Vector2(0, 0.5f);
                        tpRect.anchoredPosition3D = new Vector3(162.5f, 0, 0);
                        tp.SetText("HELP");
                        break;
                    }
                case 1:
                    {
                        tpRect.anchorMax = new Vector2(0, 0);
                        tpRect.anchorMin = new Vector2(0, 0);
                        tpRect.anchoredPosition3D = new Vector3(162.5f, 47.6f, 0);
                        tp.SetText("CONTINUE");
                        break;
                    }
            }
            okcount -= 1;
        }
        else
        {
            
            BackGround.SetActive(true);
            GameObject.Find("GameManager").GetComponent<SceneUI>().Continue();
            SetButtonsInteractible(true);
            TutorialBackground.GetComponent<TransparentBehaviour>().Action("hide");
            TutorialPanel.SetActive(false);
            StartGameAfterTutorial();
        }
    }

    public void StartGameAfterTutorial()
    {
        //запускаем игру
        GameObject.Find("GameManager").GetComponent<GameManager>().isPaused = false;
        Camera.main.GetComponent<CameraController>().SetState(1);
    }

    public void SetButtonsInteractible(bool value)
    {
        GameObject.Find("Exit").GetComponent<Button>().interactable = value;
        GameObject.Find("Continue").GetComponent<Button>().interactable = value;
        GameObject.Find("Help").GetComponent<Button>().interactable = value;
        GameObject.Find("Pause").GetComponent<Button>().interactable = value;
        GameObject.Find("Undo").GetComponent<Button>().interactable = value;
    }
}
