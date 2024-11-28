using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFixer : MonoBehaviour
{
    public ItemData item;

    private void Awake()
    {
        Image image = GetComponent<Image>();
        image.sprite = item.sprite;
        RectTransform rect = GetComponent<RectTransform>();
        rect.localScale = Vector3.one * item.size;
    }
}
