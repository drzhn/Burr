using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    Text PanelText;
    void Start()
    {
        PanelText = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetText(string text)
    {
        PanelText.text = text;
    }
    public void SetPosition(Vector3 newPosition)
    {

    }
}
