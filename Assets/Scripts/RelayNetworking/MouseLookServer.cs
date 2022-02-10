using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MouseLookServer : NetworkBehaviour
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

    [SerializeField]
    private NetworkVariable<float> forwardAxis = new NetworkVariable<float>();
    [SerializeField]
    private NetworkVariable<float> sideAxis = new NetworkVariable<float>();
    [SerializeField]
    private NetworkVariable<float> verticalAxis = new NetworkVariable<float>();
    [SerializeField]
    private NetworkVariable<Quaternion> sideRotation = new NetworkVariable<Quaternion>();

    private float oldForwardAxis;
    private float oldSideAxis;
    private float oldVerticalAxis;
    
    // Start is called before the first frame update
    void Start()
    {
        body = this.gameObject.transform.GetChild(0).gameObject;
        myCamera = this.GetComponentInChildren<Camera>().gameObject;
        grndCheck = this.gameObject.transform.GetChild(2);
        whole = this.GetComponent<Transform>();
        controller = this.GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;

        if (IsLocalPlayer) {
            myCamera.SetActive(true);
        } else {
            myCamera.SetActive(false);
        }
    }

    void Update() {
        if (IsServer) {
            UpdateServer();
        }

        if (IsClient && IsOwner) {
            UpdateClient();
        }
    }

    void UpdateServer() {
        float tScale = Time.deltaTime;
        controller.Move(new Vector3(sideAxis.Value, verticalAxis.Value, forwardAxis.Value) * tScale);
    }

    void UpdateClient()
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
            //whole.Rotate(Vector3.up * mouseX);
            UpdateServerRotationClientRpc(mouseX, new ClientRpcParams() { Send = new ClientRpcSendParams() { TargetClientIds = new[] { OwnerClientId } } });

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

            //controller.Move(move * tScale);

            if (oldForwardAxis != move.z || oldSideAxis != move.x || oldVerticalAxis != move.y) {
                oldForwardAxis = move.z;
                oldSideAxis = move.x;
                oldVerticalAxis = move.y;
                
                UpdateClientPositionServerRpc(move.z, move.x, move.y);
            }
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float moveZ, float moveX, float moveY) {
        forwardAxis.Value = moveZ;
        sideAxis.Value = moveX;
        verticalAxis.Value = moveY;
    }

    [ClientRpc]
    public void UpdateServerRotationClientRpc(float mouseX, ClientRpcParams clientRpcParams = default) {
        whole.Rotate(Vector3.up * mouseX);
    }
}
