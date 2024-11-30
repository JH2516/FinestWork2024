using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstScreenSkipper : MonoBehaviour
{
    public GameObject firstScreen;

    public MaterialManager materialManager;
    public static bool FirstScreenNoSkip { get; set; } = true;

    private void Awake()
    {
        firstScreen.SetActive(FirstScreenNoSkip);
    }

    private void Start()
    {
        if (!FirstScreenNoSkip)
        {
            materialManager.OnMaterialWithTag("Main");
        }
    }
}
