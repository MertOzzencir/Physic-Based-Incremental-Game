using UnityEngine;

public class ToolMachineManager : MonoBehaviour, IInteractable
{
    [SerializeField] private int toolKeyboardIndex;

    public void Interact()
    {
        InputManager.Instance.ExternalToolPickMenu(toolKeyboardIndex);
    }
}
