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

    public override void Show_Interact()
    {
        base.Show_Interact();
        player.target_Collapse = gameObject;
    }

    public override void Hide_Interact()
    {
        base.Hide_Interact();
        player.target_Collapse = null;
        player.SetActive_RemoveCollapse(false);
        player.SetActive_UsingPortableLift(false);

        audio.RemoveCollapse(false);
    }

    public override void Start_Interact()
    {
        base.Start_Interact();
        player.SetActive_RemoveCollapse(true);
    }

    public override void Done_Interact()
    {
        base.Done_Interact();
        player.SetActive_RemoveCollapse(false);
        player.SetActive_UsingPortableLift(false);

        audio.RemoveCollapse(true);
        audio.UsePortableLift(false);
    }
}
