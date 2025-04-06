using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(-70)]
public class Player : MonoBehaviour, IEventListener
{
    public  static  Player  player;
    public  SController_Player sController_Player { get; private set; }

    public  AudioManager    audio;

    [Header("Component")]
    public  Camera          mainCamera;
    public  StageManager    stageManager;
    public  SpriteRenderer  sr;

    [Header("PlayerFOV")]
    public  GameObject      obj_FOV;
    public  GameObject      obj_Attack;

    [Header("Light")]
    public  Light2D         light_PlayerAround;
    public  Light2D         light_PlayerFOV;
    public  float           change_FOVAngleRange;


    [Header("Player State")]
    public  bool            isMove;
    public  bool            isButtonDownMove;
    public  bool            isFire;
    public  bool            isCanMove;
    public  bool            isSavingSurvivor;
    public  bool            isDetectedFire;
    public  bool            isInteract;
    public  bool            isRemoveCollapse;
    public  bool            isInFrontOfDoor;
    public  bool            isGameOver;
    public  bool            isBoostLight;

    [Header("Player Fire")]
    public  SpriteRenderer  sr_FireExtinguisher;
    public  float           time_Fire;
    public  bool            button_isFireActive;



    //[Header("Player Transform")]
    public  SO_Player       sO_Player;

    [Header("Player Transform")]
    [SerializeField]
    private Quaternion      setRotate;
    [SerializeField]
    private Interactor      target_Interactor;
    public  Vector2         setMoveVec          { get; private set; }

    [Header("Darkness")]
    [Range(0, 1), SerializeField]
    private float           range_Darkness;
    [Range(0, 1), SerializeField]
    private float           range_DarknessInRoom;

    [Header("Detect Fire")]
    [SerializeField]
    private Color           Color_nonDetected = new Color(0, 1, 0, 0.5f);
    [SerializeField]
    private Color           Color_Detected = new Color(1, 0, 0, 0.5f);
    [Range(0, 1), SerializeField]
    private float           Range_Alpha = 0.5f;


    [Range(0, 90)]
    public  float           Range_halfAngle = 20;
    [Range(0, 20)]
    public  float           Range_radius = 5;

    [Header("Effect Collapse")]
    public  bool            warning_Collapse;
    public  bool            detect_Collapse;

    [Header("Item Status")]
    public  bool            using_CollapseAlarm;
    public  bool            using_PortableLift;
    public  bool            using_PistolNozzle;

    
    public  LayerMask       layer_Fire;

    private List<Interactor>        List_Interactor;

    [Header("Navigate CollapseRoom")]
    public  GameObject      navigator_CollapseRoom;
    public  Transform       transform_CollapseRoom;

    [Header("Target Interactor")]
    public  GameObject      target_Collapse;
    public  GameObject      target_BackDraft;

    [Header("Icons")]
    public  GameObject      icon_PistolNozzle;
    public  GameObject      icon_PortableLift;

    private PlayerEventType[] listenerTypes =
    {
        PlayerEventType.b_Light,
        PlayerEventType.s_Saved,
        PlayerEventType.d_Collapse, PlayerEventType.e_CollapseSoon, PlayerEventType.e_CollapseDone,
        PlayerEventType.i_UseItem1, PlayerEventType.i_UseItem2, PlayerEventType.i_UseItem3,
        PlayerEventType.Try_UseItem1, PlayerEventType.Try_UseItem2, PlayerEventType.Try_UseItem3
    };

    private byte count_Interact;

    private Coroutine   co_Navigate;
    private Coroutine   co_Notificate;
    private Coroutine   co_Warning;

    private void Awake()
    {
        if (player == null) player = this;

        Init_Argument();
        Init_Conmpnent();
        Init_Light();
        Init_List();
    }

    

    private void Init_Conmpnent()
    {
        mainCamera = Camera.main;
        

        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        navigator_CollapseRoom.SetActive(false);
        icon_PistolNozzle.SetActive(false);
        icon_PortableLift.SetActive(false);

        

        layer_Fire = LayerMask.GetMask("Fire");
    }

    private void Init_List()
    {
        List_Interactor = new List<Interactor>();
    }

    private void Init_Argument()
    {
        isFire = false;
        isCanMove = true;
        isButtonDownMove = false;
        isInteract = false;
        isSavingSurvivor = false;
        warning_Collapse = false;
        button_isFireActive = false;
        count_Interact = 0;
        time_Fire = 0;
        change_FOVAngleRange = 2f;
        isRemoveCollapse = false;
        isInFrontOfDoor = false;
        isBoostLight = false;

        using_CollapseAlarm = false;
        using_PortableLift = false;
        using_PistolNozzle = false;
    }

    private void Init_Light()
    {
        
    }

    private void Reset()
    {
        Awake();

        Color_nonDetected = new Color(0, 1, 0, 0.5f);
        Color_Detected = new Color(1, 0, 0, 0.5f);

        isDetectedFire = false;
    }

    private void Start()
    {
        stageManager = StageManager.stageManager;

        sController_Player = new SController_Player(this);
        sController_Player.Initialize(sController_Player.state_Idle);

        //EventManager.instance.AddListener(PlayerEventType.d_Collapse, this);
        //EventManager.instance.AddListener(PlayerEventType.e_CollapseSoon, this);
        //EventManager.instance.AddListener(PlayerEventType.e_CollapseDone, this);
        EventManager.instance.AddListener(this, listenerTypes);
        sO_Player.SetLight_DarkedRoom(light_PlayerAround, light_PlayerFOV, false);
    }

    private void Update()
    {
        sController_Player?.Update();
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveListener(this, listenerTypes);
    }

    /// <summary>
    /// Coroutine 실행 중지 및 참조 해제
    /// </summary>
    /// <param name="coroutine">종료 및 참조를 해제할 Coroutine<param>
    private void StopAndRemoveCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    /// <summary>
    /// 붕괴 위험 알림 - 카메라 떨림 효과
    /// </summary>
    private IEnumerator Warning_Collapse()
    {
        WaitForFixedUpdate wf = new WaitForFixedUpdate();

        while (true)
        {
            Vector3 effect = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), -10);
            mainCamera.transform.position = transform.position + effect;

            yield return wf;
        }
    }

    /// <summary>
    /// 인근 붕괴물 감지 - 일정 시간동안 약한 카메라 떨림 효과
    /// </summary>
    private IEnumerator Notificate_Collapse(float setTime)
    {
        WaitForFixedUpdate wf = new WaitForFixedUpdate();
        float time = 0;

        while(time < setTime)
        {
            time += Time.deltaTime;

            Vector3 effect = new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), UnityEngine.Random.Range(-0.05f, 0.05f), -10);
            mainCamera.transform.position = transform.position + effect;

            yield return wf;
        }
    }

    /// <summary>
    /// 붕괴 위험 장소 안내 - 해당 위치로 네비게이트
    /// </summary>
    private IEnumerator Navigate_CollapseRoom(Transform target = null)
    {
        WaitForFixedUpdate wf = new WaitForFixedUpdate();

        while (true)
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            navigator_CollapseRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            yield return wf;
        }
    }

    /// <summary>
    /// 붕괴물 경보기 내비게이트 활성화 여부
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    /// <param name="target">네이게이트 대상 Transform</param>
    private void SetActive_NavigateToCollapseRoom(bool isActive, Transform target = null)
    {
        using_CollapseAlarm = isActive;
        navigator_CollapseRoom.SetActive(isActive);

        switch (isActive)
        {
            case true when target != null:
                co_Navigate = StartCoroutine(Navigate_CollapseRoom(target)); break;
            case false:
                StopAndRemoveCoroutine(co_Navigate); break;
        }
    }

    /// <summary>
    /// 붕괴물 제거 작업 활성화 여부
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActive_RemoveCollapse(bool isActive)
    {
        isRemoveCollapse = isActive;
    }

    /// <summary>
    /// 휴대용 리프트 활성화 여부
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActive_UsingPortableLift(bool isActive)
    {
        using_PortableLift = isActive;
        icon_PortableLift.SetActive(isActive);
    }

    /// <summary>
    /// 플레이어가 문 앞에 위치하는 지 여부
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActive_InFrontOfDoor(bool isActive)
    {
        isInFrontOfDoor = isActive;
    }

    /// <summary>
    /// 휴대용 리프트 활성화 여부
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActive_UsingPistolNozzle(bool isActive)
    {
        using_PistolNozzle = isActive;
        icon_PistolNozzle.SetActive(isActive);
    }


    

    public void Button_CollapseAlarm()
    {
        // Item_CollapseAlarm 수신
        EventManager.instance.TriggerEventForOneListener(PlayerEventType.p_UseItem1, this);
    }

    public void Button_PortableLift()
    {
        // Item_PortableLift 수신
        EventManager.instance.TriggerEventForOneListener(PlayerEventType.p_UseItem2, this, target_Collapse);
    }
    public void Button_PistolNozzle()
    {
        // Item_PistolNozzle 수신
        EventManager.instance.TriggerEventForOneListener(PlayerEventType.p_UseItem3, this);


            target_BackDraft.
            GetComponent<Interactor_BackDraft>().Start_Interact();
    }


    /// <summary> 플레이어 회전 </summary>
    public void Player_Rotate()
    {
        if (!isMove) return;
        obj_FOV.transform.rotation = setRotate;
        obj_Attack.transform.rotation = setRotate;
    }

    public void Player_Flip(bool isFlip)
    {
        if (!isMove) return;

        sr.flipX =
        sr_FireExtinguisher.flipX =
        sr_FireExtinguisher.flipY = isFlip;
    }

    public void Player_KnockOut()
    {
        sController_Player.ChangeState(sController_Player.state_Dead);
    }

    /// <summary> 플레이어 이동 벡터 지정 </summary>
    public void Set_MoveVec(int type) => setMoveVec = sO_Player.moveVecs[type];

    /// <summary> 플레이어 회전 지정 </summary>
    public void Set_Rotation(int type) => setRotate = sO_Player.rotations[type];

    /// <summary> 플레이어 이동 여부 지정 </summary>
    public void Set_isMove(bool move)
    {
        isButtonDownMove = move;
        if (!isCanMove) return;

        isMove = move;
        audio.PlayerWalk(move);

        sController_Player.ChangeState
            (move ? sController_Player.state_Walk : sController_Player.state_Idle);
    }

    /// <summary>
    /// 플레이어 진화 작업 개시
    /// </summary>
    public void StartAttack()
    {
        // OverlapBox 영역 관련 계산
        Vector2 fov = obj_Attack.transform.right;
        Vector2 boxCenter = (Vector2)transform.position + fov * 2.5f;
        Vector2 boxSize = new Vector2(5, 2.5f);

        // OverlapBox 회전 각도 계산
        float angle = Mathf.Atan2(fov.y, fov.x) * Mathf.Rad2Deg;

        // OverlapBox 호출
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, layer_Fire);

        foreach (var item in hits)
        {
            // 감지 대상 오브젝트의 비활성화 시 작업 미수행
            if (!item.gameObject.activeSelf) continue;

            // Fire 송신
            EventManager.instance.
                TriggerEventForOneListener(PlayerEventType.p_StartAttack, this, item.GetComponent<Fire>());
        }
    }


    /// <summary>
    /// 플레이어 진화 작업 중지
    /// </summary>
    public void EndAttack()
    {
        // Fire 송신
        EventManager.instance.TriggerEvent(PlayerEventType.p_EndAttack, this);
    }


    /// <summary>
    /// Input System - 플레이어 공격 실행 (Left Shift)
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !isFire)
        {
            button_isFireActive = true;
            sController_Player.ChangeState(sController_Player.state_Attack);
            return;
        }

        if (context.canceled)
        {
            button_isFireActive = false;
        }
    }


    /// <summary>
    /// Button - 플레이어 공격 실행
    /// </summary>
    /// <param name="isButtonDown"> Button 누름 유무 </param>
    public void OnAttack(bool isButtonDown)
    {
        if (isButtonDown)
        {
            button_isFireActive = true;
            sController_Player.ChangeState(sController_Player.state_Attack);
        }
        else
        {
            button_isFireActive = false;
        }
    }

    /// <summary>
    /// Input System - 플레이어 아이템 사용 실행 (Z, X, C)
    /// </summary>
    /// <param name="context"></param>
    public void OnUseItem(InputAction.CallbackContext context)
    {
        //Debug.Log(context.control.path);
        char key = context.control.path[10];

        switch (key)
        {
            case 'z': Button_CollapseAlarm(); return;
            case 'x': Button_PortableLift(); return;
            case 'c': Button_PistolNozzle(); return;
        }
    }

    /// <summary>
    /// Input System - 플레이어 상호작용 실행 (Q)
    /// </summary>
    /// <param name="context"></param>
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Interact 송신
        if (context.started && isInteract)
            foreach (Interactor interactor in List_Interactor)
                EventManager.instance.TriggerEvent(PlayerEventType.p_Interact, interactor);
    }

    /// <summary>
    /// Button - 플레이어 상호작용 실행
    /// </summary>
    public void OnInteract()
    {
        // Interact 송신
        if (isInteract)
            foreach (Interactor interactor in List_Interactor)
                EventManager.instance.TriggerEvent(PlayerEventType.p_Interact, interactor);
    }

    

    




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DarkedRoom"))
        {
            //InDarkedRoom(collision); return;
        }

        if (collision.CompareTag("Interactor"))
        {
            InInteract(collision); return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DarkedRoom"))
        {
            //OutDarkedRoom(collision); return;
        }

        if (collision.CompareTag("Interactor"))
        {
            OutInteract(collision); return;
        }
    }

    public bool OnEvent(PlayerEventType e_Type, Component sender, object args = null)
    {
        switch (e_Type)
        {
            case PlayerEventType.d_Collapse when sender is Interactor_Collapse:
                Interactor_Collapse collapse = sender as Interactor_Collapse;

                if (!collapse.isPlayerDetect)
                    OnDetected_Collapse(out collapse.isPlayerDetect);

                return true;

            case PlayerEventType.d_DarkedRoom:
                OnDetected_DarkedRoom((bool)args);
                return true;

            case PlayerEventType.e_CollapseSoon:
                OnActivate_CollapseSoon();
                return true;

            case PlayerEventType.e_CollapseDone:
                OnActivate_CollapseDone();
                return true;

            case PlayerEventType.s_Saved when (bool)args:
                isSavingSurvivor = false;
                return true;

            case PlayerEventType.s_Saved when !(bool)args:
                isSavingSurvivor = true;
                return true;

            case PlayerEventType.Try_UseItem1:
                if ((bool)args)
                    transform_CollapseRoom = sender.gameObject.transform;
                SetActive_NavigateToCollapseRoom((bool)args);
                return true;

            case PlayerEventType.Try_UseItem2:
                SetActive_UsingPortableLift((bool)args);
                return true;

            case PlayerEventType.Try_UseItem3:
                SetActive_UsingPistolNozzle((bool)args);
                return true;

            case PlayerEventType.b_Light:
                isBoostLight = true;
                sO_Player.SetLight_DarkedRoom(light_PlayerAround, light_PlayerFOV, false, isBoostLight);
                return true;
        }

        return false;
    }

    /// <summary>
    /// 플레이어 주변 초기 붕괴물 감지 및 효과 활성화
    /// </summary>
    /// <param name="isPlayerDetected"> 붕괴물의 플레이어 감지 유무 갱신 </param>
    private void OnDetected_Collapse(out bool isPlayerDetected)
    {
        isPlayerDetected = true;
        StopAndRemoveCoroutine(co_Notificate);
        co_Notificate = StartCoroutine(Notificate_Collapse(5f));
    }

    /// <summary>
    /// 플레이어의 안개가 가득 찬 방 출입 감지 및 효과 활성화
    /// </summary>
    /// <param name="isPlayerInRoom"> 플레이어의 방 입장 유무 (퇴장 시 false) </param>
    private void OnDetected_DarkedRoom(bool isPlayerInRoom)
    {
        sO_Player.SetLight_DarkedRoom(light_PlayerAround, light_PlayerFOV, isPlayerInRoom, isBoostLight);
    }


    /// <summary>
    /// 붕괴 진행 효과 활성화
    /// </summary>
    private void OnActivate_CollapseSoon()
    {
        StopAndRemoveCoroutine(co_Warning);
        co_Warning = StartCoroutine(Warning_Collapse());
    }


    /// <summary>
    /// 붕괴 진행 효과 비활성화
    /// </summary>
    private void OnActivate_CollapseDone()
    {
        StopAndRemoveCoroutine(co_Warning);
        SetActive_NavigateToCollapseRoom(false);
    }


    /// <summary>
    /// 플레이어의 상호작용 대상 접근
    /// </summary>
    /// <param name="interact"> 상호대상의 Collider2D </param>
    private void InInteract(Collider2D interact)
    {
        Interactor obj = interact.GetComponent<Interactor>();

        // Interact, StageManager 송신
        EventManager.instance.TriggerEvent
            (PlayerEventType.d_Interact, obj, true);

        List_Interactor.Add(obj);

        count_Interact++;
        isInteract = true;
    }

    /// <summary>
    /// 플레이어의 상호작용 대상 탈출
    /// </summary>
    /// <param name="interact"> 상호대상의 Collider2D </param>\
    private void OutInteract(Collider2D interact)
    {
        Interactor obj = interact.GetComponent<Interactor>();

        EventManager.instance.TriggerEvent
            (PlayerEventType.d_Interact, obj, false);

        List_Interactor.Remove(obj);

        count_Interact--;
        if (count_Interact == 0) isInteract = false;
    }







    /*
    void DebugDrawBox(Vector2 center, Vector2 size, float angle, Color color)
    {
        // 박스의 네 꼭짓점 계산
        Vector2 halfSize = size * 0.5f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Vector3로 변환 후 Quaternion 회전 적용
        Vector2 topRight = (Vector2)(rotation * new Vector3(halfSize.x, halfSize.y, 0)) + center;
        Vector2 topLeft = (Vector2)(rotation * new Vector3(-halfSize.x, halfSize.y, 0)) + center;
        Vector2 bottomRight = (Vector2)(rotation * new Vector3(halfSize.x, -halfSize.y, 0)) + center;
        Vector2 bottomLeft = (Vector2)(rotation * new Vector3(-halfSize.x, -halfSize.y, 0)) + center;

        // 박스 외곽선 그리기
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);
    }
    */

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = (isFire) ? Color_Detected : Color_nonDetected;

        Handles.DrawSolidArc(transform.position, transform.forward, obj_FOV.transform.right, Range_halfAngle, Range_radius);
        Handles.DrawSolidArc(transform.position, transform.forward, obj_FOV.transform.right, -Range_halfAngle, Range_radius);
    }
#endif
}
