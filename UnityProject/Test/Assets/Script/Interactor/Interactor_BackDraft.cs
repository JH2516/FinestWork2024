using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
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
    public  Light2D         light_InRoom;

    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_BackDraft");
        Init_BackDraft();
        base.Awake();
    }

    private void OnEnable()
    {
        Init_BackDraft();
        light_InRoom.intensity = 0.1f;
    }

    private void Init_BackDraft()
    {
        obj_Warning.SetActive(false);
        pos_Warning.SetActive(false);
        obj_Fires.SetActive(false);
        door.SetActive(true);
    }

    private void Update()
    {
        if (!isInteraction) return;
        if (!pos_Warning.activeSelf) Break_BackDraft();
    }

    public override void Start_Interact()
    {
        base.Start_Interact();

        show_Interaction.Set_Position(pos_WarningUI.position);
        obj_Warning.SetActive(true);
        pos_Warning.SetActive(true);
        obj_Fires.SetActive(true);
        door.SetActive(false);
        light_InRoom.intensity = 3f;
    }

    public override void Done_Interact()
    {
        stageManager.GameOver();
    }

    public void Break_BackDraft()
    {
        show_Interaction.gameObject.SetActive(false);
        obj_Warning.SetActive(false);
        trigger_Interact.enabled = false;
    }
}
