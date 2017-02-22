using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartTitleBehaviour : MonoBehaviour
{

    public float stayTime = 4;

    TransparentBehaviour tr;
    Image im;
    GameObject backgroundImage;

    bool backgroundShow = false, loadLevel = false;
    void Start()
    {
        backgroundImage = GameObject.Find("Image");
        tr = GameObject.Find("Image").GetComponent<TransparentBehaviour>();
        im = GameObject.Find("Image").GetComponent<Image>();
        im.color = new Color(0, 0, 0, 1);
        tr.Action("hide");
    }

    void Update()
    {
        if (Time.time > stayTime - 1f && !backgroundShow)
        {
            backgroundImage.SetActive(true);
            tr.t = 7f;
            tr.Action("show");
            backgroundShow = true;
        }
        if (Time.time > stayTime && !loadLevel)
        {
            loadLevel = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
