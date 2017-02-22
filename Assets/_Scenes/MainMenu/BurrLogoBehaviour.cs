using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BurrLogoBehaviour : MonoBehaviour
{
    public float w = 0.4f;
    Image sr;
    void Start()
    {
        sr = GetComponent<Image>();
        //sr.color = new Color()
    }

    void Update()
    {
        float value = Time.time * w - Mathf.Round(Time.time * w);
        if (Mathf.RoundToInt(Time.time * w) % 2 == 0)
            value = -value;
        value = 2 * value;

        float r = (value * 50 + 205f) / 255f;
        float g = 109f / 255f;
        float b = (-value * 50f + 205f) / 255f;
        sr.color = new Color(r, g, b);
    }
}
