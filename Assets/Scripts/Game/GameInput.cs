//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/InputReader/GameInput.inputactions
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

public partial class @GameInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""MapNavi"",
            ""id"": ""304cdaf5-725e-4c1e-b3fc-f591fc040b6e"",
            ""actions"": [
                {
                    ""name"": ""CursorMove"",
                    ""type"": ""Value"",
                    ""id"": ""5a3ae759-c95f-47e7-810e-1b96da3a0c31"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CursorConfirm"",
                    ""type"": ""Button"",
                    ""id"": ""b83c6c04-f197-4363-9a7b-959b7e24084d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CursorCancel"",
                    ""type"": ""Button"",
                    ""id"": ""25c4dfc7-07e5-48bf-8060-0418f2392a91"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrows"",
                    ""id"": ""eb030a7a-ec9c-4290-b039-00cb9fd1c0c4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3defdebb-4962-431b-bda6-fb666031a129"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f387f271-337d-4160-b25b-f7155e208eb4"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6000d403-74c0-4247-b927-b79615c08138"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""01326e50-91cb-4c51-b446-fda028a8da9b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8f719736-aa4b-434a-af7c-1a12d1159fe7"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorConfirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9b418510-dbad-43f0-9d65-024f7514f644"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorCancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MenuNavi"",
            ""id"": ""084934ca-5ac8-4944-83cf-d5a7ef977a2b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""234c6d3e-0b5a-4767-a405-57ff51b9ec3c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""b46ab431-a252-49f8-8390-8670f98240de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""0a1a935e-8a73-4ccd-a4d8-ff11ff7a4ee5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""dbd68b43-12df-437b-aadd-5c6c6e420e8f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""af72b70d-81bf-47f6-b73a-123549f04ad8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrows"",
                    ""id"": ""ad13ddca-9114-4560-b93a-8850f8745394"",
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
                    ""id"": ""0c39dab6-faab-4015-b426-2fcf89f303bb"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8b2ef2e5-c244-4706-b302-135caa5a39bd"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""61e989f9-861c-4baf-872b-f799df1aef5b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4302ada3-32cb-4f3b-a9ee-8016a975274a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""59676708-91bc-4172-a002-64689aeec18b"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c9e38c2-4c77-45b7-b241-ad1f259ec3c6"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec8c4612-8699-441d-b869-b1fda0931671"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32a18d93-dd52-4f67-b4b1-b9cb829df759"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""DebugConsole"",
            ""id"": ""b520142b-8f20-490a-b4f9-6b3a3d95d181"",
            ""actions"": [
                {
                    ""name"": ""ToggleDebug"",
                    ""type"": ""Button"",
                    ""id"": ""ac9746b8-38d2-402c-af41-caa65062836d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ff1b97f2-570a-4ae0-9ca4-45ac831695b3"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleDebug"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BoardEditor"",
            ""id"": ""9bec351f-f139-47dd-b5aa-3209182a43b8"",
            ""actions"": [
                {
                    ""name"": ""PlaceTerrain"",
                    ""type"": ""Button"",
                    ""id"": ""0debccfb-9e8b-4fb2-8fb3-ccc928883902"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseMove"",
                    ""type"": ""Value"",
                    ""id"": ""89b7b5e6-5944-4cf5-80a7-c25e3efbbf3e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3770afc2-59d0-40ef-bf7a-81600c2c0976"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlaceTerrain"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17ff13bf-e98a-427a-a178-1b0fcbd83748"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MapNavi
        m_MapNavi = asset.FindActionMap("MapNavi", throwIfNotFound: true);
        m_MapNavi_CursorMove = m_MapNavi.FindAction("CursorMove", throwIfNotFound: true);
        m_MapNavi_CursorConfirm = m_MapNavi.FindAction("CursorConfirm", throwIfNotFound: true);
        m_MapNavi_CursorCancel = m_MapNavi.FindAction("CursorCancel", throwIfNotFound: true);
        // MenuNavi
        m_MenuNavi = asset.FindActionMap("MenuNavi", throwIfNotFound: true);
        m_MenuNavi_Move = m_MenuNavi.FindAction("Move", throwIfNotFound: true);
        m_MenuNavi_Confirm = m_MenuNavi.FindAction("Confirm", throwIfNotFound: true);
        m_MenuNavi_Cancel = m_MenuNavi.FindAction("Cancel", throwIfNotFound: true);
        m_MenuNavi_Up = m_MenuNavi.FindAction("Up", throwIfNotFound: true);
        m_MenuNavi_Left = m_MenuNavi.FindAction("Left", throwIfNotFound: true);
        // DebugConsole
        m_DebugConsole = asset.FindActionMap("DebugConsole", throwIfNotFound: true);
        m_DebugConsole_ToggleDebug = m_DebugConsole.FindAction("ToggleDebug", throwIfNotFound: true);
        // BoardEditor
        m_BoardEditor = asset.FindActionMap("BoardEditor", throwIfNotFound: true);
        m_BoardEditor_PlaceTerrain = m_BoardEditor.FindAction("PlaceTerrain", throwIfNotFound: true);
        m_BoardEditor_MouseMove = m_BoardEditor.FindAction("MouseMove", throwIfNotFound: true);
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

    // MapNavi
    private readonly InputActionMap m_MapNavi;
    private IMapNaviActions m_MapNaviActionsCallbackInterface;
    private readonly InputAction m_MapNavi_CursorMove;
    private readonly InputAction m_MapNavi_CursorConfirm;
    private readonly InputAction m_MapNavi_CursorCancel;
    public struct MapNaviActions
    {
        private @GameInput m_Wrapper;
        public MapNaviActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @CursorMove => m_Wrapper.m_MapNavi_CursorMove;
        public InputAction @CursorConfirm => m_Wrapper.m_MapNavi_CursorConfirm;
        public InputAction @CursorCancel => m_Wrapper.m_MapNavi_CursorCancel;
        public InputActionMap Get() { return m_Wrapper.m_MapNavi; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MapNaviActions set) { return set.Get(); }
        public void SetCallbacks(IMapNaviActions instance)
        {
            if (m_Wrapper.m_MapNaviActionsCallbackInterface != null)
            {
                @CursorMove.started -= m_Wrapper.m_MapNaviActionsCallbackInterface.OnCursorMove;
                @CursorMove.performed -= m_Wrapper.m_MapNaviActionsCallbackInterface.OnCursorMove;
                @CursorMove.canceled -= m_Wrapper.m_MapNaviActionsCallbackInterface.OnCursorMove;
                @CursorConfirm.started -= m_Wrapper.m_MapNaviActionsCallbackInterface.OnCursorConfirm;
                @CursorConfirm.performed -= m_Wrapper.m_MapNaviActionsCallbackInterface.OnCursorConfirm;
                @CursorConfirm.canceled -= m_Wrapper.m_MapNaviActionsCallbackInterface.OnCursorConfirm;
                @CursorCancel.started -= m_Wrapper.m_MapNaviActionsCallbackInterface.OnCursorCancel;
                @CursorCancel.performed -= m_Wrapper.m_MapNaviActionsCallbackInterface.OnCursorCancel;
                @CursorCancel.canceled -= m_Wrapper.m_MapNaviActionsCallbackInterface.OnCursorCancel;
            }
            m_Wrapper.m_MapNaviActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CursorMove.started += instance.OnCursorMove;
                @CursorMove.performed += instance.OnCursorMove;
                @CursorMove.canceled += instance.OnCursorMove;
                @CursorConfirm.started += instance.OnCursorConfirm;
                @CursorConfirm.performed += instance.OnCursorConfirm;
                @CursorConfirm.canceled += instance.OnCursorConfirm;
                @CursorCancel.started += instance.OnCursorCancel;
                @CursorCancel.performed += instance.OnCursorCancel;
                @CursorCancel.canceled += instance.OnCursorCancel;
            }
        }
    }
    public MapNaviActions @MapNavi => new MapNaviActions(this);

    // MenuNavi
    private readonly InputActionMap m_MenuNavi;
    private IMenuNaviActions m_MenuNaviActionsCallbackInterface;
    private readonly InputAction m_MenuNavi_Move;
    private readonly InputAction m_MenuNavi_Confirm;
    private readonly InputAction m_MenuNavi_Cancel;
    private readonly InputAction m_MenuNavi_Up;
    private readonly InputAction m_MenuNavi_Left;
    public struct MenuNaviActions
    {
        private @GameInput m_Wrapper;
        public MenuNaviActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_MenuNavi_Move;
        public InputAction @Confirm => m_Wrapper.m_MenuNavi_Confirm;
        public InputAction @Cancel => m_Wrapper.m_MenuNavi_Cancel;
        public InputAction @Up => m_Wrapper.m_MenuNavi_Up;
        public InputAction @Left => m_Wrapper.m_MenuNavi_Left;
        public InputActionMap Get() { return m_Wrapper.m_MenuNavi; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuNaviActions set) { return set.Get(); }
        public void SetCallbacks(IMenuNaviActions instance)
        {
            if (m_Wrapper.m_MenuNaviActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnMove;
                @Confirm.started -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnConfirm;
                @Confirm.performed -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnConfirm;
                @Confirm.canceled -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnConfirm;
                @Cancel.started -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnCancel;
                @Up.started -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnUp;
                @Up.performed -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnUp;
                @Up.canceled -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnUp;
                @Left.started -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnLeft;
                @Left.performed -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnLeft;
                @Left.canceled -= m_Wrapper.m_MenuNaviActionsCallbackInterface.OnLeft;
            }
            m_Wrapper.m_MenuNaviActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Confirm.started += instance.OnConfirm;
                @Confirm.performed += instance.OnConfirm;
                @Confirm.canceled += instance.OnConfirm;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Up.started += instance.OnUp;
                @Up.performed += instance.OnUp;
                @Up.canceled += instance.OnUp;
                @Left.started += instance.OnLeft;
                @Left.performed += instance.OnLeft;
                @Left.canceled += instance.OnLeft;
            }
        }
    }
    public MenuNaviActions @MenuNavi => new MenuNaviActions(this);

    // DebugConsole
    private readonly InputActionMap m_DebugConsole;
    private IDebugConsoleActions m_DebugConsoleActionsCallbackInterface;
    private readonly InputAction m_DebugConsole_ToggleDebug;
    public struct DebugConsoleActions
    {
        private @GameInput m_Wrapper;
        public DebugConsoleActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleDebug => m_Wrapper.m_DebugConsole_ToggleDebug;
        public InputActionMap Get() { return m_Wrapper.m_DebugConsole; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugConsoleActions set) { return set.Get(); }
        public void SetCallbacks(IDebugConsoleActions instance)
        {
            if (m_Wrapper.m_DebugConsoleActionsCallbackInterface != null)
            {
                @ToggleDebug.started -= m_Wrapper.m_DebugConsoleActionsCallbackInterface.OnToggleDebug;
                @ToggleDebug.performed -= m_Wrapper.m_DebugConsoleActionsCallbackInterface.OnToggleDebug;
                @ToggleDebug.canceled -= m_Wrapper.m_DebugConsoleActionsCallbackInterface.OnToggleDebug;
            }
            m_Wrapper.m_DebugConsoleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ToggleDebug.started += instance.OnToggleDebug;
                @ToggleDebug.performed += instance.OnToggleDebug;
                @ToggleDebug.canceled += instance.OnToggleDebug;
            }
        }
    }
    public DebugConsoleActions @DebugConsole => new DebugConsoleActions(this);

    // BoardEditor
    private readonly InputActionMap m_BoardEditor;
    private IBoardEditorActions m_BoardEditorActionsCallbackInterface;
    private readonly InputAction m_BoardEditor_PlaceTerrain;
    private readonly InputAction m_BoardEditor_MouseMove;
    public struct BoardEditorActions
    {
        private @GameInput m_Wrapper;
        public BoardEditorActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PlaceTerrain => m_Wrapper.m_BoardEditor_PlaceTerrain;
        public InputAction @MouseMove => m_Wrapper.m_BoardEditor_MouseMove;
        public InputActionMap Get() { return m_Wrapper.m_BoardEditor; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BoardEditorActions set) { return set.Get(); }
        public void SetCallbacks(IBoardEditorActions instance)
        {
            if (m_Wrapper.m_BoardEditorActionsCallbackInterface != null)
            {
                @PlaceTerrain.started -= m_Wrapper.m_BoardEditorActionsCallbackInterface.OnPlaceTerrain;
                @PlaceTerrain.performed -= m_Wrapper.m_BoardEditorActionsCallbackInterface.OnPlaceTerrain;
                @PlaceTerrain.canceled -= m_Wrapper.m_BoardEditorActionsCallbackInterface.OnPlaceTerrain;
                @MouseMove.started -= m_Wrapper.m_BoardEditorActionsCallbackInterface.OnMouseMove;
                @MouseMove.performed -= m_Wrapper.m_BoardEditorActionsCallbackInterface.OnMouseMove;
                @MouseMove.canceled -= m_Wrapper.m_BoardEditorActionsCallbackInterface.OnMouseMove;
            }
            m_Wrapper.m_BoardEditorActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PlaceTerrain.started += instance.OnPlaceTerrain;
                @PlaceTerrain.performed += instance.OnPlaceTerrain;
                @PlaceTerrain.canceled += instance.OnPlaceTerrain;
                @MouseMove.started += instance.OnMouseMove;
                @MouseMove.performed += instance.OnMouseMove;
                @MouseMove.canceled += instance.OnMouseMove;
            }
        }
    }
    public BoardEditorActions @BoardEditor => new BoardEditorActions(this);
    public interface IMapNaviActions
    {
        void OnCursorMove(InputAction.CallbackContext context);
        void OnCursorConfirm(InputAction.CallbackContext context);
        void OnCursorCancel(InputAction.CallbackContext context);
    }
    public interface IMenuNaviActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnConfirm(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnUp(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
    }
    public interface IDebugConsoleActions
    {
        void OnToggleDebug(InputAction.CallbackContext context);
    }
    public interface IBoardEditorActions
    {
        void OnPlaceTerrain(InputAction.CallbackContext context);
        void OnMouseMove(InputAction.CallbackContext context);
    }
}
