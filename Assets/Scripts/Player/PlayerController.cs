using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] GameObject gameplayUIGroup, menuUIGroup, player;
    [SerializeField] PlayerAbilities playerAbilities;
    //[SerializeField] Popup menu;
    [SerializeField] GameObject inGameMenu;

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
        this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("VolumePreference");

        gameplayActions = controls.Gameplay;
    }

    private void Update()
    {
        //Debug.Log("Player Volume = " + this.GetComponent<AudioSource>().volume);
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
        gameplayActions.Abilities1.performed += _ => playerAbilities.castSpell(2);
        gameplayActions.Abilities2.performed += _ => playerAbilities.castSpell(1);
        gameplayActions.Abilities3.performed += _ => playerAbilities.castSpell(0);
        gameplayActions.Shield.performed += _ => playerAbilities.castShield();
        gameplayActions.Escape.performed += _ => this.switchUIGroup();
        /*
        {
            
            inGameMenu.SetActive(!inGameMenu.activeSelf);
            if (inGameMenu.activeSelf) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                //Disable mouse X & Y axis
                gameplayActions.MouseX.Disable();
                gameplayActions.MouseY.Disable();
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                //Ensable mouse X & Y axis
                gameplayActions.MouseX.Enable();
                gameplayActions.MouseY.Enable();
            }
        };
        */
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
        gameplayActions.Escape.performed -= _ => switchUIGroup();
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
            gameplayActions.Escape.performed -= _ => switchUIGroup();
        }
    }
	
    public void switchUIGroup(){
        bool temp = gameplayUIGroup.activeSelf;
        gameplayUIGroup.SetActive(!temp);
        menuUIGroup.SetActive(temp);
        Cursor.visible = menuUIGroup.activeSelf;
        if (!Cursor.visible) {
            //Gameplay mode
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<MouseLook>().enabled = true;
            player.GetComponent<Movement>().enabled = true;

            player.GetComponent<PlayerInGameSetting>().saveSetting();
            mouseLook.SetSensitityFromPlayerPrefs();
        }
            
        else {
            //Ingame Menu mode
            Cursor.lockState = CursorLockMode.None; 
            player.GetComponent<MouseLook>().enabled = false;
            player.GetComponent<Movement>().enabled = false;

            player.GetComponent<PlayerInGameSetting>().loadSettings();
        }
        
    }
	
}
