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
        stageManager.button_PortableLift.color = new Color(1, 1, 1, 0.25f);

        audio.RemoveCollapse(false);
    }

    public override void Start_Interact()
    {
        base.Start_Interact();
        player.SetActive_RemoveCollapse(true);
        stageManager.button_PortableLift.color = new Color(1, 1, 1, 1);
    }

    public override void Done_Interact()
    {
        base.Done_Interact();
        player.SetActive_RemoveCollapse(false);
        player.SetActive_UsingPortableLift(false);
        stageManager.button_PortableLift.color = new Color(1, 1, 1, 0.25f);

        audio.RemoveCollapse(true);
        audio.UsePortableLift(false);
    }
}
