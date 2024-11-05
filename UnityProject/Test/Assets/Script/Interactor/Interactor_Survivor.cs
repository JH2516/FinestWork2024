using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Survivor : Interactor
{
    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_Survivor");
        base.Awake();
    }
}
