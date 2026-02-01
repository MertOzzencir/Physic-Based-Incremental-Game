using UnityEngine;

public class CollectToolState : ToolState
{
    protected CollectController ToolController;
    protected ToolMachineManager ToolMachine;
    protected LayerMask GroundLayerMask;
    protected Wire Wire;
    public CollectToolState(StateMachine stateMachine,CollectController controller, UIIndicator indicator, GameObject tool, ToolControllers toolControlManager, ToolMachineManager toolMachine, Wire wire, LayerMask groundLayerMask) : base(stateMachine, indicator, tool, toolControlManager)
    {
        ToolController = controller;
        ToolMachine = toolMachine;
        GroundLayerMask = groundLayerMask;
        Wire = wire;
    }
}
