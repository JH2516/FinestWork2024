using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemImageAutoSizer : MonoBehaviour
{
    public float imageSizer = 0.25f;
    Image Image;
    RectTransform rect;

    private void Awake()
    {
        Image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        Sprite sprite = Image.sprite;
        rect.transform.localScale = new Vector3(sprite.bounds.size.x * imageSizer, sprite.bounds.size.y * imageSizer);
    }
}
