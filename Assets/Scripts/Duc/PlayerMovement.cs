using Cinemachine;
using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    CinemachineVirtualCamera virtualCamera;

    Vector2 movement;
    Vector2 mousePos;

    public override void Spawned()
    {
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public override void FixedUpdateNetwork()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Runner.DeltaTime);

        Vector2 lookDir = -(mousePos - rb.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
