using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstScreenSkipper : MonoBehaviour
{
    public GameObject firstScreen;
    public static bool FirstScreenNoSkip { get; set; } = true;

    private void Awake()
    {
        firstScreen.SetActive(FirstScreenNoSkip);
    }
}
