// GENERATED AUTOMATICALLY FROM 'Assets/Settings/ControlsMap.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ControlsMap : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControlsMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControlsMap"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""7a52ae9b-b088-4c31-8dad-4e717b506bc1"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""3a570890-6bce-40b8-9dc7-fb2658cdb5ae"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""5404f5aa-1bed-45ce-99af-ed22d4776049"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponsWheel"",
                    ""type"": ""Button"",
                    ""id"": ""64204cae-73f5-4639-91b3-4e80ad6df4fd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WeaponsSelection"",
                    ""type"": ""Value"",
                    ""id"": ""ae79f8c8-26a0-472b-9915-7543a398b048"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AimDirection"",
                    ""type"": ""Value"",
                    ""id"": ""d28e69c8-0e41-4123-ba3e-29e03c22c872"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePos"",
                    ""type"": ""Value"",
                    ""id"": ""0ae940dc-265f-4377-acfa-2e1ed5c5685c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shot"",
                    ""type"": ""Button"",
                    ""id"": ""67831410-f105-485d-a534-b6803c12bf92"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryShot"",
                    ""type"": ""Button"",
                    ""id"": ""b47cbd01-7958-4388-8529-6a56d80051d7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""beb976ec-b064-4bef-9209-86682fb0558a"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""7e4ae24b-cff2-41c7-b76e-9b372057f16a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""42b1e176-0cbd-450e-ba5e-86301ac81afb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""83e3ae40-32c1-4f7d-8b09-c43159f01bec"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""226319b7-7b6d-44f3-a702-6012d47aa076"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d2acea4c-e8c1-4d48-becf-2466f02a57bc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""41dd8197-6eca-499b-a96c-568f8ac5c1a4"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45e6bb53-f0c5-4b22-a5e5-2f5221d81035"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac3585ed-407a-4ea5-af2a-dcb5364e1b50"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""WeaponsWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08836fbf-7e54-4720-b43f-29f6630c7415"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""WeaponsWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c23bcfd3-a994-443f-9c1c-237f1a67862c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""WeaponsSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9cea776-403b-4d41-8df7-671cbd43de1e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AimDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f524f96-4806-46f9-a2c7-ad7208be4bc6"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MousePos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ecf8e29-b777-40a0-9ced-d193d80a1401"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f792005e-05aa-4d17-8f1f-363400c79fea"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Shot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf9725cc-9480-4d61-992f-547a649875c1"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""SecondaryShot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f0c25ea-0463-4dfa-93fc-7f9d24f27b97"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SecondaryShot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": []
        },
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": []
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
        m_Gameplay_Dash = m_Gameplay.FindAction("Dash", throwIfNotFound: true);
        m_Gameplay_WeaponsWheel = m_Gameplay.FindAction("WeaponsWheel", throwIfNotFound: true);
        m_Gameplay_WeaponsSelection = m_Gameplay.FindAction("WeaponsSelection", throwIfNotFound: true);
        m_Gameplay_AimDirection = m_Gameplay.FindAction("AimDirection", throwIfNotFound: true);
        m_Gameplay_MousePos = m_Gameplay.FindAction("MousePos", throwIfNotFound: true);
        m_Gameplay_Shot = m_Gameplay.FindAction("Shot", throwIfNotFound: true);
        m_Gameplay_SecondaryShot = m_Gameplay.FindAction("SecondaryShot", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Movement;
    private readonly InputAction m_Gameplay_Dash;
    private readonly InputAction m_Gameplay_WeaponsWheel;
    private readonly InputAction m_Gameplay_WeaponsSelection;
    private readonly InputAction m_Gameplay_AimDirection;
    private readonly InputAction m_Gameplay_MousePos;
    private readonly InputAction m_Gameplay_Shot;
    private readonly InputAction m_Gameplay_SecondaryShot;
    public struct GameplayActions
    {
        private @ControlsMap m_Wrapper;
        public GameplayActions(@ControlsMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputAction @Dash => m_Wrapper.m_Gameplay_Dash;
        public InputAction @WeaponsWheel => m_Wrapper.m_Gameplay_WeaponsWheel;
        public InputAction @WeaponsSelection => m_Wrapper.m_Gameplay_WeaponsSelection;
        public InputAction @AimDirection => m_Wrapper.m_Gameplay_AimDirection;
        public InputAction @MousePos => m_Wrapper.m_Gameplay_MousePos;
        public InputAction @Shot => m_Wrapper.m_Gameplay_Shot;
        public InputAction @SecondaryShot => m_Wrapper.m_Gameplay_SecondaryShot;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @WeaponsWheel.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeaponsWheel;
                @WeaponsWheel.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeaponsWheel;
                @WeaponsWheel.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeaponsWheel;
                @WeaponsSelection.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeaponsSelection;
                @WeaponsSelection.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeaponsSelection;
                @WeaponsSelection.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeaponsSelection;
                @AimDirection.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAimDirection;
                @AimDirection.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAimDirection;
                @AimDirection.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAimDirection;
                @MousePos.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMousePos;
                @MousePos.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMousePos;
                @MousePos.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMousePos;
                @Shot.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShot;
                @Shot.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShot;
                @Shot.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShot;
                @SecondaryShot.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryShot;
                @SecondaryShot.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryShot;
                @SecondaryShot.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryShot;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @WeaponsWheel.started += instance.OnWeaponsWheel;
                @WeaponsWheel.performed += instance.OnWeaponsWheel;
                @WeaponsWheel.canceled += instance.OnWeaponsWheel;
                @WeaponsSelection.started += instance.OnWeaponsSelection;
                @WeaponsSelection.performed += instance.OnWeaponsSelection;
                @WeaponsSelection.canceled += instance.OnWeaponsSelection;
                @AimDirection.started += instance.OnAimDirection;
                @AimDirection.performed += instance.OnAimDirection;
                @AimDirection.canceled += instance.OnAimDirection;
                @MousePos.started += instance.OnMousePos;
                @MousePos.performed += instance.OnMousePos;
                @MousePos.canceled += instance.OnMousePos;
                @Shot.started += instance.OnShot;
                @Shot.performed += instance.OnShot;
                @Shot.canceled += instance.OnShot;
                @SecondaryShot.started += instance.OnSecondaryShot;
                @SecondaryShot.performed += instance.OnSecondaryShot;
                @SecondaryShot.canceled += instance.OnSecondaryShot;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnWeaponsWheel(InputAction.CallbackContext context);
        void OnWeaponsSelection(InputAction.CallbackContext context);
        void OnAimDirection(InputAction.CallbackContext context);
        void OnMousePos(InputAction.CallbackContext context);
        void OnShot(InputAction.CallbackContext context);
        void OnSecondaryShot(InputAction.CallbackContext context);
    }
}
