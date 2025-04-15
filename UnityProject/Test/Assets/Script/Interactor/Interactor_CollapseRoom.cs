using System.Collections;
using UnityEngine;
using TMPro;

public class Interactor_CollapseRoom : Interactor
{
    [SerializeField]
    private float           time_Collapse;
    [SerializeField]
    private TextMeshProUGUI text_Collapse;
    [SerializeField]
    private bool            isPlayerInside;
    [SerializeField]
    private GameObject      obj_Collapse;
    [SerializeField]
    private Transform       pos_UIInteract;

    public  Transform       item_CollapseAlarm;

    [SerializeField]
    private TextMeshProUGUI text_RemainCollapse;

    public  bool            isSurvivorInRoom;
    private bool            isCollapsed;
    private bool            isUsedCollapseAlarm;

    public float            Time_Collapse => time_Collapse;





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    protected override void Awake()
    {
        Init_UIInteract(InteractorType.CollaspeRoom);
        base.Awake();

        Init_CollapseRoom();
        AddEvent(this, PlayerEventType.i_UseItem1);
    }

    private void Start()
    {
        StartCoroutine(Timing());
    }

    protected override void OnDestroy()
    {
        RemoveEvent(this, PlayerEventType.i_UseItem1);
        base.OnDestroy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Survivor") isSurvivorInRoom = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Survivor") isSurvivorInRoom = false;
    }





    //-----------------< Initialize. 초기화 모음 >-----------------//

    private void Init_CollapseRoom()
    {
        obj_Collapse.SetActive(false);
        item_CollapseAlarm.gameObject.SetActive(false);
        text_Collapse = show_Interaction.transform.Find("Text_Interaction").GetComponent<TextMeshProUGUI>();
        text_Collapse.text = $"Collapse\n{time_Collapse:N2}";

        if (time_Collapse == 0) time_Collapse = 10;

        isSurvivorInRoom = false;
        isCollapsed = false;
        isUsedCollapseAlarm = false;

        Invoke("CollapseSoon", time_Collapse - 5f);

        text_RemainCollapse = UIManager.ui.text_CollapseRoomRemain;
    }



    //-----------------< Interact. 상호작용 모음 >-----------------//

    protected override void Show_Interact()
    {
        if (!isInteraction)
        {
            StartCoroutine(FirstRemainTimeOfCollapse());
            isInteraction = true;
        }

        isPlayerInside = true;

        stageManager.Button_ChangeToAttack();

        if (!isUsedCollapseAlarm)
        {
            TriggerEvent(PlayerEventType.UI_UseItem1, this, true);
            TriggerEvent(PlayerEventType.a_UseItem1, this, true);
        }
    }

    protected override void Hide_Interact()
    {
        isPlayerInside = false;

        if (!isUsedCollapseAlarm)
        {
            TriggerEvent(PlayerEventType.UI_UseItem1, this, false);
        }
    }





    //-----------------< Activity. 활동 모음 >-----------------//

    /// <summary>
    /// 붕괴까지 남은 시간 갱신
    /// </summary>
    private IEnumerator Timing()
    {
        while (true)
        {
            time_Collapse -= Time.deltaTime;
            text_Collapse.text = $"붕괴까지\n{(int)time_Collapse}초";

            text_RemainCollapse.text = $"{time_Collapse:N2}";

            if (time_Collapse <= 0f)
            {
                CollapseDone();
                yield break;
            }  

            yield return null;
        }
    }

    /// <summary>
    /// 플레이어의 붕괴 징조 초기 발견 시 3초간 붕괴 위험 안내
    /// </summary>
    IEnumerator FirstRemainTimeOfCollapse()
    {
        show_Interaction.Set_Interactor(this);
        show_Interaction.Set_Position(pos_UIInteract.position);
        show_Interaction.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        show_Interaction.gameObject.SetActive(false);
    }


    /// <summary>
    /// 알람 경보 작동 시작
    /// </summary>
    private void TriggerAlert()
    {
        text_RemainCollapse.color = Color.red;
        audio.AlertCollapseAlarm(true);
    }

    /// <summary>
    /// 붕괴 위험 효과 발동
    /// </summary>
    private void CollapseSoon()
    {
        TriggerEvent(PlayerEventType.e_CollapseSoon, this);
        audio.StartCollapse(true);
    }

    /// <summary>
    /// 붕괴 현상 발동
    /// </summary>
    private void CollapseDone()
    {
        TriggerEvent(PlayerEventType.e_CollapseDone, this);
        audio.AlertCollapseAlarm(false);
        audio.StartCollapse(false);

        obj_Collapse.SetActive(true);
        isCollapsed = true;

        if (isPlayerInside)
        {
            TriggerEvent(PlayerEventType.g_GameOver, this, GameOverType.CollaspeRoom);
            audio.GameoverByCollapse(true);
        }
        else if (isSurvivorInRoom)
        {
            TriggerEvent(PlayerEventType.g_GameOver, this, GameOverType.FailedSaveSurvivor);
            audio.TriggerCollapse(true);
        }
        else
        {
            audio.TriggerCollapse(true);
        }

        item_CollapseAlarm.gameObject.SetActive(false);
        text_Collapse.enabled = false;
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public override bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        if (isCollapsed) return false;

        switch (e_Type)
        {
            // Player
            case PlayerEventType.i_UseItem1:
                // StageManager 송신
                TriggerEvent(PlayerEventType.Try_UseItem1, item_CollapseAlarm, true);
                Invoke("TriggerAlert", time_Collapse - 5f);
                item_CollapseAlarm.gameObject.SetActive(true);
                isUsedCollapseAlarm = true;
                return true;

            default:
                if ((sender as Interactor_CollapseRoom) == this)
                    return base.OnEvent(e_Type, sender, args);
                break;
        }

        return false;
    }
}
