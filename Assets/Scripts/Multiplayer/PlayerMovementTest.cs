using Fusion;
using UnityEngine;

public class PlayerMovementTest : NetworkBehaviour
{
    private CharacterController _controller;

    public float PlayerSpeed = 2f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        // FixedUpdateNetwork is only executed on the StateAuthority

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

        _controller.Move(move);

        if (move != Vector3.zero)
        {
            gameObject.transform.position += move;
        }
    }
}