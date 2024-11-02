using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 8.0f;

    Rigidbody2D rb;
    Camera viewCamera;
    Vector2 velocity;
    float rotDegree;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        viewCamera = Camera.main;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos = viewCamera.ScreenToWorldPoint(mousePos);

        

        float dy = mousePos.y - rb.position.y;
        float dx = mousePos.x - rb.position.x;
        rotDegree = -(Mathf.Rad2Deg * Mathf.Atan2(dy, dx) - 90);
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed;

        //Debug.Log(mousePos);
        //Debug.Log(rotDegree);
    }

    void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(0, 0, rotDegree));
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}