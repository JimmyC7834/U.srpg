using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IMapNaviActions ,GameInput.IMenuNaviActions
{
    // MapNavi
    public event UnityAction<Vector2> cursorMoveEvent = delegate { };
    public event UnityAction cursorConfirmEvent = delegate { };
    public event UnityAction cursorCancelEvent = delegate { };

    // Menus
    public event UnityAction<Vector2> menuMoveEvent = delegate { };
    public event UnityAction menuConfirmEvent = delegate { };
    public event UnityAction menuCancelEvent = delegate { };

    // !!! Remember to edit Input Reader functions upon updating the input map !!!
    private GameInput gameInput;

    private void OnEnable() {
        if (gameInput == null) {
            gameInput = new GameInput();

            gameInput.MapNavi.SetCallbacks(this);
            gameInput.MenuNavi.SetCallbacks(this);
        }

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
    public void OnPointerMove(InputAction.CallbackContext context)
    {
        menuMoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPointerConfirm(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            menuConfirmEvent.Invoke();
    }

    public void OnPointerCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            menuCancelEvent.Invoke();
    }

    // Input Reader
    public void EnableMapNaviInput() {
        DisableAllInput();

        gameInput.MapNavi.Enable();
    }

    public void EnableMenuNaviInput() {
        DisableAllInput();

        gameInput.MenuNavi.Enable();
    }

    public void DisableAllInput() {
        gameInput.MenuNavi.Disable();
        gameInput.MapNavi.Disable();
    }
}
