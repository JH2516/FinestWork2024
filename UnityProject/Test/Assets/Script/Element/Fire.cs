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





    //-----------------< MonoBehaviour. 게임 루프 >-----------------//

    private void Awake()        => Init_Fire();
    private void Start()        => AddEvent(this, listenerTypes);
    private void Update()       => sController_Fire?.Update();
    private void OnDestroy()    => RemoveEvent(this, listenerTypes);





    //-----------------< Initialize. 초기화 모음 >-----------------//

    /// <summary>
    /// Fire 초기화
    /// </summary>
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





    //-----------------< Setting. 속성 설정 >-----------------//

    /// <summary>
    /// 불 Sprite Mask 정렬 속성 설정
    /// </summary>
    /// <param name="orderNum"> 정렬 번호 </param>
    public void Set_OrderInLayer(int orderNum = 0)
    {
        sprite.sortingOrder     = orderNum;
        mask.frontSortingOrder  = orderNum;
        mask.backSortingOrder   = orderNum - 1;
    }

    /// <summary>
    /// 불 화력(오브젝트 크기) 설정
    /// </summary>
    /// <param name="power"> 화력 값 </param>
    public void Set_Power(float power) => this.power = power;





    //-----------------< Event. 이벤트 모음 >-----------------//

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

    public void AddEvent(IEventListener listener, params PlayerEventType[] types)
    => EventManager.instance.AddListener(listener, types);

    public void RemoveEvent(IEventListener listener, params PlayerEventType[] types)
    => EventManager.instance.RemoveListener(listener, types);
}
