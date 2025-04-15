using System.Collections;
using UnityEngine;

public class Interactor_Collapse : Interactor
{
    public  bool        isPlayerDetect;

    private LayerMask   layer_Player;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    protected override void Awake()
    {
        Init_UIInteract(InteractorType.Collapse);
        base.Awake();

        isPlayerDetect = false;
        layer_Player = LayerMask.GetMask("Player");

        AddEvent(this, PlayerEventType.i_UseItem2);
        StartCoroutine(CheckPlayer());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        RemoveEvent(this, PlayerEventType.i_UseItem2);
    }





    //-----------------< Interact. 상호작용 모음 >-----------------//

    protected override void Show_Interact()
    {
        base.Show_Interact();
    }

    protected override void Hide_Interact()
    {
        base.Hide_Interact();
        TriggerEvent(PlayerEventType.UI_UseItem2, this, false);
        TriggerEvent(PlayerEventType.Try_UseItem2, this, false);

        audio.RemoveCollapse(false);
    }

    public override void Start_Interact()
    {
        base.Start_Interact();
        TriggerEvent(PlayerEventType.UI_UseItem2, this, true);
        TriggerEvent(PlayerEventType.a_UseItem2, this, true);
    }

    public override void Done_Interact()
    {
        base.Done_Interact();
        TriggerEvent(PlayerEventType.UI_UseItem2, this, false);
        TriggerEvent(PlayerEventType.Try_UseItem2, this, false);

        audio.RemoveCollapse(true);
        audio.UsePortableLift(false);
    }





    //-----------------< Activity. 활동 모음 >-----------------//

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
                TriggerEvent(PlayerEventType.d_Collapse, this);
                yield break;
            }
            yield return wf;
        }
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.i_UseItem2 when (args as GameObject) == gameObject && isInteraction:
                UIInteraction.Set_GuageAmountUpPerSecond(4f);
                return true;

            default:
                if ((sender as Interactor_Collapse) == this)
                    return base.OnEvent(e_Type, sender, args);

                return false;
        }
    }
}
