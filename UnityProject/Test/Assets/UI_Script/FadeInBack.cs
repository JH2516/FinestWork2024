using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInBack : MonoBehaviour
{
    float progress;
    public float fadeInTime;
    public float fadeInAccel;
    public Image fader;
    public GameObject frontTitle;
    public MaterialManager materialManager;

    private void Start()
    {
        materialManager.OnMaterialWithTag("Main");
    }

    void Update()
    {
        progress += Time.deltaTime;
        fader.color = new Color(1, 1, 1, 1 - Mathf.Pow(progress / fadeInTime, fadeInAccel));
        if (progress > fadeInTime)
        {
            FirstScreenSkipper.FirstScreenNoSkip = false;
            frontTitle.SetActive(false);
        }
    }
}
