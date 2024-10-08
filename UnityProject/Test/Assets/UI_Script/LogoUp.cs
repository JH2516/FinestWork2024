using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoUp : MonoBehaviour
{
    public GameObject frontUI;
    public float upDistant;
    public float upAccel;
    public float upTime;
    RectTransform rect;
    float progress;
    Vector3 originPositon;
    public FadeInBack screenFader;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originPositon = rect.localPosition;
    }

    void Update()
    {
        progress += Time.deltaTime;
        rect.localPosition = new Vector3(originPositon.x, originPositon.y + upDistant * Mathf.Pow(progress / upTime, upAccel), 0);
        if (progress >= upTime)
        {
            screenFader.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
