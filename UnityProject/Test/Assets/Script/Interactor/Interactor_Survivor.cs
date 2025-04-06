using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Survivor : Interactor
{
    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_Survivor");
        base.Awake();
        EventManager.instance.AddListener(this, PlayerEventType.b_Save);
    }

    private void Start()
    {
        EventManager.instance.TriggerEvent(PlayerEventType.s_Summon, this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.instance.RemoveListener(this, PlayerEventType.b_Save);
    }

    public override void Start_Interact()
    {
        base.Start_Interact();
        EventManager.instance.TriggerEvent(PlayerEventType.s_Saved, this, false);
        player.isSavingSurvivor = true;
    }

    public override void Done_Interact()
    {
        EventManager.instance.TriggerEvent(PlayerEventType.s_Saved, this, true);
        player.isSavingSurvivor = false;
        base.Done_Interact();
    }

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.b_Save:
                gameObject.SetActive(false);
                return true;

            default:
                if ((sender as Interactor_Survivor) == this)
                    return base.OnEvent(e_Type, sender, args);

                return false;
        }
    }
}
