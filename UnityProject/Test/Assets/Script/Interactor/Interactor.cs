using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    // Setting Component
    [Header("UI")]
    [SerializeField]
    protected   GameObject      prefab_Interaction;

    [Header("Time")]
    [SerializeField]
    private     float           timeSec_Interaction;


    // Get Component
    protected   StageManager    stageManager;
    protected   UIInteract      show_Interaction;
    private     Transform       parent_Interaction;


    // Check
    protected   bool            isInteraction;


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

    protected void Init_UIInteraction(string namePrefab)
    {
        if (prefab_Interaction == null)
        prefab_Interaction = Resources.Load<GameObject>($"Prefab/{namePrefab}");
    }

    protected virtual void OnEnable()
    {
        isInteraction = false;
    }

    public virtual void Show_Interact()
    {
        isInteraction = false;

        show_Interaction.Set_Interactor(gameObject);
        show_Interaction.Set_Position(transform.position);
        show_Interaction.gameObject.SetActive(true);
    }

    public virtual void Hide_Interact()
    {
        show_Interaction.gameObject.SetActive(false);
    }

    public virtual void Start_Interact()
    {
        if (isInteraction) return;

        show_Interaction.Request_Start();
        isInteraction = true;
    }

    public virtual void Done_Interact()
    {
        gameObject.SetActive(false);
    }
}
