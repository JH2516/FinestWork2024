
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Stage : MonoBehaviour
{
    public GameObject player;
    public GameObject joyStick;

    public Camera mainCamera;
    public Camera playerCamera;

    public Vector2 joyStickPos;

    public bool[] moveControl;
    public bool isMove;

    public Vector2[] moveVec;
    public Vector2 setMoveVec;

    private void Awake()
    {
        mainCamera = Camera.main;
        moveControl = new bool[9];
        moveVec = new Vector2[9];

        moveVec[0] = new Vector2(-1, 1);
        moveVec[1] = new Vector2(0, 1);
        moveVec[2] = new Vector2(1, 1);
        moveVec[3] = new Vector2(-1, 0);
        moveVec[4] = new Vector2(0, 0);
        moveVec[5] = new Vector2(1, 0);
        moveVec[6] = new Vector2(-1, -1);
        moveVec[7] = new Vector2(0, -1);
        moveVec[8] = new Vector2(1, -1);
    }

    private void Update()
    {
        Player_Move();
    }

    /// <summary> 플레이어 이동 </summary>
    void Player_Move()
    {
        if (!isMove) return;

        /*
        if (Input.GetMouseButton(0))
        {
            player.transform.Translate(Vector2.right * 3f * Time.deltaTime);
            playerCamera.transform.position = player.transform.position + Vector3.back;
        }
        */

        player.transform.Translate(setMoveVec * 3f * Time.deltaTime);
    }


    public void Button_Move(int type)
    {
        for (int i = 0; i < 9; i++)
        {
            moveControl[i] = i == type;
        }

        setMoveVec = moveVec[type];
    }

    public void Button_isDown() => isMove = true;

    public void Button_isUp() => isMove = false;


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
