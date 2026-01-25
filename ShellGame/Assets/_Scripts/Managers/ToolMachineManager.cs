using UnityEditor;
using UnityEngine;

public class ToolMachineManager : MonoBehaviour, IInteractable
{
    [SerializeField] private int toolKeyboardIndex;
    public Tools MachineTool;
    public Transform ToolPlacement;
    private ToolControllers toolController;
    void Start()
    {
        toolController = FindAnyObjectByType<ToolControllers>();
    }

    public void Interact()
    {
        toolController.SetTool(MachineTool);
    }
}
