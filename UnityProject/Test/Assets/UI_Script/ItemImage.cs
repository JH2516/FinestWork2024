using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemImage : MonoBehaviour
{
    public float imageSizer;
    Image currentImage;
    RectTransform rect;
    Sprite[] sprites;

    private void Awake()
    {
        currentImage = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ItemList");
        sprites = new Sprite[objects.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = objects[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
        }
    }

    public void ChangeItemImage(int number)
    {
        Sprite sprite = sprites[sprites.Length - number - 1];
        currentImage.sprite = sprite;
        rect.transform.localScale = new Vector3(sprite.bounds.size.x * imageSizer, sprite.bounds.size.y * imageSizer);
    }
}
