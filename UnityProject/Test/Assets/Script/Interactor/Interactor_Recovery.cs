using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Recovery : Interactor
{
    //protected override void Awake()
    //{
    //    base.Awake();
    //    show_Interaction.time_Interact = 2f;
    //}

    public override void Start_Interact()
    {
        show_Interaction.Request_Start(true, stageManager.Player_HP);
        isInteraction = true;
    }

    public override void Done_Interact()
    {
        show_Interaction.Init();
    }
}
