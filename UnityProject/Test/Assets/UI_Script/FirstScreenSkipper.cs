using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstScreenSkipper : MonoBehaviour
{
    public GameObject firstScreen;
    public static bool firstScreenNoSkip = true;
    private void Awake()
    {
        firstScreen.SetActive(firstScreenNoSkip);
    }
}
