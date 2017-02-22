using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TransparentBehaviour : MonoBehaviour
{

    public int alpha = 0;
    public float t = 1f;

    int target;
    bool isBlackout = false;
    Image im;
    void Start()
    {
        im = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlackout)
        {
            im.color = new Color(
                im.color.r,
                im.color.g,
                im.color.b,
                Mathf.Lerp(255f * im.color.a, target, t * Time.deltaTime) / 255f);
            if (Mathf.Abs(255f * im.color.a - target) <= 3)
            {
                isBlackout = false;
                im.color = new Color(
                    im.color.r,
                    im.color.g,
                    im.color.b,
                    target / 255f);
                //убираем объект только тогда, когда он исчезает
                if (target == 0)
                    gameObject.SetActive(false);
            }
        }
    }
    public void Action(string action)
    {
        if (action == "show")
        {
            target = alpha;
        }
        else if (action == "hide")
        {
            target = 0;
        }
        else return;
        isBlackout = true;
    }
}
