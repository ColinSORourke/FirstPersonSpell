using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 11f;
    Vector3 horizontalInput;

  /*  [SerializeField] float jumpHeight = 4f;
    public bool jump;

    [SerializeField] float downSpeed = 4f;
    public bool down; */

    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    public bool sprint;

    public GameObject spec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = sprint ? 20f : 11f;
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.z + transform.up * horizontalInput.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);

/*        if (jump || down)
        {
            if (jump)
            {
                Debug.Log("up you go");
                verticalVelocity.y = Mathf.Sqrt(60f * jumpHeight);
            }
            if (down)
            {
                Debug.Log("making my way down town");
                verticalVelocity.y = -Mathf.Sqrt(60f * downSpeed);
            }
        }
        if (!jump && !down)
        {
            verticalVelocity.y = 0;
        }  


        controller.Move(verticalVelocity * Time.deltaTime);  */
    }

    public void ReceiveInput(Vector3 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }
}
