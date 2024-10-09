using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Player : MonoBehaviour
{
    public Camera           mainCamera;

    public SpriteRenderer   sr;

    public bool             isMove;

    public Vector2[]        moveVec;
    public Vector2          setMoveVec;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        mainCamera = Camera.main;
        moveVec = Test_Init.Get_MoveVecs();
    }

    private void Update()
    {
        Player_Move();
        Player_Flip();
    }

    /// <summary> 플레이어 이동 </summary>
    void Player_Move()
    {
        if (!isMove) return;
        transform.Translate(setMoveVec * 3f * Time.deltaTime);
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
}
