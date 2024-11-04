using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEditor;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    public  Camera          mainCamera;

    public  StageManager    stageManager;

    public  SpriteRenderer  sr;

    public  SpriteMask      sMask;

    public  GameObject      obj_FOV;
    public  GameObject      obj_Attack;

    public  Light2D         light_PlayerFOV;

    public  bool            isMove;
    public  bool            isFire;
    public  bool            isDetected;
    public  bool            isInteract;
    public  byte            count_Interact;

    public  float           time_Fire;

    
    public  Vector2[]       moveVec;
    public  Vector2         setMoveVec;

    public  Quaternion[]    rotate;
    public  Quaternion      setRotate;

    private Interactor      target_Interactor;

    [Header("Darkness")]
    [Range(0, 1), SerializeField]
    private float           range_Darkness;
    [Range(0, 1), SerializeField]
    private float           range_DarknessInRoom;

    [Header("Detect Draw")]
    public  Color           Color_nonDetected = new Color(0, 1, 0, 0.5f);
    public  Color           Color_Detected = new Color(1, 0, 0, 0.5f);
    [Range(0, 1)]
    [SerializeField]
    private float           Range_Alpha = 0.5f;


    [Range(0, 90)]
    public  float           Range_halfAngle = 20;
    [Range(0, 20)]
    public  float           Range_radius = 5;

    [Header("Detect Interaction")]
    [Range(0, 90), SerializeField]
    private float           interaction_halfAngle = 20;
    [Range(0, 20), SerializeField]
    private float           interaction_Radius = 5;

    [Header("Detect Fire")]
    [Range(0, 90)]
    [SerializeField]
    private float           fire_halfAngle = 20;
    [Range(0, 20)]
    [SerializeField]
    private float           fire_Radius = 5;

    

    public GameObject test_Obj;

    LayerMask               layer_Fire;
    LayerMask               layer_Interaction;

    List<GameObject>        List_FireInFOV;
    List<Interactor>        List_Interactor;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        List_FireInFOV = new List<GameObject>();
        List_Interactor = new List<Interactor>();

        layer_Fire = LayerMask.GetMask("Fire");
        layer_Interaction = LayerMask.GetMask("Interaction");

        mainCamera = Camera.main;

        moveVec = Test_Init.Get_MoveVecs();
        rotate = Test_Init.Get_Rotation();

        isFire = false;
        isInteract = false;
        count_Interact = 0;
        time_Fire = 0;
        Set_LightFOV(45, 60);
    }

    private void Reset()
    {
        Awake();

        Color_nonDetected = new Color(0, 1, 0, 0.5f);
        Color_Detected = new Color(1, 0, 0, 0.5f);

        isDetected = false;
    }

    private void Update()
    {
        Get_Input();
        Timing();

        Player_Move();
        Player_Rotate();
        Player_Flip();

        Player_Attack();

        Check_Fire();
        //Check_Interaction();
    }

    void Get_Input()
    {
        if (Input.GetKey(KeyCode.A)) isFire = true;

        if (Input.GetKeyDown(KeyCode.Q) && isInteract)
        {
            foreach (var interactor in List_Interactor)
            interactor.Start_Interact();
        }
    }

    /// <summary> 플레이어 이동 </summary>
    void Player_Move()
    {
        if (!isMove) return;
        if (isFire) return;
        transform.Translate(setMoveVec * 3f * Time.deltaTime);
    }

    void Player_Rotate()
    {
        if (!isMove) return;
        obj_FOV.transform.rotation = setRotate;
        obj_Attack.transform.rotation = setRotate;
    }

    void Player_Attack()
    {
        if (!isFire)
        {
            obj_Attack.SetActive(false);
            return;
        }
        obj_Attack.SetActive(true);

        foreach (var item in List_FireInFOV)
        {
            item.gameObject.SetActive(false);
        }
    }

    void Timing()
    {
        if (time_Fire > 1f && !Input.GetKey(KeyCode.A))
        {
            time_Fire = 0f;
            isFire = false;
        }

        if (isFire)
        {
            Timing_Fire(); return;
        }
    }

    void Timing_Fire()
    {
        time_Fire += Time.deltaTime;
    }

    /// <summary> 플레이어 이미지 소스 좌우반전 </summary>
    void Player_Flip()
    {
        if (!isMove) return;

        if (setMoveVec.x < 0) sr.flipX = true;
        if (setMoveVec.x > 0) sr.flipX = false;
    }

    /// <summary> 플레이어 이동 벡터 지정 </summary>
    public void Set_MoveVec(int type) => setMoveVec = moveVec[type];

    /// <summary> 플레이어 이동 여부 지정 </summary>
    public void Set_isMove(bool move) => isMove = move;


    public void Set_Rotation(int type) => setRotate = rotate[type];


    void Check_Fire()
    {
        List_FireInFOV.Clear();

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, Range_radius, layer_Fire);

        foreach (Collider2D item in hit)
        {
            //float theta = Get_DotTheta(item);
            bool dotInside = Check_DotInside(item, Range_halfAngle);

            //item.gameObject.GetComponent<SpriteRenderer>().color =
            //dotInside ? Color.blue : Color.red;

            if (dotInside)  List_FireInFOV.Add(item.gameObject);
        }
    }

    void Check_Interaction()
    {
        //List_InteractionInFOV.Clear();

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, Range_radius, layer_Interaction);

        foreach (Collider2D item in hit)
        {
            //float theta = Get_DotTheta(item);
            bool dotInside = Check_DotInside(item, Range_halfAngle);

            item.gameObject.GetComponent<SpriteRenderer>().color =
            dotInside ? Color.blue : Color.red;

            //if (dotInside) List_InteractionInFOV.Add(item.gameObject);
        }
    }

    float Get_DotTheta(Collider2D hitObj)
    {
        Vector2 targetVec = hitObj.transform.position - transform.position;

        float dot = Vector2.Dot(obj_FOV.transform.right, targetVec.normalized);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return theta;
    }

    bool Check_DotInside(Collider2D hitObj, float half_Range)
    {
        //float theta = Get_DotTheta(hitObj);

        Vector2 targetVec = hitObj.transform.position - transform.position;

        float dot = Vector2.Dot(obj_FOV.transform.right, targetVec.normalized);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return theta < half_Range;
    }

    private void Set_LightFOV(float inner, float outer)
    {
        light_PlayerFOV.pointLightInnerAngle = inner;
        light_PlayerFOV.pointLightOuterAngle = outer;
    }

    private void OnDrawGizmos()
    {
        Handles.color = (isFire) ? Color_Detected : Color_nonDetected;

        Handles.DrawSolidArc(transform.position, transform.forward, obj_FOV.transform.right, Range_halfAngle, Range_radius);
        Handles.DrawSolidArc(transform.position, transform.forward, obj_FOV.transform.right, -Range_halfAngle, Range_radius);
    }

    private void InDarkedRoom(Collider2D room)
    {
        stageManager.State_InDarkedRoom(room);
        Set_LightFOV(15f, 20f);
    }

    private void OutDarkedRoom(Collider2D room)
    {
        stageManager.State_OutDarkedRoom(room);
        Set_LightFOV(45f, 60f);
    }

    private void InInteract(Collider2D interact)
    {
        Interactor obj = interact.GetComponent<Interactor>();
        List_Interactor.Add(obj);
        obj.Show_Interact();

        count_Interact++;
        isInteract = true;
    }

    private void OutInteract(Collider2D interact)
    {
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
