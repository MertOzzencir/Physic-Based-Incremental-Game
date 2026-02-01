using UnityEngine;

public class CollectStationState : CollectToolState
{

    public CollectStationState(StateMachine stateMachine, CollectController controller, UIIndicator indicator, GameObject tool, ToolControllers toolControlManager, ToolMachineManager toolMachine, Wire wire, LayerMask groundLayerMask) : base(stateMachine, controller, indicator, tool, toolControlManager, toolMachine, wire, groundLayerMask)
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
        float distance = Vector3.Distance(ToolGameObject.transform.position, ToolMachine.transform.position);
        Wire.totalLength = distance + 1;
        Wire.UpdateLength();
        if (ToolPickManager.CurrentTool == ToolController)
        {
            StateMachine.ChangeState(ToolController.CollectIdleState);
        }
    }
}
