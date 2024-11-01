using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private GameObject      prefab_Interaction;
    [SerializeField]
    private Transform       parent_Interaction;

    [SerializeField]
    private Test_Player     obj_Player;
    [SerializeField]
    protected Test_Stage      stageManager;
    private float           Time_Interaction;
    protected bool isInteraction;

    protected UIInteract show_Interaction;

    protected virtual void Awake()
    {
        parent_Interaction = GameObject.Find("InGame_Canvas").transform.Find("Interactions");
        stageManager = GameObject.Find("StageManager").GetComponent<Test_Stage>();

        show_Interaction =
        Instantiate(prefab_Interaction, parent_Interaction).GetComponent<UIInteract>();
        show_Interaction.gameObject.SetActive(false);

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
