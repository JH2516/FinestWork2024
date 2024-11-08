using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SurviverUpdate : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetSurviverNumber(int num)
    {
        text.text = num.ToString();
    }
}
