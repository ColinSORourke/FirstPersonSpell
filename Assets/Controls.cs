//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Controls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""ffa04782-ba68-47be-b82e-abd785983a58"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""abb2a10b-bcc2-496f-8d53-957a6eeb6828"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""50ac9ee1-01f7-4ec4-b6ba-cd1dab344833"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseX"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ef96c22d-934e-43d3-bcf3-e197e8059b5d"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseY"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7f751920-db45-44b4-b6d8-7b416511d2d5"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""537c541e-27c9-4d2f-add8-01e1d8216804"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Abilities 1"",
                    ""type"": ""Button"",
                    ""id"": ""91334280-deda-42de-b870-3d36a6acd873"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Abilities 2"",
                    ""type"": ""Button"",
                    ""id"": ""a1fbfffe-f406-42f8-96c9-1520094c8f99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Abilities 3"",
                    ""type"": ""Button"",
                    ""id"": ""ae35bf93-d41a-4a54-8a9c-8b065bb959de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shield"",
                    ""type"": ""Button"",
                    ""id"": ""9e49a4e8-395d-4ca2-8fa0-af239f91023d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""755459e4-5493-4a63-a8f2-343978e353bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""27c82230-fecc-4a14-add0-e234d2d5dd77"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""62476112-cd4e-4e65-9a1a-c511d4e6956e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""47088735-9d15-45eb-9d48-63c76df89f1c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4b6f2d53-1268-45d1-9d8f-db6fae42c2f6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1182e6b0-5c88-4bc9-9e29-e10c93880c38"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6fd305c3-9f9e-4d2d-a937-b52a0fd4e3d1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""bf14d12b-4f6f-4b4b-bce6-76c13efea201"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d019f8b-c6ec-4e3c-8961-da099df9b3af"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a97b873-d05b-49ed-8965-6542f9412b58"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20b1f498-8fd1-49bd-88ed-c28e9ad29d04"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Abilities 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43a17493-395d-4ac5-94e8-7e06c594dafb"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Abilities 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f05f63f-a0eb-4cd6-8dd0-7ac2df1d5592"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Abilities 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""594f1382-1382-4784-a2dc-9ec555ee56f7"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shield"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ccf1c69-499c-41bf-a54f-ecb319d13284"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Spectator"",
            ""id"": ""eea64e9c-0820-4240-a218-a214afe3db21"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bd0616b3-eb12-47f1-b40d-6369c92169a6"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseX"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5bcd6794-9bb3-449b-acf5-32a00e6d065e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseY"",
                    ""type"": ""PassThrough"",
                    ""id"": ""522f7560-2572-4b9f-88be-c73255d93084"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""470a7451-0645-4bcf-a4f0-fdc6be10c4a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""f5ab0765-f5d7-4aa6-b065-e010bbee91d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""bb8704ad-1589-4e4d-afe0-239c980cc6e9"",
                    ""path"": ""3DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""534ba171-f1d2-47e0-b4ef-8b8b21ed2197"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""85d1b0a4-78a1-4d5b-9ad5-a333b2b247b6"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""dd5e05c4-2406-4aa9-923a-5239c5983a51"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""2d89c381-5a15-41b2-92aa-6c64d5471dbf"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Forward"",
                    ""id"": ""b2fd9094-f300-41fc-bf6b-1272cf2b4aa6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Backward"",
                    ""id"": ""774399ca-455e-46c8-bf1e-a37663a07b69"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""beae579b-be64-46fc-a65d-a17e86631914"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d62891ca-c3ad-4d19-9011-962b3305af6a"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d2e7c9b-3d48-4624-afbf-7270bb6a9d28"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e5e7f54-c3af-486e-a3fb-6424dfc92931"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""b47e43bc-944b-4af3-861f-0e33945cfd3c"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""f1fc0f06-c9ca-47da-8400-ab6ac7b49bf4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7b434ecb-b64f-46ee-9d58-b60753bcfc59"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_MouseX = m_Gameplay.FindAction("MouseX", throwIfNotFound: true);
        m_Gameplay_MouseY = m_Gameplay.FindAction("MouseY", throwIfNotFound: true);
        m_Gameplay_Shoot = m_Gameplay.FindAction("Shoot", throwIfNotFound: true);
        m_Gameplay_Abilities1 = m_Gameplay.FindAction("Abilities 1", throwIfNotFound: true);
        m_Gameplay_Abilities2 = m_Gameplay.FindAction("Abilities 2", throwIfNotFound: true);
        m_Gameplay_Abilities3 = m_Gameplay.FindAction("Abilities 3", throwIfNotFound: true);
        m_Gameplay_Shield = m_Gameplay.FindAction("Shield", throwIfNotFound: true);
        m_Gameplay_Escape = m_Gameplay.FindAction("Escape", throwIfNotFound: true);
        // Spectator
        m_Spectator = asset.FindActionMap("Spectator", throwIfNotFound: true);
        m_Spectator_Move = m_Spectator.FindAction("Move", throwIfNotFound: true);
        m_Spectator_MouseX = m_Spectator.FindAction("MouseX", throwIfNotFound: true);
        m_Spectator_MouseY = m_Spectator.FindAction("MouseY", throwIfNotFound: true);
        m_Spectator_Escape = m_Spectator.FindAction("Escape", throwIfNotFound: true);
        m_Spectator_Sprint = m_Spectator.FindAction("Sprint", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Newaction = m_Menu.FindAction("New action", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_MouseX;
    private readonly InputAction m_Gameplay_MouseY;
    private readonly InputAction m_Gameplay_Shoot;
    private readonly InputAction m_Gameplay_Abilities1;
    private readonly InputAction m_Gameplay_Abilities2;
    private readonly InputAction m_Gameplay_Abilities3;
    private readonly InputAction m_Gameplay_Shield;
    private readonly InputAction m_Gameplay_Escape;
    public struct GameplayActions
    {
        private @Controls m_Wrapper;
        public GameplayActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @MouseX => m_Wrapper.m_Gameplay_MouseX;
        public InputAction @MouseY => m_Wrapper.m_Gameplay_MouseY;
        public InputAction @Shoot => m_Wrapper.m_Gameplay_Shoot;
        public InputAction @Abilities1 => m_Wrapper.m_Gameplay_Abilities1;
        public InputAction @Abilities2 => m_Wrapper.m_Gameplay_Abilities2;
        public InputAction @Abilities3 => m_Wrapper.m_Gameplay_Abilities3;
        public InputAction @Shield => m_Wrapper.m_Gameplay_Shield;
        public InputAction @Escape => m_Wrapper.m_Gameplay_Escape;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @MouseX.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouseX;
                @MouseX.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouseX;
                @MouseX.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouseX;
                @MouseY.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouseY;
                @MouseY.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouseY;
                @MouseY.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouseY;
                @Shoot.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                @Abilities1.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilities1;
                @Abilities1.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilities1;
                @Abilities1.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilities1;
                @Abilities2.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilities2;
                @Abilities2.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilities2;
                @Abilities2.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilities2;
                @Abilities3.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilities3;
                @Abilities3.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilities3;
                @Abilities3.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilities3;
                @Shield.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShield;
                @Shield.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShield;
                @Shield.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShield;
                @Escape.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEscape;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @MouseX.started += instance.OnMouseX;
                @MouseX.performed += instance.OnMouseX;
                @MouseX.canceled += instance.OnMouseX;
                @MouseY.started += instance.OnMouseY;
                @MouseY.performed += instance.OnMouseY;
                @MouseY.canceled += instance.OnMouseY;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Abilities1.started += instance.OnAbilities1;
                @Abilities1.performed += instance.OnAbilities1;
                @Abilities1.canceled += instance.OnAbilities1;
                @Abilities2.started += instance.OnAbilities2;
                @Abilities2.performed += instance.OnAbilities2;
                @Abilities2.canceled += instance.OnAbilities2;
                @Abilities3.started += instance.OnAbilities3;
                @Abilities3.performed += instance.OnAbilities3;
                @Abilities3.canceled += instance.OnAbilities3;
                @Shield.started += instance.OnShield;
                @Shield.performed += instance.OnShield;
                @Shield.canceled += instance.OnShield;
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Spectator
    private readonly InputActionMap m_Spectator;
    private ISpectatorActions m_SpectatorActionsCallbackInterface;
    private readonly InputAction m_Spectator_Move;
    private readonly InputAction m_Spectator_MouseX;
    private readonly InputAction m_Spectator_MouseY;
    private readonly InputAction m_Spectator_Escape;
    private readonly InputAction m_Spectator_Sprint;
    public struct SpectatorActions
    {
        private @Controls m_Wrapper;
        public SpectatorActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Spectator_Move;
        public InputAction @MouseX => m_Wrapper.m_Spectator_MouseX;
        public InputAction @MouseY => m_Wrapper.m_Spectator_MouseY;
        public InputAction @Escape => m_Wrapper.m_Spectator_Escape;
        public InputAction @Sprint => m_Wrapper.m_Spectator_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_Spectator; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SpectatorActions set) { return set.Get(); }
        public void SetCallbacks(ISpectatorActions instance)
        {
            if (m_Wrapper.m_SpectatorActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnMove;
                @MouseX.started -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnMouseX;
                @MouseX.performed -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnMouseX;
                @MouseX.canceled -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnMouseX;
                @MouseY.started -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnMouseY;
                @MouseY.performed -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnMouseY;
                @MouseY.canceled -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnMouseY;
                @Escape.started -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnEscape;
                @Sprint.started -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_SpectatorActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_SpectatorActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @MouseX.started += instance.OnMouseX;
                @MouseX.performed += instance.OnMouseX;
                @MouseX.canceled += instance.OnMouseX;
                @MouseY.started += instance.OnMouseY;
                @MouseY.performed += instance.OnMouseY;
                @MouseY.canceled += instance.OnMouseY;
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public SpectatorActions @Spectator => new SpectatorActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Newaction;
    public struct MenuActions
    {
        private @Controls m_Wrapper;
        public MenuActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Menu_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnMouseX(InputAction.CallbackContext context);
        void OnMouseY(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnAbilities1(InputAction.CallbackContext context);
        void OnAbilities2(InputAction.CallbackContext context);
        void OnAbilities3(InputAction.CallbackContext context);
        void OnShield(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
    }
    public interface ISpectatorActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMouseX(InputAction.CallbackContext context);
        void OnMouseY(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
