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
        stageManager.Count_Survivors(gameObject);
    }

    public override void Start_Interact()
    {
        base.Start_Interact();
        player.isSavingSurvivor = true;
    }

    public override void Done_Interact()
    {
        stageManager.Save_Survivor(gameObject);
        player.isSavingSurvivor = false;
        base.Done_Interact();
    }
}
