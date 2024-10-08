using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInBack : MonoBehaviour
{
    float progress;
    public float fadeInTime;
    public Image fader;
    public GameObject frontTitle;

    void Update()
    {
        progress += Time.deltaTime;
        fader.color = new Color(1, 1, 1, 1 - progress / fadeInTime);
        if (progress > fadeInTime)
        {
            frontTitle.SetActive(false);
        }
    }
}
