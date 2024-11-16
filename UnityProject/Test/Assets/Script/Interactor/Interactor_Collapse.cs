using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactor_Collapse : Interactor
{
    public  bool isPlayerDetect;

    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_Collapse");
        base.Awake();

        isPlayerDetect = false;
    }
}
