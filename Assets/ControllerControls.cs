//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Samples/XR Interaction Toolkit/3.0.3/Starter Assets/ControllerControls.inputactions
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

public partial class @ControllerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControllerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControllerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""cb1108cf-1694-4933-8175-a1b71fd0dbfd"",
            ""actions"": [
                {
                    ""name"": ""leftsectrigger"",
                    ""type"": ""Button"",
                    ""id"": ""d26b1253-748e-40b7-a666-bea1615b7dba"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""rightsectrigger"",
                    ""type"": ""Button"",
                    ""id"": ""85fb0dbb-6451-4859-9850-16fbba3775e3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""lefttriggerheld"",
                    ""type"": ""Value"",
                    ""id"": ""8e9fcbd2-85d2-4e2a-ac6e-0001c4553801"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""righttriggerheld"",
                    ""type"": ""Value"",
                    ""id"": ""eaa1e01b-3402-4cc9-a849-2615f4c7e2ef"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""B"",
                    ""type"": ""Button"",
                    ""id"": ""b1f771c3-b8fe-4911-9ee6-f10ff42ed654"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a7c614d7-25c2-4faf-8d1c-626a228bc1b9"",
                    ""path"": ""<XRController>{LeftHand}/{TriggerButton}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""leftsectrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""065397cd-b371-4fe4-9bad-1318fa8a15b7"",
                    ""path"": ""<XRController>{RightHand}/{TriggerButton}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""rightsectrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e901792-8cc2-4773-b5a1-9a5621c7f58b"",
                    ""path"": ""<XRController>{LeftHand}/{Trigger}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""lefttriggerheld"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04aee182-4d12-4a59-a796-0840a1daa0e8"",
                    ""path"": ""<XRController>{RightHand}/{Trigger}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""righttriggerheld"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fabb01e1-e0b6-406b-b427-8230d6ea9624"",
                    ""path"": ""<XRController>{RightHand}/{SecondaryButton}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""B"",
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
        m_Gameplay_leftsectrigger = m_Gameplay.FindAction("leftsectrigger", throwIfNotFound: true);
        m_Gameplay_rightsectrigger = m_Gameplay.FindAction("rightsectrigger", throwIfNotFound: true);
        m_Gameplay_lefttriggerheld = m_Gameplay.FindAction("lefttriggerheld", throwIfNotFound: true);
        m_Gameplay_righttriggerheld = m_Gameplay.FindAction("righttriggerheld", throwIfNotFound: true);
        m_Gameplay_B = m_Gameplay.FindAction("B", throwIfNotFound: true);
    }

    ~@ControllerControls()
    {
        UnityEngine.Debug.Assert(!m_Gameplay.enabled, "This will cause a leak and performance issues, ControllerControls.Gameplay.Disable() has not been called.");
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
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_Gameplay_leftsectrigger;
    private readonly InputAction m_Gameplay_rightsectrigger;
    private readonly InputAction m_Gameplay_lefttriggerheld;
    private readonly InputAction m_Gameplay_righttriggerheld;
    private readonly InputAction m_Gameplay_B;
    public struct GameplayActions
    {
        private @ControllerControls m_Wrapper;
        public GameplayActions(@ControllerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @leftsectrigger => m_Wrapper.m_Gameplay_leftsectrigger;
        public InputAction @rightsectrigger => m_Wrapper.m_Gameplay_rightsectrigger;
        public InputAction @lefttriggerheld => m_Wrapper.m_Gameplay_lefttriggerheld;
        public InputAction @righttriggerheld => m_Wrapper.m_Gameplay_righttriggerheld;
        public InputAction @B => m_Wrapper.m_Gameplay_B;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @leftsectrigger.started += instance.OnLeftsectrigger;
            @leftsectrigger.performed += instance.OnLeftsectrigger;
            @leftsectrigger.canceled += instance.OnLeftsectrigger;
            @rightsectrigger.started += instance.OnRightsectrigger;
            @rightsectrigger.performed += instance.OnRightsectrigger;
            @rightsectrigger.canceled += instance.OnRightsectrigger;
            @lefttriggerheld.started += instance.OnLefttriggerheld;
            @lefttriggerheld.performed += instance.OnLefttriggerheld;
            @lefttriggerheld.canceled += instance.OnLefttriggerheld;
            @righttriggerheld.started += instance.OnRighttriggerheld;
            @righttriggerheld.performed += instance.OnRighttriggerheld;
            @righttriggerheld.canceled += instance.OnRighttriggerheld;
            @B.started += instance.OnB;
            @B.performed += instance.OnB;
            @B.canceled += instance.OnB;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @leftsectrigger.started -= instance.OnLeftsectrigger;
            @leftsectrigger.performed -= instance.OnLeftsectrigger;
            @leftsectrigger.canceled -= instance.OnLeftsectrigger;
            @rightsectrigger.started -= instance.OnRightsectrigger;
            @rightsectrigger.performed -= instance.OnRightsectrigger;
            @rightsectrigger.canceled -= instance.OnRightsectrigger;
            @lefttriggerheld.started -= instance.OnLefttriggerheld;
            @lefttriggerheld.performed -= instance.OnLefttriggerheld;
            @lefttriggerheld.canceled -= instance.OnLefttriggerheld;
            @righttriggerheld.started -= instance.OnRighttriggerheld;
            @righttriggerheld.performed -= instance.OnRighttriggerheld;
            @righttriggerheld.canceled -= instance.OnRighttriggerheld;
            @B.started -= instance.OnB;
            @B.performed -= instance.OnB;
            @B.canceled -= instance.OnB;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnLeftsectrigger(InputAction.CallbackContext context);
        void OnRightsectrigger(InputAction.CallbackContext context);
        void OnLefttriggerheld(InputAction.CallbackContext context);
        void OnRighttriggerheld(InputAction.CallbackContext context);
        void OnB(InputAction.CallbackContext context);
    }
}