using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IEventListener
{
    public  SO_Fire             sO_Fire;
    public  SController_Fire    sController_Fire { get; private set; }

    public  StageManager    stageManager { get; private set; }

    [Header("Fire Animation")]
    [SerializeField]
    private Animator        animator;
    [SerializeField]
    private SpriteRenderer  sprite;

    [Header("Fire Sprite Mask")]
    public  SpriteMask      mask;

    [Header("Fire Light power")]
    [SerializeField]
    private float           powerUp;
    [SerializeField]
    private float           powerDown;

    [Header("Fire Extinguish")]
    [SerializeField]
    public  bool            isExtinguish;

    [Header("Fire In BackDraft Room")]
    public  bool            isBackdraft = false;

    public  float           power       { get; private set; }
    public  float           maxPower    { get; private set; }

    public  bool            debug;

    private PlayerEventType[] listenerTypes =
    {
        PlayerEventType.p_StartAttack, PlayerEventType.p_EndAttack, PlayerEventType.Debug_Fire
    };

    private void Awake()
    {
        Init_Fire();
    }

    private void Start()
    {
        EventAdd();
    }

    public void Init_Fire()
    {
        sController_Fire = new SController_Fire(this);
        sController_Fire.Initialize(sController_Fire.state_Idle);

        stageManager        = GameObject.Find("StageManager").GetComponent<StageManager>();

        if (isBackdraft) return;

        power       = 
        maxPower    = sO_Fire.randomPower;
        transform.localScale = Vector2.one * maxPower;

        powerUp     = sO_Fire.power_Increase;
        powerDown   = sO_Fire.power_Decrease;

        animator.speed      = sO_Fire.randomSpeed;
        sprite.sortingOrder = 0;

        debug = false;
    }

    public void Set_OrderInLayer(int orderNum = 0)
    {
        sprite.sortingOrder     = orderNum;
        mask.frontSortingOrder  = orderNum;
        mask.backSortingOrder   = orderNum - 1;
    }


    public void Set_Power(float power) => this.power = power;

    private void Update()
    {
        sController_Fire.Update();
    }

    





    //--------------------이벤트 관련련--------------------//

    /// <summary>
    /// 이벤트 등록
    /// </summary>
    public void EventAdd()
    {
        
        EventManager.instance.AddListener(this, listenerTypes);
        //stageManager.player.PlayerAttackStart  += StartExtinguished;
        //stageManager.player.PlayerAttackEnd    += EndExtinguished;
    }
    /// <summary>
    /// 이벤트 등록 해제
    /// </summary>
    public void EventRemove()
    {
        EventManager.instance.RemoveListener(this, listenerTypes);
        //stageManager.player.PlayerAttackStart  -= StartExtinguished;
        //stageManager.player.PlayerAttackEnd    -= EndExtinguished;
        
    }

    /// <summary>
    /// 이벤트 : 플레이어가 소화기 사용 시 진화 여부 검사
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fire"></param>
    private void StartExtinguished(object obj, Fire fire)
    {
        if (!isExtinguish && fire == this)
            sController_Fire.ChangeState(sController_Fire.state_Extinguish);
    }

    /// <summary>
    /// 이벤트 : 플레이어가 소화기 사용 마무리 시
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    private void EndExtinguished(object obj, EventArgs e)
    {
        if (isExtinguish)
            sController_Fire.ChangeState(sController_Fire.state_Restore);
    }

    private void OnDestroy() => EventRemove();


    public bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.p_StartAttack when args as Fire == this:
                if (!isExtinguish)
                    sController_Fire.ChangeState(sController_Fire.state_Extinguish);
                return true;

            case PlayerEventType.p_EndAttack:
                if (isExtinguish)
                    sController_Fire.ChangeState(sController_Fire.state_Restore);
                return true;

            case PlayerEventType.Debug_Fire:
                debug = (bool)args;
                sController_Fire.ChangeState(sController_Fire.state_Dead);
                return true;
        }

        return false;
    }
}
