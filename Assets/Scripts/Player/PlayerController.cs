using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] PlayerAbilities playerAbilities;
    //[SerializeField] Popup menu;
    [SerializeField] GameObject leaveButton;

    Controls controls;
    Controls.GameplayActions gameplayActions;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        if (controls == null)
        {
            Debug.Log("New Controls");
            controls = new Controls();
        }

        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds)) {
            Debug.Log("Load Existing Keybinds");
            controls.LoadBindingOverridesFromJson(rebinds);
        }

        gameplayActions = controls.Gameplay;
    }

    private void Update()
    {
        movement.ReceiveInput(horizontalInput);
        mouseLook.ReceiveInput(mouseInput);
    }

    private void OnEnable()
    {
        controls.Enable();

        // groundMovement.[action].performed += context => do something
        gameplayActions.Move.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        gameplayActions.Jump.performed += _ => movement.OnJumpPressed();

        gameplayActions.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        gameplayActions.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        gameplayActions.Shoot.performed += _ => playerAbilities.newTarget();
        gameplayActions.Abilities1.performed += _ => playerAbilities.castSpell(0);
        gameplayActions.Abilities2.performed += _ => playerAbilities.castSpell(1);
        gameplayActions.Abilities3.performed += _ => playerAbilities.castSpell(2);
        gameplayActions.Shield.performed += _ => playerAbilities.castShield();
        gameplayActions.Escape.performed += _ => {
            leaveButton.SetActive(!leaveButton.activeSelf);
            if (leaveButton.activeSelf) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        };
    }

    private void OnDisable() {
        controls.Disable();

        // groundMovement.[action].performed += context => do something
        gameplayActions.Move.performed -= ctx => horizontalInput = ctx.ReadValue<Vector2>();

        gameplayActions.Jump.performed -= _ => movement.OnJumpPressed();

        gameplayActions.MouseX.performed -= ctx => mouseInput.x = ctx.ReadValue<float>();
        gameplayActions.MouseY.performed -= ctx => mouseInput.y = ctx.ReadValue<float>();

        gameplayActions.Shoot.performed -= _ => playerAbilities.newTarget();
        gameplayActions.Abilities1.performed -= _ => playerAbilities.castSpell(0);
        gameplayActions.Abilities2.performed -= _ => playerAbilities.castSpell(1);
        gameplayActions.Abilities3.performed -= _ => playerAbilities.castSpell(2);
        gameplayActions.Shield.performed -= _ => playerAbilities.castShield();
        gameplayActions.Escape.performed -= _ => {
            leaveButton.SetActive(!leaveButton.activeSelf);
            if (leaveButton.activeSelf) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        };
    }

    public void DisableCasting() {
        gameplayActions.Shoot.performed -= _ => playerAbilities.newTarget();
        gameplayActions.Abilities1.performed -= _ => playerAbilities.castSpell(0);
        gameplayActions.Abilities2.performed -= _ => playerAbilities.castSpell(1);
        gameplayActions.Abilities3.performed -= _ => playerAbilities.castSpell(2);
        gameplayActions.Shield.performed -= _ => playerAbilities.castShield();
    }

    private void OnDestroy()
    {
        if (enabled) {
            controls.Disable();

            // groundMovement.[action].performed += context => do something
            gameplayActions.Move.performed -= ctx => horizontalInput = ctx.ReadValue<Vector2>();

            gameplayActions.Jump.performed -= _ => movement.OnJumpPressed();

            gameplayActions.MouseX.performed -= ctx => mouseInput.x = ctx.ReadValue<float>();
            gameplayActions.MouseY.performed -= ctx => mouseInput.y = ctx.ReadValue<float>();

            gameplayActions.Shoot.performed -= _ => playerAbilities.newTarget();
            gameplayActions.Abilities1.performed -= _ => playerAbilities.castSpell(0);
            gameplayActions.Abilities2.performed -= _ => playerAbilities.castSpell(1);
            gameplayActions.Abilities3.performed -= _ => playerAbilities.castSpell(2);
            gameplayActions.Shield.performed -= _ => playerAbilities.castShield();
            gameplayActions.Escape.performed -= _ => {
                leaveButton.SetActive(!leaveButton.activeSelf);
                if (leaveButton.activeSelf) {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                } else {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            };
        }
    }
}
