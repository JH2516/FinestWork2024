using System.Collections;
using System.Collections.Generic;
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

    public  GameObject      item_CollapseAlarm;

    [SerializeField]
    private TextMeshProUGUI text_RemainCollapse;

    public  bool            isSurvivorInRoom;
    private bool            isCollapsed;
    private bool            isUsedCollapseAlarm;

    public float            Time_Collapse => time_Collapse;



    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_CollapseRoom");
        base.Awake();

        obj_Collapse.SetActive(false);
        item_CollapseAlarm.SetActive(false);
        text_Collapse = show_Interaction.transform.Find("Text_Interaction").GetComponent<TextMeshProUGUI>();
        text_Collapse.text = $"Collapse\n{time_Collapse:N2}";

        if (time_Collapse == 0) time_Collapse = 10;

        isSurvivorInRoom = false;
        isCollapsed = false;
        isUsedCollapseAlarm = false;

        Invoke("Waiting_Collapse", time_Collapse - 5f);

        text_RemainCollapse = stageManager.text_CollapseRoomRemain;
    }

    private void Update()
    {
        if (isCollapsed) return;

        Timing();
        Check_Collapse();
    }

    private void Timing()
    {
        time_Collapse -= Time.deltaTime;
        text_Collapse.text = $"붕괴까지\n{(int)time_Collapse}초";

        if (isUsedCollapseAlarm)
        Timing_RemainCollapseRoom();
    }

    private void Timing_RemainCollapseRoom()
    {
        if (time_Collapse <= 5f)
        {
            text_RemainCollapse.color = Color.red;
            audio.AlertCollapseAlarm(true);
        }

        text_RemainCollapse.text = $"{time_Collapse:N2}";
    }

    public void SetActive_UseCollapseAlarm(bool isActive)
    {
        item_CollapseAlarm.SetActive(isActive);
        isUsedCollapseAlarm = isActive;
    }

    private void Check_Collapse()
    {
        if (time_Collapse <= 0f)
        Start_Collapse();
    }

    private void Start_Collapse()
    {
        audio.AlertCollapseAlarm(false);
        audio.StartCollapse(false);

        obj_Collapse.SetActive(true);
        isCollapsed = true;

        if (isPlayerInside)
        {
            stageManager.GameOver_CauseOfCollaspeRoom();
            audio.GameoverByCollapse(true);
        }
        else if (isSurvivorInRoom)
        {
            stageManager.GameOver_CauseOfFailedSaveSurvivor();
            audio.TriggerCollapse(true);
        }
        else
        {
            audio.TriggerCollapse(true);
        }

        text_Collapse.enabled = false;

        stageManager.player.warning_Collapse = false;

        player.InActive_NavigateToCollapseRoom();
        stageManager.SetActive_UIRemainCollapseRoom(false);
        SetActive_UseCollapseAlarm(false);
    }

    public override void Show_Interact()
    {
        if (!isInteraction)
        {
            StartCoroutine("FirstRemainTimeOfCollapse");
            isInteraction = true;
        }

        isPlayerInside = true;

        if (!player.using_CollapseAlarm)
        player.transform_CollapseRoom = transform;

        if (!stageManager.used_CollapseAlarm)
        {
            stageManager.UIButton_IsActiveItemCollapseAlarm(true);
        }
    }

    public override void Hide_Interact()
    {
        //base.Hide_Interact();
        isPlayerInside = false;

        if (!player.using_CollapseAlarm)
        stageManager.player.transform_CollapseRoom = null;

        if (!stageManager.used_CollapseAlarm)
        {
            stageManager.UIButton_IsActiveItemCollapseAlarm(false);
        }
    }

    private void Waiting_Collapse()
    {
        stageManager.player.warning_Collapse = true;
        audio.StartCollapse(true);
    }

    IEnumerator FirstRemainTimeOfCollapse()
    {
        show_Interaction.Set_Interactor(gameObject);
        show_Interaction.Set_Position(pos_UIInteract.position);
        show_Interaction.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        show_Interaction.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Survivor") isSurvivorInRoom = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Survivor") isSurvivorInRoom = false;
    }
}
