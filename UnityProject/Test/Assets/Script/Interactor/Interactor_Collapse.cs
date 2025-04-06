using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactor_Collapse : Interactor
{
    public  bool isPlayerDetect;

    private LayerMask layer_Player;

    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_Collapse");
        base.Awake();

        isPlayerDetect = false;
        layer_Player = LayerMask.GetMask("Player");
        EventManager.instance.AddListener(this, PlayerEventType.i_UseItem2);
        StartCoroutine(CheckPlayer());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.instance.RemoveListener(this, PlayerEventType.i_UseItem2);
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
        EventManager.instance.TriggerEvent(PlayerEventType.UI_UseItem2, this, false);

        audio.RemoveCollapse(false);
    }

    public override void Start_Interact()
    {
        base.Start_Interact();
        player.SetActive_RemoveCollapse(true);
        EventManager.instance.TriggerEvent(PlayerEventType.UI_UseItem2, this, true);
    }

    public override void Done_Interact()
    {
        base.Done_Interact();
        player.SetActive_RemoveCollapse(false);
        player.SetActive_UsingPortableLift(false);
        EventManager.instance.TriggerEvent(PlayerEventType.UI_UseItem2, this, false);

        audio.RemoveCollapse(true);
        audio.UsePortableLift(false);
    }

    /// <summary>
    /// 붕괴물 범위 내 플레이어 출현 감지
    /// </summary>
    IEnumerator CheckPlayer()
    {
        WaitForFixedUpdate wf = new WaitForFixedUpdate();
        Collider2D[] hit = null;

        while (true)
        {
            hit = Physics2D.OverlapCircleAll(transform.position, 7f, layer_Player);
            if (hit.Length > 0)
            {
                EventManager.instance.TriggerEvent(PlayerEventType.d_Collapse, this);
                yield break;
            }
            yield return wf;
        }
    }

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.i_UseItem2 when (args as GameObject) == gameObject && isInteraction:
                UIInteraction.Modify_GuageAmountUpPerSecond(4f);
                return true;

            default:
                if ((sender as Interactor_Collapse) == this)
                    return base.OnEvent(e_Type, sender, args);

                return false;
        }
    }
}
