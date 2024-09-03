
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Stage : MonoBehaviour
{
    public GameObject player;
    public GameObject joyStick;

    public Camera camera;

    public Vector2 joyStickPos;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        Player_Move();
    }

    /// <summary> 플레이어 이동 </summary>
    void Player_Move()
    {
        if (Input.GetMouseButton(0))
        {
            Player_Rotate();
            player.transform.Translate(Vector2.right * 3f * Time.deltaTime);
            camera.transform.position = player.transform.position + Vector3.back;
        }
    }

    /// <summary> 플레이어 회전 </summary>
    void Player_Rotate()
    {
        joyStickPos = joyStick.transform.position;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dis = mousePos - joyStickPos;
        float degree = Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg;

        player.transform.rotation = Quaternion.Euler(0, 0, degree);
    }
}
