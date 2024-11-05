using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Recovery : Interactor
{
    public override void Start_Interact()
    {
        if (isInteraction) return;

        show_Interaction.Request_Start(true, stageManager.Player_HP);
        isInteraction = true;
    }

    public override void Done_Interact()
    {
        show_Interaction.Init();
        isInteraction = false;
    }
}
