using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MouseLook : NetworkBehaviour
{
    [SerializeField] float sensitivityX = 2.0f;
    [SerializeField] float sensitivityY = 0.5f;
    float mouseX, mouseY;

    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;

    private void Start() {
        gameObject.name = "Player " + gameObject.GetComponent<NetworkObject>().OwnerClientId;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetSensitityFromPlayerPrefs();

        if (IsLocalPlayer) {
            playerCamera.gameObject.SetActive(true);
            this.transform.Find("InfoCanvas").gameObject.SetActive(false);
            this.transform.Find("Sphere").gameObject.SetActive(false);
            this.gameObject.layer = 0;
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            var playUI = this.gameObject.GetComponent<PlayerUI>();
            playUI.enabled = true;
            FindObjectOfType<LobbyManager>().countdownText = transform.Find("KeyUI/Countdown").GetComponent<Text>();
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
            this.transform.Find("KeyUI").gameObject.SetActive(false);
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

    public void SetSensitityFromManual(float newSensitivityX = 1.55f)
    {
        sensitivityX = 0.05f + newSensitivityX*0.15f;
        sensitivityY = sensitivityX / 2;
    }

    public void SetSensitityFromPlayerPrefs()
    {
        sensitivityX = PlayerPrefs.HasKey("MouseSensitivityPreference") ? 0.05f + PlayerPrefs.GetFloat("MouseSensitivityPreference")*0.15f : 1.55f;
        sensitivityY = sensitivityX / 2;
    }
}
