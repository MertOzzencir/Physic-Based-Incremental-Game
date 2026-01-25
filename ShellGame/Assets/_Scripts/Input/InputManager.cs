using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;
    public static event Action<bool> OnLeftClickAction;
    public static event Action<bool> OnRightClickAction;
    public static event Action<int> OnToolPick;
    private InputControllers _inputSource;


    private void Awake()
    {
        _inputSource = new InputControllers();
        Instance = this;
    }

    private void RightClickAction(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
            OnRightClickAction?.Invoke(true);
        else if (ctx.phase == InputActionPhase.Canceled)
            OnRightClickAction?.Invoke(false);
    }
    private void LeftClickStateClickAction(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
            OnLeftClickAction?.Invoke(true);
        else if (ctx.phase == InputActionPhase.Canceled)
            OnLeftClickAction?.Invoke(false);
    }


    private void OnEnable()
    {
        _inputSource.Enable();
        _inputSource.Player.Interact.performed += LeftClickStateClickAction;
        _inputSource.Player.Interact.canceled += LeftClickStateClickAction;

        _inputSource.Player.BreakStart.performed += RightClickAction;
        _inputSource.Player.BreakStart.canceled += RightClickAction;

        _inputSource.Player.ToolPick.performed += ToolPickMenu;
    }

    public void ToolPickMenu(InputAction.CallbackContext context)
    {
        if (int.TryParse(context.control.name, out int index))
            OnToolPick?.Invoke(index);

    }
    public void ExternalToolPickMenu(int index)
    {
        OnToolPick?.Invoke(index);
    }

    private void OnDisable()
    {
        _inputSource.Disable();
        _inputSource.Player.Interact.performed -= LeftClickStateClickAction;
        _inputSource.Player.Interact.canceled -= LeftClickStateClickAction;
        _inputSource.Player.BreakStart.performed -= RightClickAction;
        _inputSource.Player.BreakStart.canceled -= RightClickAction;
        _inputSource.Player.ToolPick.performed -= ToolPickMenu;
    }
}
