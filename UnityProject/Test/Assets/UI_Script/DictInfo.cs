using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DictInfo : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public GameObject image;
    public float imageSizer;
    private RectTransform imageRect;
    private Image imageComponent;

    private void Awake()
    {
        imageRect = image.GetComponent<RectTransform>();
        imageComponent = image.GetComponent<Image>();
    }

    public void ChangeInfo(ItemData item)
    {
        itemName.text = item.ItemName;
        description.text = item.ItemDesciption;
        imageComponent.sprite = item.sprite;
        imageComponent.color = Color.white;
        imageRect.transform.localScale = imageSizer * item.size * new Vector3(item.sprite.bounds.size.x, item.sprite.bounds.size.y);
    }
}
