using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MouseLook : NetworkBehaviour
{
    [SerializeField] float sensitivityX = 8f;
    [SerializeField] float sensitivityY = 0.5f;
    float mouseX, mouseY;

    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;

    private void Start() {
        gameObject.name = "Player " + NetworkManager.Singleton.LocalClientId;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensitivityX = PlayerPrefs.HasKey("MouseSensitivityPreference") ? PlayerPrefs.GetFloat("MouseSensitivityPreference") : 10.0f;

        if (IsLocalPlayer) {
            playerCamera.gameObject.SetActive(true);
            this.transform.Find("InfoCanvas").gameObject.SetActive(false);
            this.transform.Find("Sphere").gameObject.SetActive(false);
            this.gameObject.layer = 0;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            var playUI = this.gameObject.GetComponent<PlayerUI>();
            playUI.enabled = true;
        } else {
            playerCamera.gameObject.SetActive(false);
            this.gameObject.GetComponent<PlayerUI>().enabled = false;
            var tarUI = this.gameObject.GetComponent<TargetUI>();
            tarUI.enabled = true;
            this.gameObject.GetComponent<PlayerAbilities>().enabled = false;
            this.gameObject.GetComponent<Movement>().enabled = false;
            this.gameObject.GetComponent<PlayerController>().enabled = false;
            this.gameObject.GetComponent<CharacterController>().enabled = false;
            this.transform.Find("PlayerUI").gameObject.SetActive(false);
        }
    }

    private void Update() {
        transform.Rotate(Vector3.up, mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }

    public void ReceiveInput(Vector2 mouseInput) {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }
}
