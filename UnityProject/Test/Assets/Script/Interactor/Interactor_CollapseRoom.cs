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

    private bool            isCollapsed;

    public float            Time_Collapse => time_Collapse;



    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_CollapseRoom");
        base.Awake();

        obj_Collapse.SetActive(false);
        text_Collapse = show_Interaction.transform.Find("Text_Interaction").GetComponent<TextMeshProUGUI>();
        text_Collapse.text = $"Collapse\n{time_Collapse:N2}";

        if (time_Collapse == 0) time_Collapse = 10;

        isCollapsed = false;

        Invoke("Waiting_Collapse", time_Collapse - 5f);
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
        text_Collapse.text = $"Collapse\n{time_Collapse:N2}";
    }

    private void Check_Collapse()
    {
        if (time_Collapse <= 0f)
        Start_Collapse();
    }

    private void Start_Collapse()
    {
        obj_Collapse.SetActive(true);
        isCollapsed = true;

        if (isPlayerInside)
        stageManager.GameOver();

        text_Collapse.enabled = false;

        stageManager.player.warning_Collapse = false;
    }

    public override void Show_Interact()
    {
        isInteraction = false;

        show_Interaction.Set_Interactor(gameObject);
        show_Interaction.Set_Position(pos_UIInteract.position);
        show_Interaction.gameObject.SetActive(true);

        isPlayerInside = true;
    }

    public override void Hide_Interact()
    {
        base.Hide_Interact();
        isPlayerInside = false;
    }

    private void Waiting_Collapse()
    {
        stageManager.player.warning_Collapse = true;
    }
}
