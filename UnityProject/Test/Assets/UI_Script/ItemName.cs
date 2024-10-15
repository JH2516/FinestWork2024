using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemName : MonoBehaviour
{
    TextMeshProUGUI name;
    private void Awake()
    {
        name = GetComponent<TextMeshProUGUI>();
    }

    public void changeItemName(string itemName)
    {
        name.text = itemName;
    }
}
