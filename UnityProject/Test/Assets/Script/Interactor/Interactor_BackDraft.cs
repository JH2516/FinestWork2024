using System.Collections;
using UnityEngine;

public class Interactor_BackDraft : Interactor
{
    [Header("BackDraft")]
    [SerializeField]
    private GameObject      obj_Warning;
    [SerializeField]
    private GameObject      pos_Warning;
    [SerializeField]
    private Transform       pos_WarningUI;
    [SerializeField]
    private BoxCollider2D   trigger_Interact;

    [Header("Fire InRoom")]
    [SerializeField]
    private GameObject      obj_Fires;

    [Header("Room Option")]
    public  GameObject      door;

    private bool            isPlayerUseItem;
    private bool            isBreakWarning;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    protected override void Awake()
    {
        Init_UIInteract(InteractorType.BackDraft);
        Init_BackDraft();
        base.Awake();

        AddEvent(this, PlayerEventType.Try_UseItem3);
    }

    protected override void OnEnable()
    {
        Init_BackDraft();
    }

    protected override void OnDestroy()
    {
        RemoveEvent(this, PlayerEventType.Try_UseItem3);
        base.OnDestroy();
    }





    //-----------------< Initialize. 초기화 모음 >-----------------//

    private void Init_BackDraft()
    {
        obj_Warning.SetActive(false);
        pos_Warning.SetActive(false);
        obj_Fires.SetActive(false);
        door.SetActive(true);
        isPlayerUseItem = false;
        isBreakWarning = false;
    }





    //-----------------< Interact. 상호작용 모음 >-----------------//

    protected override void Show_Interact()
    {
        base.Show_Interact();
        TriggerEvent(PlayerEventType.UI_UseItem3, this, true);
    }

    protected override void Hide_Interact()
    {
        base.Hide_Interact();
        TriggerEvent(PlayerEventType.UI_UseItem3, this, false);
    }

    public override void Start_Interact()
    {
        TriggerEvent(PlayerEventType.UI_UseItem3, this, false);
        base.Start_Interact();
    }

    /// <summary>
    /// Interact 시작 - Backdraft 위험 출현
    /// </summary>
    public void Start_InteractWithWarning()
    {
        Start_Interact();

        show_Interaction.Set_Position(pos_WarningUI.position);
        stageManager.Button_ChangeToAttack();
        obj_Warning.SetActive(true);
        pos_Warning.SetActive(true);
        obj_Fires.SetActive(true);
        door.SetActive(false);

        StartCoroutine(CheckWarning());
    }

    /// <summary>
    /// Interact 시작 - Backdraft 위험 없음
    /// </summary>
    public void Start_InteractWithoutWarning()
    {
        isPlayerUseItem = true;

        show_Interaction.Set_GuageAmountUpPerSecond(3f);
        show_Interaction.Set_Position(door.transform.position);
        Start_Interact();
    }

    public override void Done_Interact()
    {
        StopCoroutine(CheckWarning());

        // Backdraft 진압 및 PistolNozzle 사용 후
        if (isBreakWarning || isPlayerUseItem)
        {
            Done_InteractWithOutWarning();
        }

        // 플레이어가 Backdraft 미진압 시 게임 오버
        else
        {
            TriggerEvent(PlayerEventType.g_GameOver, this, GameOverType.BackDraft);
            audio.GameoverByBackDraft(true);
        }

        base.Done_Interact();
    }

    /// <summary>
    /// Interact 완료 - Backdraft 위험 없음
    /// </summary>
    public void Done_InteractWithOutWarning()
    {
        door.SetActive(false);
        obj_Fires.SetActive(true);
        TriggerEvent(PlayerEventType.Try_UseItem3, this, false);
    }





    //-----------------< Activity. 활동 모음 >-----------------//

    /// <summary>
    /// Backdraft 진행 중 플레이어의 진압 여부 검사
    /// </summary>
    private IEnumerator CheckWarning()
    {
        while (true)
        {
            if (!pos_Warning.activeSelf)
            {
                Break_BackDraft();
                Done_Interact();
                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 플레이어로부터 Backdraft 진압
    /// </summary>
    public void Break_BackDraft()
    {
        isBreakWarning = true;
        show_Interaction.gameObject.SetActive(false);
        obj_Warning.SetActive(false);
        trigger_Interact.enabled = false;
        isInteraction = false;

        Debug.Log("Break_BackDraft");
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.Try_UseItem3:
                Start_InteractWithoutWarning();
                return true;

            case PlayerEventType.p_Interact:
                Start_InteractWithWarning();
                return true;

            default:
                if ((sender as Interactor_BackDraft) == this)
                    return base.OnEvent(e_Type, sender, args);

                return false;
        }
    }
}
