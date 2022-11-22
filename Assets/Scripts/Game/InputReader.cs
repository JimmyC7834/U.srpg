using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, 
    GameInput.IMapNaviActions ,GameInput.IMenuNaviActions, GameInput.IDebugConsoleActions
{
    // MapNavi
    public event UnityAction<Vector2> cursorMoveEvent = delegate { };
    public event UnityAction cursorConfirmEvent = delegate { };
    public event UnityAction cursorCancelEvent = delegate { };

    // Menus
    public event UnityAction<Vector2> menuMoveEvent = delegate { };
    public event UnityAction menuConfirmEvent = delegate { };
    public event UnityAction menuCancelEvent = delegate { };
    public event UnityAction menuUpEvent = delegate { };
    public event UnityAction menuLeftEvent = delegate { };
    
    // Debug Console
    public event UnityAction toggleDebugConsole = delegate { };

    // // BoardEditor
    // public event UnityAction placeTerrainEvent = delegate { };
    // public event UnityAction<Vector2> boardEditorMouseMoveEvent = delegate { };

    // !!! Remember to edit Input Reader functions upon updating the input map !!!
    private GameInput gameInput;

    private void OnEnable() {
        if (gameInput == null) {
            gameInput = new GameInput();

            gameInput.MapNavi.SetCallbacks(this);
            gameInput.MenuNavi.SetCallbacks(this);
            // gameInput.BoardEditor.SetCallbacks(this);
        }

        // gameInput.BoardEditor.Enable();
        EnableMapNaviInput();
    }

    private void OnDisable() {

    }

    // -----MAPNAVI-----
    public void OnCursorMove(InputAction.CallbackContext context)
    {
        cursorMoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnCursorConfirm(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            cursorConfirmEvent.Invoke();
    }

    public void OnCursorCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            cursorCancelEvent.Invoke();
    }

    // -----MENUNAVI-----
    public void OnMove(InputAction.CallbackContext context)
    {
        menuMoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            menuConfirmEvent.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            menuCancelEvent.Invoke();
    }
    
    public void OnUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            menuUpEvent.Invoke();
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            menuLeftEvent.Invoke();
    }
    
    // Debug Console
    public void OnToggleDebugConsole(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            toggleDebugConsole.Invoke();
    }

    // BoardEditor
    // public void OnPlaceTerrain(InputAction.CallbackContext context)
    // {
    //     if (context.phase == InputActionPhase.Performed)
    //         placeTerrainEvent.Invoke();
    // }
    //
    // public void OnMouseMove(InputAction.CallbackContext context)
    // {
    //     boardEditorMouseMoveEvent.Invoke(context.ReadValue<Vector2>());
    // }
    
    // Input Reader
    public void EnableMapNaviInput() {
        DisableAllInput();

        gameInput.MapNavi.Enable();
    }

    public void EnableMenuNaviInput() {
        DisableAllInput();

        gameInput.MenuNavi.Enable();
    }
    
    // public void EnableBoardEditorInput() {
    //     DisableAllInput();
    //     gameInput.MapNavi.Enable();
    // }

    public void DisableAllInput() {
        gameInput.MenuNavi.Disable();
        gameInput.MapNavi.Disable();
    }
}
