using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 10.0f;

    public float speed = 12.0f;
    public float jumpSpeed = 12.0f;
    public float gravity = 0.2f;
    public float terminalV = -16.0f;

    CharacterController controller;
    GameObject myCamera;
    GameObject body;
    Transform whole;


    Transform grndCheck;
    float grndDistance = 0.25f;
    public LayerMask groundMask;
    bool isGrounded;

    float xRotation = 0.0f;
    float yVelocity = 0.0f;
    Vector3 jumpMomentum;

    bool paused = false;

    

    // Start is called before the first frame update
    void Start()
    {
        body = this.gameObject.transform.GetChild(0).gameObject;
        myCamera = this.gameObject.transform.GetChild(1).gameObject;
        grndCheck = this.gameObject.transform.GetChild(2);
        whole = this.GetComponent<Transform>();
        controller = this.GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            paused = !paused;
            if (paused){
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (!paused){

            float tScale = Time.deltaTime;
            isGrounded = Physics.CheckSphere(grndCheck.position, grndDistance, groundMask);

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp (xRotation, -90.0f, 90.0f);

            myCamera.GetComponent<Transform>().localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
            whole.Rotate(Vector3.up * mouseX);

            float moveZ = Input.GetAxis("Vertical");
            float moveX = Input.GetAxis("Horizontal");

            Vector3 move = (transform.right * moveX) + (transform.forward * moveZ);
            move.Normalize();
            move *= speed;

            if (isGrounded){
                jumpMomentum = move;
                if (yVelocity < 0){
                    yVelocity = -1.0f;
                }
                if (Input.GetKeyDown("space")){
                    yVelocity = jumpSpeed;
                }
            } else {
                move *= 0.75f;

                if (yVelocity > terminalV){
                    yVelocity -= (gravity * tScale);
                }
            }
            move.y = yVelocity;


            controller.Move(move * tScale);
        }
    }
}
