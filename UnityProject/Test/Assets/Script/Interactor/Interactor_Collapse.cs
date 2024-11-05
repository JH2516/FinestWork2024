using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactor_Collapse : Interactor
{
    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_Collapse");
        base.Awake();
    }
}
