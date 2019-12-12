// GENERATED AUTOMATICALLY FROM 'Assets/Input/InputController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputController : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @InputController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputController"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""3ce62785-afe1-48aa-a789-17f42a1d9fe2"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""6c4401d3-99eb-44aa-84d3-14a41f4f1b23"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""d997ed2a-014c-4ce7-8c5a-6114f2d067dc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CaughtObject"",
                    ""type"": ""Button"",
                    ""id"": ""bdcb96b9-5a8e-4cc6-9443-3bd9bd198b40"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""758b21f3-01f5-4b9b-95dd-38fae4815217"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""734d64d0-4a9d-4f6f-ba99-06313b05dde5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HoldLv1"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a14da7a4-e6f4-4021-99ad-dd91a58fa38e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=1)""
                },
                {
                    ""name"": ""HoldLv2"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fda40f4c-cf97-4128-8194-a94e2c921263"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=2)""
                },
                {
                    ""name"": ""CaugthLv1"",
                    ""type"": ""Button"",
                    ""id"": ""17f9f7a6-2637-41d3-8543-1f7fb3ec79e3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.5)""
                },
                {
                    ""name"": ""TestAction"",
                    ""type"": ""Button"",
                    ""id"": ""ad2ec385-8e18-4d3a-97cb-41ca2c985f10"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""4815b5cf-b69c-4360-994d-2d6ddfaf91ab"",
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
                    ""id"": ""e8521969-6a15-4409-9120-f25b7bd4d24c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a5ce2c77-64e4-4d5b-b396-954da01fbd81"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""84fbd08e-a13c-46af-836f-0d518caa4f06"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""07c250b7-3f09-4224-af37-201f74406ad8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c13eeebd-b6d7-4413-aee0-4e1dadddf5c5"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f580cbe8-3505-471c-9375-edfa030747ec"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a926bd38-89af-4eaa-9707-38a93f982374"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""099e0e9d-b1a5-4d43-8b38-f60fe4a05631"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CaughtObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbceb50f-526e-4207-8de6-316c1d8f5710"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CaughtObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a77d042f-1c27-4b2e-97a9-78f160ab8afc"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f840c5d-eb6c-4be5-90b0-f55abb108018"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""635c9472-b638-41c8-a16c-d0e135621a0b"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""845ddbe3-1e0c-43f8-83cb-abd23dbac950"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldLv1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6907ded2-fe5d-47b0-8b1f-87b52aa4bd24"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldLv1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""549c6965-81c3-46d2-9cfb-901f6bb45de3"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldLv2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8c32699-0131-4547-9f79-41f1ee784b67"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldLv2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef6d0f18-fa30-4a36-95eb-d8dc56295a77"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TestAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e243ebbc-f4e3-4d70-ab8d-f0b34ca0f028"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CaugthLv1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f950a2a-b469-4740-88f7-f0071e2dedfd"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CaugthLv1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Movement = m_Character.FindAction("Movement", throwIfNotFound: true);
        m_Character_Fire = m_Character.FindAction("Fire", throwIfNotFound: true);
        m_Character_CaughtObject = m_Character.FindAction("CaughtObject", throwIfNotFound: true);
        m_Character_Aim = m_Character.FindAction("Aim", throwIfNotFound: true);
        m_Character_Dash = m_Character.FindAction("Dash", throwIfNotFound: true);
        m_Character_HoldLv1 = m_Character.FindAction("HoldLv1", throwIfNotFound: true);
        m_Character_HoldLv2 = m_Character.FindAction("HoldLv2", throwIfNotFound: true);
        m_Character_CaugthLv1 = m_Character.FindAction("CaugthLv1", throwIfNotFound: true);
        m_Character_TestAction = m_Character.FindAction("TestAction", throwIfNotFound: true);
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

    // Character
    private readonly InputActionMap m_Character;
    private ICharacterActions m_CharacterActionsCallbackInterface;
    private readonly InputAction m_Character_Movement;
    private readonly InputAction m_Character_Fire;
    private readonly InputAction m_Character_CaughtObject;
    private readonly InputAction m_Character_Aim;
    private readonly InputAction m_Character_Dash;
    private readonly InputAction m_Character_HoldLv1;
    private readonly InputAction m_Character_HoldLv2;
    private readonly InputAction m_Character_CaugthLv1;
    private readonly InputAction m_Character_TestAction;
    public struct CharacterActions
    {
        private @InputController m_Wrapper;
        public CharacterActions(@InputController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Character_Movement;
        public InputAction @Fire => m_Wrapper.m_Character_Fire;
        public InputAction @CaughtObject => m_Wrapper.m_Character_CaughtObject;
        public InputAction @Aim => m_Wrapper.m_Character_Aim;
        public InputAction @Dash => m_Wrapper.m_Character_Dash;
        public InputAction @HoldLv1 => m_Wrapper.m_Character_HoldLv1;
        public InputAction @HoldLv2 => m_Wrapper.m_Character_HoldLv2;
        public InputAction @CaugthLv1 => m_Wrapper.m_Character_CaugthLv1;
        public InputAction @TestAction => m_Wrapper.m_Character_TestAction;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Fire.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnFire;
                @CaughtObject.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCaughtObject;
                @CaughtObject.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCaughtObject;
                @CaughtObject.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCaughtObject;
                @Aim.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                @Dash.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDash;
                @HoldLv1.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHoldLv1;
                @HoldLv1.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHoldLv1;
                @HoldLv1.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHoldLv1;
                @HoldLv2.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHoldLv2;
                @HoldLv2.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHoldLv2;
                @HoldLv2.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHoldLv2;
                @CaugthLv1.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCaugthLv1;
                @CaugthLv1.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCaugthLv1;
                @CaugthLv1.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnCaugthLv1;
                @TestAction.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnTestAction;
                @TestAction.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnTestAction;
                @TestAction.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnTestAction;
            }
            m_Wrapper.m_CharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @CaughtObject.started += instance.OnCaughtObject;
                @CaughtObject.performed += instance.OnCaughtObject;
                @CaughtObject.canceled += instance.OnCaughtObject;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @HoldLv1.started += instance.OnHoldLv1;
                @HoldLv1.performed += instance.OnHoldLv1;
                @HoldLv1.canceled += instance.OnHoldLv1;
                @HoldLv2.started += instance.OnHoldLv2;
                @HoldLv2.performed += instance.OnHoldLv2;
                @HoldLv2.canceled += instance.OnHoldLv2;
                @CaugthLv1.started += instance.OnCaugthLv1;
                @CaugthLv1.performed += instance.OnCaugthLv1;
                @CaugthLv1.canceled += instance.OnCaugthLv1;
                @TestAction.started += instance.OnTestAction;
                @TestAction.performed += instance.OnTestAction;
                @TestAction.canceled += instance.OnTestAction;
            }
        }
    }
    public CharacterActions @Character => new CharacterActions(this);
    public interface ICharacterActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnCaughtObject(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnHoldLv1(InputAction.CallbackContext context);
        void OnHoldLv2(InputAction.CallbackContext context);
        void OnCaugthLv1(InputAction.CallbackContext context);
        void OnTestAction(InputAction.CallbackContext context);
    }
}
