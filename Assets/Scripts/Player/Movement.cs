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
    bool previouslyGrounded = true;
    public float coyoteTime = 0.1f;
    private float graceTime;

    [SerializeField] PlayerStateScript playerStateScript;

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position - (new Vector3(0, transform.localScale.y, 0)), 0.2f, groundMask);
        if (isGrounded)
        {
            if (!previouslyGrounded){
                playerStateScript.UpdateGroundStateServerRpc(true);
                playerStateScript.UpdateJumpStateServerRpc(false);
                verticalVelocity.y = 0;
                graceTime = coyoteTime;
                previouslyGrounded = true;
            }
        } else
        {
            if (previouslyGrounded){
                playerStateScript.UpdateGroundStateServerRpc(false);
                previouslyGrounded = false;
            }
            
            graceTime -= Time.deltaTime;
        }

        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);

        // Jump: v = sqrt(-2 * jumpHeight * gravity)
        if (jump)
        {
            
            if (isGrounded || graceTime > 0)
            {
                playerStateScript.UpdateJumpStateServerRpc(true);
                graceTime = 0;
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        /* if (verticalVelocity.y < -1) playerStateScript.UpdateAnimStateServerRpc(PlayerStateScript.AnimState.JumpFall);
        else this.setHorizontalAnimatorState(horizontalInput); */
        
        playerStateScript.UpdateMoveStateServerRpc(horizontalInput);
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
}
