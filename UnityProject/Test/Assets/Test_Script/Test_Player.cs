using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.UI.Image;

public class Test_Player : MonoBehaviour
{
    public Camera           mainCamera;

    public SpriteRenderer   sr;

    public GameObject       obj_FOV;

    public bool             isMove;
    public bool             isFire;
    public bool             isDetected;

    public float            time_Fire;

    public Color            Color_nonDetected = new Color(0, 1, 0, 0.5f);
    public Color            Color_Detected = new Color(1, 0, 0, 0.5f);
    public Vector2[]        moveVec;
    public Vector2          setMoveVec;

    public Quaternion[]     rotate;
    public Quaternion       setRotate;

    [Range(0, 90)] [SerializeField]
    private float Range_halfAngle = 20;
    [Range(0, 20)] [SerializeField]
    private float Range_radius = 5;

    [Range(0, 1)] [SerializeField]
    private float Range_Alpha = 0.5f;

    public GameObject test_Obj;

    LayerMask               target_Layermask;

    List<GameObject> List_FireInFOV;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        time_Fire = 0;
        isFire = false;
        List_FireInFOV = new List<GameObject>();
        mainCamera = Camera.main;
        moveVec = Test_Init.Get_MoveVecs();
        rotate = Test_Init.Get_Rotation();
        target_Layermask = LayerMask.GetMask("Target");
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
    }

    void Get_Input()
    {
        if (Input.GetKey(KeyCode.A)) isFire = true;
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
    }

    void Player_Attack()
    {
        if (!isFire) return;

        foreach (var item in List_FireInFOV)
        {
            item.gameObject.SetActive(false);
        }
    }

    void Timing()
    {
        if (time_Fire > 1f)
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

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, Range_radius, target_Layermask);

        foreach (Collider2D item in hit)
        {
            Vector2 targetVec = item.transform.position - transform.position;

            float dot = Vector2.Dot(setMoveVec, targetVec.normalized);
            //Debug.Log(dot);

            float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;
            //Debug.Log(theta);

            float distance = Vector2.Distance(targetVec, transform.position);
            //Debug.Log($"{dot}, {theta}, {distance}");

            item.gameObject.GetComponent<SpriteRenderer>().color =
                (theta < Range_halfAngle && distance < Range_radius) ? new Color(1, 1, 1, Range_Alpha) : Color.white;

            if(theta < Range_halfAngle)
            {
                item.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                List_FireInFOV.Add(item.gameObject);
            }
            else
            {
                item.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = (isFire) ? Color_Detected : Color_nonDetected;

        if (!isMove) return;
        Handles.DrawSolidArc(transform.position, transform.forward, setMoveVec, Range_halfAngle, Range_radius);
        Handles.DrawSolidArc(transform.position, transform.forward, setMoveVec, -Range_halfAngle, Range_radius);
    }
}
