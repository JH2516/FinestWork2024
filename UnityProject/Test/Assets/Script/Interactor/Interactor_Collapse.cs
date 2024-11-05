using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactor_Collapse : Interactor
{
    [SerializeField]
    private float           time_Collapse;
    [SerializeField]
    private TextMeshProUGUI text_Collapse;
    [SerializeField]
    private bool            isPlayerInside;
    [SerializeField]
    private GameObject      obj_Collapse;



    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_Collapse");
        base.Awake();

        obj_Collapse.SetActive(false);
        text_Collapse.text = $"Collapse\v
    }

    private void Update()
    {
        Timing();
        Check_Collapse();
    }

    private void Timing()
    {
        time_Collapse -= Time.deltaTime;
    }

    private void Check_Collapse()
    {
        if (time_Collapse <= 0)
        Start_Collapse();
    }

    private void Start_Collapse()
    {
        obj_Collapse.SetActive(true);

        if (isPlayerInside)
        stageManager.GameOver();
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

    public override void Start_Interact()
    {
        
    }
}
