using UnityEngine;

public abstract class HammerState : ToolState
{
    protected HammerController ToolController;
    protected GameObject HammerTool;
    protected LayerMask GroundLayerMask;
    protected LayerMask BreakableLayerMask;
    protected HammerState(StateMachine stateMachine, UIIndicator indicator, HammerController controller, GameObject tool, ToolControllers toolManager, LayerMask groundLayerMask, LayerMask breakableLayerMask) : base(stateMachine, indicator, tool, toolManager)
    {
        ToolController = controller;
        GroundLayerMask = groundLayerMask;
        BreakableLayerMask = breakableLayerMask;
    }

}
