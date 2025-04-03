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

    [SerializeField] GameObject playerUI;

    private bool isMoving => movement != Vector2.zero;
    private bool isInvoking;
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
            virtualCamera.Follow = transform;
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isMoving && !isInvoking)
        {
            isInvoking = true;
            InvokeRepeating(nameof(PlayFootstep), 0, 0.5f);
        }
        else if (!isMoving && isInvoking)
        {
            isInvoking = false;
            CancelInvoke(nameof(PlayFootstep));
        }
    }

    void PlayFootstep()
    {
        if (!HasStateAuthority)
            return;

        AudioManager.Instance.PlayAudioRpc("Footstep", "Master/SFX/Footstep", transform.position);
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
            return;

        rb.MovePosition(rb.position + movement * moveSpeed * Runner.DeltaTime);

        if (movement == Vector2.zero)
            rb.velocity = Vector2.zero;

        Vector2 lookDir = -(mousePos - rb.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = angle;
    }

    public override void Render()
    {
        playerUI.transform.forward = Camera.main.transform.forward;
    }
}
