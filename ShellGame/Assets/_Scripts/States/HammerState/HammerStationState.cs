using UnityEditor.EditorTools;
using UnityEngine;

public class HammerStationState : HammerState
{
    public HammerStationState(StateMachine stateMachine, UIIndicator indicator, HammerController controller, GameObject tool, ToolControllers toolManager, LayerMask groundLayerMask, LayerMask breakableLayerMask) : base(stateMachine, indicator, controller, tool, toolManager, groundLayerMask, breakableLayerMask)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (ToolPickManager.CurrentTool == ToolController)
        {
            StateMachine.ChangeState(ToolController.HammerIdleState);
        }
    }


}
