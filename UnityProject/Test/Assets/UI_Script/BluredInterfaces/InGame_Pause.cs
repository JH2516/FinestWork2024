using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_Pause : MonoBehaviour, IAfterBlurBack
{
    public StageManager stageManager;

    public void AfterBackCapture()
    {
        stageManager.Button_Pause();
    }
}
