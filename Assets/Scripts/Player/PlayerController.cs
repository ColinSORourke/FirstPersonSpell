using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] PlayerAbilities playerAbilities;

    Controls controls;
    Controls.GameplayActions gameplayActions;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        controls = new Controls();
        gameplayActions = controls.Gameplay;

        // groundMovement.[action].performed += context => do something
        gameplayActions.Move.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();

        gameplayActions.Jump.performed += _ => movement.OnJumpPressed();

        gameplayActions.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        gameplayActions.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        gameplayActions.Shoot.performed += _ => playerAbilities.newTarget();
        gameplayActions.Abilities1.performed += _ => playerAbilities.cast(0, 1f);
        gameplayActions.Abilities2.performed += _ => playerAbilities.cast(1, 1f);
        gameplayActions.Abilities3.performed += _ => playerAbilities.cast(2, 1f);
    }

    private void Update()
    {
        movement.ReceiveInput(horizontalInput);
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
