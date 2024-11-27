using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEditor;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    public  static  Player  player;

    private AudioManager    audio;

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
    public  bool            isFire;
    public  bool            isSavingSurvivor;
    public  bool            isDetectedFire;
    public  bool            isInteract;
    public  bool            isRemoveCollapse;
    public  bool            isInFrontOfDoor;

    [Header("Player Fire")]
    public  SpriteRenderer  sr_FireExtinguisher;
    public  float           time_Fire;
    public  bool            button_isFireActive;



    //[Header("Player Transform")]
    private Vector2[]       moveVec;
    private Quaternion[]    rotate;

    [Header("Player Transform")]
    public  Vector2         setMoveVec;
    public  Quaternion      setRotate;

    private Interactor      target_Interactor;

    [Header("Darkness")]
    [Range(0, 1), SerializeField]
    private float           range_Darkness;
    [Range(0, 1), SerializeField]
    private float           range_DarknessInRoom;

    [Header("Detect Fire")]
    public  Color           Color_nonDetected = new Color(0, 1, 0, 0.5f);
    public  Color           Color_Detected = new Color(1, 0, 0, 0.5f);
    [Range(0, 1)]
    [SerializeField]
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

    

    private LayerMask       layer_Collapse;
    private LayerMask       layer_Fire;
    private LayerMask       layer_Interaction;

    private List<Fire>              List_FireInFOV;
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



    private byte count_Interact;


    private void Awake()
    {
        if (player == null) player = this;

        Init_Argument();
        Init_Conmpnent();
        Init_Layer();
        Init_Light();
        Init_List();
        Init_Transform();
    }

    private void Init_Conmpnent()
    {
        mainCamera = Camera.main;
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        sr = GetComponent<SpriteRenderer>();

        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        navigator_CollapseRoom.SetActive(false);
        icon_PistolNozzle.SetActive(false);
        icon_PortableLift.SetActive(false);
    }

    private void Init_List()
    {
        List_FireInFOV = new List<Fire>();
        List_Interactor = new List<Interactor>();
    }

    private void Init_Layer()
    {
        layer_Collapse = LayerMask.GetMask("Collapse");
        layer_Fire = LayerMask.GetMask("Fire");
        layer_Interaction = LayerMask.GetMask("Interaction");
    }

    private void Init_Transform()
    {
        moveVec = Init_PlayerTransform.Get_MoveVecs();
        rotate = Init_PlayerTransform.Get_Rotation();
    }

    private void Init_Argument()
    {
        isFire = false;
        isInteract = false;
        isSavingSurvivor = false;
        warning_Collapse = false;
        button_isFireActive = false;
        count_Interact = 0;
        time_Fire = 0;
        change_FOVAngleRange = 2f;
        isRemoveCollapse = false;
        isInFrontOfDoor = false;

        using_CollapseAlarm = false;
        using_PortableLift = false;
        using_PistolNozzle = false;
    }

    private void Init_Light()
    {
        Set_LightAroundIntensity(0.5f);
        Set_LightAroundRadius(2f, 5f);
    }

    private void Reset()
    {
        Awake();

        Color_nonDetected = new Color(0, 1, 0, 0.5f);
        Color_Detected = new Color(1, 0, 0, 0.5f);

        isDetectedFire = false;
    }

    private void Update()
    {
        if (stageManager.IsGameOver) return;

        Get_Input();
        Timing();

        Player_Move();
        Player_Rotate();
        Player_Flip();

        //Player_Attack();

        //Check_Fire();
        //Check_Collapse();
    }

    private void FixedUpdate()
    {
        if (stageManager.IsGameOver) return;

        Player_Attack();
        Check_Fire();
        Check_Collapse();

        Warning_Collapse();
        Detect_FirstCollapse();
        Navigate_CollapseRoom();
    }


    /// <summary> 붕괴 위험, 화면 흔들림 출현 </summary>
    public void Warning_Collapse()
    {
        if (!warning_Collapse) return;

        Vector3 effect = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), -10);
        mainCamera.transform.position = transform.position + effect;
    }

    /// <summary> 초기 붕괴물 근처 감지 시 화면 흔들림 출현 </summary>
    public void Detect_FirstCollapse()
    {
        if (!detect_Collapse) return;

        Vector3 effect = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), -10);
        mainCamera.transform.position = transform.position + effect;
    }

    /// <summary> 붕괴물 경보기 사용 시 내비게이트 기능 출현 </summary>
    public void Navigate_CollapseRoom()
    {
        if (!using_CollapseAlarm) return;

        Vector3 direction = transform_CollapseRoom.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        navigator_CollapseRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    //public void SetTarget_CollapseRoomForNavigate(Transform collapseRoom)
    //=> transform_CollapseRoom = collapseRoom;

    /// <summary> 붕괴물 경보기 내비게이트 활성화 </summary>
    public void Active_NavigateToCollapseRoom()
    {
        navigator_CollapseRoom.SetActive(true);
        using_CollapseAlarm = true;
    }

    /// <summary> 붕괴물 경보기 내비게이트 비활성화 </summary>
    public void InActive_NavigateToCollapseRoom()
    {
        navigator_CollapseRoom.SetActive(false);
        using_CollapseAlarm = false;
    }

    /// <summary> 붕괴물 제거 작업 활성화 여부 </summary>
    public void SetActive_RemoveCollapse(bool isActive)
    {
        isRemoveCollapse = isActive;
    }

    /// <summary> 휴대용 리프트 활성화 여부 </summary>
    public void SetActive_UsingPortableLift(bool isActive)
    {
        using_PortableLift = isActive;
        icon_PortableLift.SetActive(isActive);
    }

    /// <summary> 플레이어가 문 앞에 위치하는 지 여부 </summary>
    public void SetActive_InFrontOfDoor(bool isActive)
    {
        isInFrontOfDoor = isActive;
    }

    /// <summary> 휴대용 리프트 활성화 여부 </summary>
    public void SetActive_UsingPistolNozzle(bool isActive)
    {
        using_PistolNozzle = isActive;
        icon_PistolNozzle.SetActive(isActive);
    }


    /// <summary> 입력 키 검사 </summary>
    void Get_Input()
    {
        if (Input.GetKey(KeyCode.A) && !isFire)
        {
            Button_PlayerAttackActive();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            Button_PlayerAttackInActive();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Button_PlayerInteract();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Button_CollapseAlarm();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Button_PortableLift();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Button_PistolNozzle();
        }
    }

    public void Button_PlayerAttackActive()
    {
        isFire = true;
        button_isFireActive = true;
        audio.Extinguising(true);
    }

    public void Button_PlayerInteract()
    {
        if (!isInteract) return;

        foreach (var interactor in List_Interactor)
        interactor.Start_Interact();
    }

    public void Button_PlayerAttackInActive()
    {
        button_isFireActive = false;
    }

    public void Button_CollapseAlarm()
    {
        if (stageManager.UseItem_CollapseAlarm())
        {
            transform_CollapseRoom.
            GetComponent<Interactor_CollapseRoom>().SetActive_UseCollapseAlarm(true);
        }
    }
    public void Button_PortableLift()
    {
        if (stageManager.UseItem_PortableLift())
            target_Collapse.
            GetComponent<Interactor_Collapse>().UIInteraction.Modify_GuageAmountUpPerSecond(4f);
    }
    public void Button_PistolNozzle()
    {
        if (stageManager.UseItem_PistolNozzle())
            target_BackDraft.
            GetComponent<Interactor_BackDraft>().Start_Interact();
    }

    /// <summary> 플레이어 이동 </summary>
    void Player_Move()
    {
        if (!isMove) return;
        if (isFire) return;
        if (using_PistolNozzle) return;
        if (isSavingSurvivor) return;
        transform.Translate(setMoveVec * 3f * Time.deltaTime);
    }

    /// <summary> 플레이어 회전 </summary>
    void Player_Rotate()
    {
        if (!isMove) return;
        obj_FOV.transform.rotation = setRotate;
        obj_Attack.transform.rotation = setRotate;
    }

    /// <summary> 플레이어 소화기 분사 </summary>
    void Player_Attack()
    {
        if (!isFire)
        {
            obj_Attack.SetActive(false);
            audio.Extinguising(false);
            return;
        }
        else
        {
            obj_Attack.SetActive(true);

            foreach (Fire item in List_FireInFOV)
            {
                if (item.gameObject.activeSelf == false) continue;
                item.isExtinguish = true;

                if (item.GetComponent<Fire>().isBackdraft)
                item.gameObject.SetActive(false);
            }
        }
    }

    /// <summary> 시간 갱신 </summary>
    void Timing()
    {
        if (time_Fire > 1f && !button_isFireActive)
        {
            time_Fire = 0f;
            isFire = false;

            foreach (Fire item in List_FireInFOV)
            {
                item.isExtinguish = false;
            }
        }

        if (isFire)
        {
            Timing_Fire(); return;
        }
    }

    /// <summary> 소화기 분사 시간 갱신 </summary>
    void Timing_Fire()
    {
        time_Fire += Time.deltaTime;
    }

    /// <summary> 플레이어 이미지 소스 좌우반전 </summary>
    void Player_Flip()
    {
        if (!isMove) return;

        if (setMoveVec.x < 0)
        {
            sr.flipX = true;
            sr_FireExtinguisher.flipX = true;
            sr_FireExtinguisher.flipY = true;
            return;
        }

        if (setMoveVec.x > 0)
        {
            sr.flipX = false;
            sr_FireExtinguisher.flipX = false;
            sr_FireExtinguisher.flipY = false;
            return;
        }
    }

    /// <summary> 플레이어 이동 벡터 지정 </summary>
    public void Set_MoveVec(int type) => setMoveVec = moveVec[type];

    /// <summary> 플레이어 회전 지정 </summary>
    public void Set_Rotation(int type) => setRotate = rotate[type];

    /// <summary> 플레이어 이동 여부 지정 </summary>
    public void Set_isMove(bool move)
    {
        isMove = move;
        audio.PlayerWalk(move);
    }

    void Check_Fire()
    {
        List<Fire> temp = List_FireInFOV.ToList();

        List_FireInFOV.Clear();

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, Range_radius, layer_Fire);

        if (hit.Length != 0 && !isDetectedFire)
        {
            isDetectedFire = true;
            stageManager.FadeInAudio_BurningAround();
        }

        if (hit.Length == 0 && isDetectedFire)
        {
            isDetectedFire = false;
            stageManager.FadeOutAudio_BurningAround();
        }

        Vector2 fov = obj_Attack.transform.right;

        // 방향에 따른 박스 중심 계산
        Vector2 boxCenter = (Vector2)transform.position + fov * 2.5f;
        Vector2 boxSize = new Vector2(5, 2.5f);

        // 박스의 회전 각도 계산
        float angle = Mathf.Atan2(fov.y, fov.x) * Mathf.Rad2Deg;

        // OverlapBox 호출
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, layer_Fire);

        // 감지된 오브젝트 처리
        foreach (var hitCollider in hitColliders)
        {
            List_FireInFOV.Add(hitCollider.GetComponent<Fire>());
        }

        foreach (var item in temp)
        {
            if (!List_FireInFOV.Contains(item))
            item.isExtinguish = false;
        }

        // 디버그용 박스 그리기
        DebugDrawBox(boxCenter, boxSize, angle, Color.red);

        //Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, Range_radius, layer_Fire);

        //if (hit.Length != 0 && !isDetectedFire)
        //{
        //    isDetectedFire = true;
        //    stageManager.FadeInAudio_BurningAround();
        //}

        //if (hit.Length == 0 && isDetectedFire)
        //{
        //    isDetectedFire = false;
        //    stageManager.FadeOutAudio_BurningAround();
        //}

        //for (int i = 0; i < hit.Length; i++)
        //{
        //    try
        //    {
        //        if (!hit[i].gameObject.activeSelf) continue;

        //        //float theta = Get_DotTheta(item);
        //        bool dotInside = Check_DotInside(hit[i], Range_halfAngle);

        //        //item.gameObject.GetComponent<SpriteRenderer>().color =
        //        //dotInside ? Color.blue : Color.red;

        //        if (dotInside) List_FireInFOV.Add(hit[i].GetComponent<Fire>());
        //    }
        //    catch
        //    {
        //        continue;
        //    }
        //}

        //foreach (Collider2D item in hit)
        //{
        //    try
        //    {
        //        if (!item.gameObject.activeSelf) continue;

        //        //float theta = Get_DotTheta(item);
        //        bool dotInside = Check_DotInside(item, Range_halfAngle);

        //        //item.gameObject.GetComponent<SpriteRenderer>().color =
        //        //dotInside ? Color.blue : Color.red;

        //        if (dotInside) List_FireInFOV.Add(item.GetComponent<Fire>());
        //    }
        //    catch (System.Exception ex)
        //    {
        //        continue;
        //    }
            
        //}
    }

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


    private void Check_Collapse()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, Range_radius, layer_Collapse);
        foreach (Collider2D item in hit)
        {
            Interactor_Collapse collapse = item.GetComponent<Interactor_Collapse>();
            if (!collapse.isPlayerDetect)
            {
                collapse.isPlayerDetect = true;
                StartCoroutine("Detect_Collapse");
            }
        }
    }

    private IEnumerator Detect_Collapse()
    {
        detect_Collapse = true;
        yield return new WaitForSeconds(5f);

        detect_Collapse = false;
    }



    /// <summary> 플레이어 전방 시야각 설정 </summary>
    public void Set_LightFOVAngle(float inner, float outer)
    {
        light_PlayerFOV.pointLightInnerAngle = inner;
        light_PlayerFOV.pointLightOuterAngle = outer;
    }

    /// <summary> 플레이어 전방 시야 거리 설정 </summary>
    public void Set_LightFOVRadius(float raduis)
    {
        light_PlayerFOV.pointLightOuterRadius = raduis;
    }

    /// <summary> 플레이어 전방 시야각 일정 배율 증가 </summary>
    private void Set_IncreaseLightFOVAngle(float increase = 2f)
    {
        light_PlayerFOV.pointLightInnerAngle *= increase;
        light_PlayerFOV.pointLightOuterAngle *= increase;
    }

    /// <summary> 플레이어 전방 시야각 일정 배율 감소 </summary>
    private void Set_DecreaseLightFOVAngle(float decrease = 2f)
    {
        light_PlayerFOV.pointLightInnerAngle /= decrease;
        light_PlayerFOV.pointLightOuterAngle /= decrease;
    }

    /// <summary> 플레이어 전방 시야각 일정 배율 증가 </summary>
    private void Set_IncreaseLightFOVRaduis(float increase = 2f)
    {
        light_PlayerFOV.pointLightOuterRadius *= increase;
    }

    /// <summary> 플레이어 전방 시야각 일정 배율 감소 </summary>
    private void Set_DecreaseLightFOVRaduis(float decrease = 2f)
    {
        light_PlayerFOV.pointLightOuterRadius /= decrease;
    }

    /// <summary> 플레이어 주변 밝기 수치 설정 </summary>
    public void Set_LightAroundIntensity(float intensity)
    => light_PlayerAround.intensity = intensity;

    /// <summary> 플레이어 주변 밝기 반경 설정 </summary>
    public void Set_LightAroundRadius(float inner = 0f, float outer = 5f)
    {
        light_PlayerAround.pointLightInnerRadius = inner;
        light_PlayerAround.pointLightOuterRadius = outer;
    }

    /// <summary> 플레이어 전방 시야각 일정 배율 증가 </summary>
    private void Set_IncreaseLightAroundRaduis(float increase = 2f)
    {
        light_PlayerAround.pointLightOuterRadius *= increase;
    }

    /// <summary> 플레이어 전방 시야각 일정 배율 감소 </summary>
    private void Set_DecreaseLightAroundRaduis(float decrease = 2f)
    {
        light_PlayerAround.pointLightOuterRadius /= decrease;
    }

    private void OnDrawGizmos()
    {
        Handles.color = (isFire) ? Color_Detected : Color_nonDetected;

        Handles.DrawSolidArc(transform.position, transform.forward, obj_FOV.transform.right, Range_halfAngle, Range_radius);
        Handles.DrawSolidArc(transform.position, transform.forward, obj_FOV.transform.right, -Range_halfAngle, Range_radius);
    }

    /// <summary> 플레이어의 어두운 방 입장 </summary>
    private void InDarkedRoom(Collider2D room)
    {
        stageManager.State_InDarkedRoom(room);
        Set_LightAroundIntensity(0.25f);
        //Set_LightAroundRadius(0f, 5f);
        Set_DecreaseLightAroundRaduis(change_FOVAngleRange);
        Set_DecreaseLightFOVAngle(change_FOVAngleRange);
        Set_DecreaseLightFOVRaduis();
    }

    /// <summary> 플레이어의 어두운 방 퇴장 </summary>
    private void OutDarkedRoom(Collider2D room)
    {
        stageManager.State_OutDarkedRoom(room);
        Set_LightAroundIntensity(0.5f);
        //Set_LightAroundRadius(2f, 7f);
        Set_IncreaseLightAroundRaduis(change_FOVAngleRange);
        Set_IncreaseLightFOVAngle(change_FOVAngleRange);
        Set_IncreaseLightFOVRaduis();
    }

    /// <summary> 플레이어의 상호작용 대상에 접근 </summary>
    private void InInteract(Collider2D interact)
    {
        stageManager.Button_ChangeToInteract();

        Interactor obj = interact.GetComponent<Interactor>();
        List_Interactor.Add(obj);
        obj.Show_Interact();

        count_Interact++;
        isInteract = true;
    }

    /// <summary> 플레이어의 상호작용 대상으로부터 나가기 </summary>
    private void OutInteract(Collider2D interact)
    {
        stageManager.Button_ChangeToAttack();

        Interactor obj = interact.GetComponent<Interactor>();
        List_Interactor.Remove(obj);
        obj.Hide_Interact();

        count_Interact--;
        if (count_Interact == 0) isInteract = false;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DarkedRoom"))
        {
            InDarkedRoom(collision); return;
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
            OutDarkedRoom(collision); return;
        }

        if (collision.CompareTag("Interactor"))
        {
            OutInteract(collision); return;
        }
    }
}
