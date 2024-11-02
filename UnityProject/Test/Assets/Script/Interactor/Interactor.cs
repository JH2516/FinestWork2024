using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Set_Interaction")]
    [SerializeField]
    private     GameObject      prefab_Interaction;
    [SerializeField]
    private     Transform       parent_Interaction;

    [Header("Set_Interaction")]
    [SerializeField]
    private     float           timeSec_Interaction;

    private     Player          obj_Player;
    protected   StageManager    stageManager;

    private     float           Time_Interaction;
    protected   bool            isInteraction;

    protected   UIInteract      show_Interaction;

    protected virtual void Awake()
    {
        Init_Conmpnent();
        Init_Interact();
    }

    protected void Init_Conmpnent()
    {
        parent_Interaction = GameObject.Find("InGame_Canvas").transform.Find("Interactions");
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    protected virtual void Init_Interact()
    {
        show_Interaction =
        Instantiate(prefab_Interaction, parent_Interaction).GetComponent<UIInteract>();
        show_Interaction.gameObject.SetActive(false);
        show_Interaction.time_Interact = timeSec_Interaction;

        isInteraction = false;
    }

    private void OnEnable()
    {
        isInteraction = false;
    }

    public void Show_Interact()
    {
        show_Interaction.Set_Interactor(this.gameObject);
        show_Interaction.Set_Position(transform.position);
        show_Interaction.gameObject.SetActive(true);
    }

    public void Hide_Interact()
    {
        show_Interaction.gameObject.SetActive(false);
    }

    public virtual void Start_Interact()
    {
        show_Interaction.Request_Start();
        isInteraction = true;
    }

    public virtual void Done_Interact()
    {
        gameObject.SetActive(false);
    }
}
