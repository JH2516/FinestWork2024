using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Recovery : Interactor
{
    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_Recovery");
        base.Awake();
    }

    public override void Show_Interact()
    {
        base.Show_Interact();
        Start_Interact();

        stageManager.GameClear();
    }

    public override void Start_Interact()
    {
        if (isInteraction) return;
        
        show_Interaction.Request_Start(true, stageManager.Player_HP / stageManager.Player_HPMax * 100);
        isInteraction = true;
    }

    public override void Done_Interact()
    {
        show_Interaction.Init();
        isInteraction = false;

        stageManager.Complete_EscapeSurvivor();
    }
}
