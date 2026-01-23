using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;
    public static event Action OnInteract;
    public static event Action OnBreakBeginning;
    public static event Action OnBreakCanceled;
    public static event Action<int> OnToolPick;
    private InputControllers _inputSource;


    private void Awake()
    {
        _inputSource = new InputControllers();
        Instance = this;
    }

    private void StartBreak(InputAction.CallbackContext context)
    {
        OnBreakBeginning?.Invoke();
    }
    private void CancelBreak(InputAction.CallbackContext context)
    {
        OnBreakCanceled?.Invoke();
    }

    private void InteractAction(InputAction.CallbackContext context)
    {
        OnInteract?.Invoke();
    }

    private void OnEnable()
    {
        _inputSource.Enable();
        _inputSource.Player.Interact.performed += InteractAction;
        _inputSource.Player.BreakStart.performed += StartBreak;
        _inputSource.Player.BreakStart.canceled += CancelBreak;
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
        _inputSource.Player.Interact.performed -= InteractAction;
        _inputSource.Player.BreakStart.performed -= StartBreak;
        _inputSource.Player.BreakStart.canceled -= CancelBreak;
        _inputSource.Player.ToolPick.performed -= ToolPickMenu;
    }
}
