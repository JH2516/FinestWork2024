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
    public bool             isDetected;

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


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

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
        Player_Move();
        Player_Rotate();
        Player_Flip();

        Player_Attack();

        Check_Dot();
    }

    /// <summary> 플레이어 이동 </summary>
    void Player_Move()
    {
        if (!isMove) return;
        transform.Translate(setMoveVec * 3f * Time.deltaTime);
    }

    void Player_Rotate()
    {
        if (!isMove) return;
        obj_FOV.transform.rotation = setRotate;
    }

    void Player_Attack()
    {
        if (!Input.GetKey(KeyCode.A)) return;


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


    void Check_Dot()
    {
        /*
        Vector2 targetVec = test_Obj.transform.position - transform.position;

        float dot = Vector2.Dot(transform.right, targetVec.normalized);
        //Debug.Log(dot);

        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;
        //Debug.Log(theta);

        float distance = Vector2.Distance(targetVec, transform.position);
        Debug.Log($"{dot}, {theta}, {distance}");

        isDetected = (theta < Range_halfAngle && distance < Range_radius) ? true : false;
        */


        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, Range_radius, target_Layermask);
        //Debug.Log(hitColliders.Length);

        foreach (Collider2D item in hitColliders)
        {
            Vector2 targetVec = item.transform.position - transform.position;

            float dot = Vector2.Dot(setMoveVec, targetVec.normalized);
            //Debug.Log(dot);

            float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;
            Debug.Log(theta);

            float distance = Vector2.Distance(targetVec, transform.position);
            //Debug.Log($"{dot}, {theta}, {distance}");

            item.gameObject.GetComponent<SpriteRenderer>().color =
                (theta < Range_halfAngle && distance < Range_radius) ? new Color(1, 1, 1, Range_Alpha) : Color.white;

            if(theta < Range_halfAngle)
            {
                item.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                item.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            
        }
    }

    /*
    /// <summary> 플레이어 회전 </summary>
    void Player_Rotate()
    {
        joyStickPos = joyStick.transform.position;

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dis = mousePos - joyStickPos;
        float degree = Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg;

        player.transform.rotation = Quaternion.Euler(0, 0, degree);
    }
    */

    private void OnDrawGizmos()
    {
        Handles.color = (isDetected) ? Color_Detected : Color_nonDetected;

        Handles.DrawSolidArc(transform.position, transform.forward, transform.right, Range_halfAngle, Range_radius);
        Handles.DrawSolidArc(transform.position, transform.forward, transform.right, -Range_halfAngle, Range_radius);
    }
}
