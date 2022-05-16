using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 12.0f;
    Vector2 horizontalInput;

    [SerializeField] float jumpHeight = 3.5f;
    bool jump;

    [SerializeField] float gravity = -30f; // -9.81
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    public float coyoteTime = 0.1f;
    private float graceTime;

    [SerializeField] PlayerStateScript playerStateScript;

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position - (new Vector3(0, transform.localScale.y, 0)), 0.2f, groundMask);
        if (isGrounded)
        {
            verticalVelocity.y = 0;
            graceTime = coyoteTime;
        } else
        {
            graceTime -= Time.deltaTime;
        }

        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);

        // Jump: v = sqrt(-2 * jumpHeight * gravity)
        if (jump)
        {
            if (isGrounded || graceTime > 0)
            {
                graceTime = 0;
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
            playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.JumpStart);
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        if (verticalVelocity.y < -1) playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.JumpFall);
        else this.setHorizontalAnimatorState(horizontalInput);
        /*
        else if (horizontalInput.x == 0 && horizontalInput.y == 0) playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.Idle);
        else playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.Run);
        */
    }

    public void ReceiveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    public void OnJumpPressed()
    {
        jump = true;
    }

    public void setMovementSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void setHorizontalAnimatorState(Vector2 horizontalInput)
    {
        Debug.Log("SetHorizonalAnimatorState");
        switch (horizontalInput)
        {
            case Vector2 v when v.Equals(Vector2.zero):
                playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.Idle);
                break;
            case Vector2 v when v.Equals(Vector2.left):
                playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.StrafeLeft);
                break;
            case Vector2 v when v.Equals(Vector2.right):
                playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.StrafeRight);
                break;
            case Vector2 v when v.Equals(new Vector2(-1, 1)):
                playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.RunLeft);
                break;
            case Vector2 v when v.Equals(Vector2.one):
                playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.RunRight);
                break;
            case Vector2 v when v.Equals(Vector2.down):
                playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.RunBack);
                break;
            case Vector2 v when v.Equals(Vector2.up):
                playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.Run);
                break;
        }
    }
}
