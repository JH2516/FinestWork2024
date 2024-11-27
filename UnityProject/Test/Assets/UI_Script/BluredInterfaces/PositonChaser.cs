using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositonChaser : MonoBehaviour
{
    public RectTransform chasing;
    RectTransform mySelf;

    private void Awake()
    {
        mySelf = GetComponent<RectTransform>();
    }

    private void Update()
    {
        mySelf.localPosition = chasing.localPosition;
    }
}
