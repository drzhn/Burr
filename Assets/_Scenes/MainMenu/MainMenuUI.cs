using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuUI : MonoBehaviour
{

    LerpMove Levels, AboutView;
    GameObject AboutButtonImage, CloseButtonImage;
    GameObject AboutButton, CloseButton;
    GameObject BlackoutPanel;
    GameObject RUSurePanel;

    //float ScreenRatio;
    //float ScreenToWorldRatio; //отношение размеров экрана к разрешающей способности камеры
    //float ScreenToCanvasRatio; // отношение размеров экрана к разрешающей способности canvas
    float CameraWidth, height, WorldWidth, CanvasWidth;

    float CurrentTime;
    bool isStarted = false;
    string level;
    void Start()
    {
        //CameraWidth = Screen.width;
        //height = Screen.height;
        //ScreenRatio = CameraWidth / height;
        //float CameraSize = Camera.main.orthographicSize;
        //WorldWidth = CameraSize * ScreenRatio * 2f;
        //ScreenToWorldRatio = CameraWidth / WorldWidth;

        CanvasWidth = GameObject.Find("MainCanvas").GetComponent<CanvasScaler>().referenceResolution.x;

        Levels = GameObject.Find("Levels").GetComponent<LerpMove>();
        AboutView = GameObject.Find("AboutView").GetComponent<LerpMove>();
        AboutView.showPosition = new Vector3(CanvasWidth / 4f, 0, 0);


        AboutButtonImage = GameObject.Find("About");
        CloseButtonImage = GameObject.Find("Close");
        CloseButtonImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        CloseButtonImage.SetActive(false);

        AboutButton = GameObject.Find("AboutButton");
        CloseButton = GameObject.Find("CloseButton");
        CloseButton.SetActive(false);

        //поставим скролл-вью в нужное место
        AboutView.showPosition = new Vector3(CanvasWidth / 4, 0, 0);

        BlackoutPanel = GameObject.Find("BlackoutPanel");
        BlackoutPanel.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        BlackoutPanel.GetComponent<TransparentBehaviour>().Action("hide");

        RUSurePanel = GameObject.Find("RUSure");
    }
    public void About()
    {
        Levels.Action("hide");
        //Buttons.Action("hide");
        AboutView.Action("show");
        AboutButtonImage.GetComponent<TransparentBehaviour>().Action("hide");
        CloseButtonImage.SetActive(true);
        CloseButtonImage.GetComponent<TransparentBehaviour>().Action("show");
        CloseButton.SetActive(true);
        AboutButton.SetActive(false);

    }

    public void Close()
    {
        Levels.Action("show");
        //Buttons.Action("show");
        AboutView.Action("hide");
        CloseButtonImage.GetComponent<TransparentBehaviour>().Action("hide");
        AboutButtonImage.SetActive(true);
        AboutButtonImage.GetComponent<TransparentBehaviour>().Action("show");
        AboutButton.SetActive(true);
        CloseButton.SetActive(false);
    }
    public void ResetSaveProgress()
    {
        //GetComponent<SaveManager>().ResetSaveProgress();
        BlackoutPanel.SetActive(true);
        BlackoutPanel.GetComponent<TransparentBehaviour>().t = 5f;
        BlackoutPanel.GetComponent<TransparentBehaviour>().alpha = 214;
        BlackoutPanel.GetComponent<TransparentBehaviour>().Action("show");
        RUSurePanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 400, 0);
        RUSurePanel.GetComponent<LerpMove>().Action("show");
    }
    public void RuSureYES()
    {
        GetComponent<SaveManager>().ResetSaveProgress();
        BlackoutPanel.SetActive(true);
        BlackoutPanel.GetComponent<TransparentBehaviour>().Action("hide");
        RUSurePanel.GetComponent<LerpMove>().Action("hide");
    }
    public void RuSureNO()
    {
        BlackoutPanel.SetActive(true);
        BlackoutPanel.GetComponent<TransparentBehaviour>().Action("hide");
        RUSurePanel.GetComponent<LerpMove>().Action("hide");
    }

    void Update()
    {
        if (Time.time - CurrentTime > 0.5f && isStarted)
        {
            SceneManager.LoadScene(level);
            isStarted = false;
        }
    }

    public void _StartLevel(string _level)
    {
        level = _level;
        BlackoutPanel.SetActive(true);
        BlackoutPanel.GetComponent<TransparentBehaviour>().alpha = 255;
        GameObject.Find("BlackoutPanel").GetComponent<TransparentBehaviour>().t = 10;
        GameObject.Find("BlackoutPanel").GetComponent<TransparentBehaviour>().Action("show");
        CurrentTime = Time.time;
        isStarted = true;
    }
}
