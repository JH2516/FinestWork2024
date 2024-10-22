using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemImage : MonoBehaviour
{
    public float imageSizer;
    Image currentImage;
    RectTransform rect;

    private void Awake()
    {
        currentImage = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void ChangeItemImage(Image image)
    {
        Sprite sprite = image.sprite;
        currentImage.sprite = sprite;
        rect.transform.localScale = new Vector3(sprite.bounds.size.x * imageSizer, sprite.bounds.size.y * imageSizer);
    }
}
