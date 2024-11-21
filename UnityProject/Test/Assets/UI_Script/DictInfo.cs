using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        imageRect.transform.localScale = new Vector3(item.sprite.bounds.size.x * imageSizer, item.sprite.bounds.size.y * imageSizer);
    }
}
