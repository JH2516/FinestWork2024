using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Exit : MonoBehaviour, IAfterBlurBack
{
    public GameObject exit;
    public void AfterBackCapture()
    {
        exit.SetActive(true);
    }
}
