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

    protected override void Awake()
    {
        Init_UIInteraction("UIInteract_BackDraft");
        Init_BackDraft();
        base.Awake();
    }

    private void OnEnable()
    {
        Init_BackDraft();
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
        if (!pos_Warning.activeSelf && !player.using_PistolNozzle) Break_BackDraft();
    }

    public override void Show_Interact()
    {
        base.Show_Interact();

        player.SetActive_InFrontOfDoor(true);
        player.target_BackDraft = gameObject;
    }

    public override void Hide_Interact()
    {
        base.Hide_Interact();

        player.SetActive_InFrontOfDoor(false);
        player.SetActive_UsingPistolNozzle(false);

        if (!player.using_PistolNozzle)
        player.target_BackDraft = null;
    }

    public override void Start_Interact()
    {
        if (player.using_PistolNozzle && !isInteraction)
        {
            Start_InteractWithOutWarning();
            base.Start_Interact();
            return;
        }

        if (!isInteraction)
        {
            base.Start_Interact();

            show_Interaction.Set_Position(pos_WarningUI.position);
            obj_Warning.SetActive(true);
            pos_Warning.SetActive(true);
            obj_Fires.SetActive(true);
            door.SetActive(false);
        }
    }

    public void Start_InteractWithOutWarning()
    {
        show_Interaction.Modify_GuageAmountUpPerSecond(3f);
        show_Interaction.Set_Position(door.transform.position);
    }

    public override void Done_Interact()
    {
        if (player.using_PistolNozzle)
        {
            Done_InteractWithOutWarning();
            return;
        }

        if (stageManager.IsGamePlay)
        {
            stageManager.GameOver();
            audio.GameoverByBackDraft(true);
        }
    }

    public void Done_InteractWithOutWarning()
    {
        door.SetActive(false);
        obj_Fires.SetActive(true);
        player.SetActive_UsingPistolNozzle(false);
    }

    public void Break_BackDraft()
    {
        Debug.Log("bbb");
        show_Interaction.gameObject.SetActive(false);
        obj_Warning.SetActive(false);
        trigger_Interact.enabled = false;
    }
}
