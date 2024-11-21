using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TapTextSparkling : MonoBehaviour
{
    public float sparkleSpeed = 1.0f;
    bool isFade = true;
    float progress;
    public TextMeshProUGUI sparkleText;

    private void Update()
    {
        progress += Time.deltaTime;
        if (isFade)
        {
            sparkleText.color = new Color(0.1953125f, 0.1953125f, 0.1953125f, 1 - progress / sparkleSpeed);
            if (progress >= sparkleSpeed)
            {
                isFade = false;
                progress = 0;
            }
        }
        else
        {
            sparkleText.color = new Color(0.1953125f, 0.1953125f, 0.1953125f, progress / sparkleSpeed);
            if (progress >= sparkleSpeed)
            {
                isFade = true;
                progress = 0;
            }
        }
    }
}
