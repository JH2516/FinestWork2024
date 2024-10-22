using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationExiter : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
