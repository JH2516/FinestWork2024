using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Survivor : Interactor
{
    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_Survivor");
        base.Awake();

        if (stageManager.used_AirMat)
        {
            gameObject.SetActive(false);
            return;
        }
        stageManager.Count_Survivors();
    }

    public override void Done_Interact()
    {
        stageManager.Save_Survivor();
        base.Done_Interact();
    }
}
