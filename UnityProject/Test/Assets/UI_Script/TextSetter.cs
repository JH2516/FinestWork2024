using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextSetter : MonoBehaviour
{

    private void Start()
    {
        TMP_FontAsset font = Resources.Load<TMP_FontAsset>("Text/NeoDunggeunmoPro-Regular SDF");
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].font = font;
        }
    }
}
