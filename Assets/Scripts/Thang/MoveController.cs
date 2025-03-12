using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Nhận đầu vào từ bàn phím
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Chuẩn hóa vector để tránh di chuyển nhanh hơn khi đi chéo
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Di chuyển nhân vật bằng cách thay đổi vận tốc Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}
