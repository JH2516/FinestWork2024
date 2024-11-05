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

    private bool            isCollapsed;



    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_CollapseRoom");
        base.Awake();

        obj_Collapse.SetActive(false);
        text_Collapse = show_Interaction.transform.Find("Text_Interaction").GetComponent<TextMeshProUGUI>();
        text_Collapse.text = $"Collapse\n{time_Collapse:N2}";

        if (time_Collapse == 0) time_Collapse = 10;

        isCollapsed = false;
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
        Debug.Log(time_Collapse);
        Debug.Log(time_Collapse <= 0f);

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
    }

    public override void Show_Interact()
    {
        base.Show_Interact();
        isPlayerInside = true;
    }

    public override void Hide_Interact()
    {
        base.Hide_Interact();
        isPlayerInside = false;
    }
}
