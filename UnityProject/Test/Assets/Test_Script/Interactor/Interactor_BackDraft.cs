using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_BackDraft : Interactor
{
    [SerializeField]
    private GameObject obj_Warning;
    [SerializeField]
    private GameObject pos_Warning;

    [SerializeField]
    private Transform pos_WarningUI;

    protected override void Awake()
    {
        base.Awake();
        show_Interaction.time_Interact = 1f;
        obj_Warning.SetActive(false);
        pos_Warning.SetActive(false);
    }

    private void OnEnable()
    {
        obj_Warning.SetActive(false);
        pos_Warning.SetActive(false);
    }

    private void Update()
    {
        if (!isInteraction) return;
        if (!pos_Warning.activeSelf) base.Done_Interact();
    }

    public override void Start_Interact()
    {
        base.Start_Interact();

        show_Interaction.Set_Position(pos_WarningUI.position);
        obj_Warning.SetActive(true);
        pos_Warning.SetActive(true);
    }

    public override void Done_Interact()
    {
        stageManager.GameOver_BackDraft();
        base.Done_Interact();
    }
}
