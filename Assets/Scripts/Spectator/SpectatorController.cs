using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorController : MonoBehaviour
{
    [SerializeField] SpectatorMovement SpectatorMovement;
    [SerializeField] MouseLook mouseLook;
    //[SerializeField] Popup menu;

    Controls controls;
    Controls.SpectatorActions spectatorActions;

    Vector3 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        if (controls == null)
        {
            Debug.Log("New Controls");
            controls = new Controls();
        }

    /*    var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            Debug.Log("Load Existing Keybinds");
            controls.LoadBindingOverridesFromJson(rebinds);
        } */

        spectatorActions = controls.Spectator;

        // groundMovement.[action].performed += context => do something
        spectatorActions.Move.performed += ctx => horizontalInput = ctx.ReadValue<Vector3>();

        spectatorActions.Sprint.performed += _ => SpectatorMovement.sprint = true;
        spectatorActions.Sprint.canceled += _ => SpectatorMovement.sprint = false;

        spectatorActions.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        spectatorActions.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        //gameplayActions.Escape.performed += _ => menu.popup();
    }

    private void Update()
    {
        SpectatorMovement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }
}