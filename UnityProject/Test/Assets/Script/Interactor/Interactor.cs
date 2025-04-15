using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class Interactor : MonoBehaviour, IEventListener, IEventTrigger
{
    public      SO_Interactor   so_Interactor;

    // Setting Component
    [Header("UI")]
    [SerializeField]
    protected   UIInteract      prefab_Interaction;

    [Header("Time")]
    [SerializeField]
    private     float           timeSec_Interaction;


    // Get Component
    protected   StageManager    stageManager;
    protected   AudioManager    audio;
    //protected   Player          player;
    protected   UIInteract      show_Interaction;
    private     Transform       parent_Interaction;

    public      UIInteract      UIInteraction => show_Interaction;

    private PlayerEventType[] listenerTypes =
    {
        PlayerEventType.d_Interact, PlayerEventType.p_Interact
    };

    // Check
    protected   bool            isInteraction;



    

    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    protected virtual void Awake()
    {
        Init_Conmpnent();
        Init_Interact();
        AddEvent(this, listenerTypes);
    }

    protected virtual void OnEnable()
    {
        isInteraction = false;
    }

    protected virtual void OnDestroy()
    {
        RemoveEvent(this, listenerTypes);
    }





    //-----------------< Initialize. 초기화 모음 >-----------------//

    /// <summary>
    /// 초기화 - Conmpnent
    /// </summary>
    protected void Init_Conmpnent()
    {
        parent_Interaction = GameObject.Find("InGame_Canvas").transform.Find("Interactions");
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        //player = stageManager.player;
    }

    /// <summary>
    /// 초기화 - Interact
    /// </summary>
    protected virtual void Init_Interact()
    {
        show_Interaction =
        Instantiate(prefab_Interaction, parent_Interaction).GetComponent<UIInteract>();

        show_Interaction.gameObject.SetActive(false);
        show_Interaction.time_Interact = timeSec_Interaction;

        isInteraction = false;
    }

    /// <summary>
    /// 초기화 - UIInteract
    /// </summary>
    protected void Init_UIInteract(InteractorType type)
    {
        prefab_Interaction = so_Interactor.GetUIInteract(type);
    }





    //-----------------< Interact. 상호작용 모음 >-----------------//

    /// <summary>
    /// 안내문 텍스트 활성화
    /// </summary>
    protected virtual void Show_Interact()
    {
        isInteraction = false;

        show_Interaction.Set_Interactor(this);
        show_Interaction.Set_Position(transform.position);
        show_Interaction.gameObject.SetActive(true);
    }

    /// <summary>
    /// 안내문 텍스트 비활성화
    /// </summary>
    protected virtual void Hide_Interact()
    {
        show_Interaction.gameObject.SetActive(false);
    }

    /// <summary>
    /// 상호작용 시작
    /// </summary>
    public virtual void Start_Interact()
    {
        if (isInteraction) return;

        show_Interaction.Request_Start();
        isInteraction = true;
    }

    /// <summary>
    /// 상호작용 완료
    /// </summary>
    public virtual void Done_Interact()
    {
        gameObject.SetActive(false);
    }





    //-----------------< Event. 이벤트 모음 >-----------------//

    public virtual bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.d_Interact:
                if ((bool)args) Show_Interact();
                else            Hide_Interact();
                return true;

            case PlayerEventType.p_Interact:
                Start_Interact();
                return true;
        }

        return false;
    }

    public void AddEvent(IEventListener listener, params PlayerEventType[] types)
    => EventManager.instance.AddListener(listener, types);

    public void RemoveEvent(IEventListener listener, params PlayerEventType[] types)
    => EventManager.instance.RemoveListener(listener, types);

    public void TriggerEvent(PlayerEventType e_Type, Component sender, object args = null)
    => EventManager.instance.TriggerEvent(e_Type, sender, args);

    public void TriggerEventOneListener(PlayerEventType e_Type, Component sender, object args = null)
    => EventManager.instance.TriggerEventForOneListener(e_Type, sender, args);
}
