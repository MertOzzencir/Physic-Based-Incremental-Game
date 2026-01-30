using UnityEngine;

public class CollectStationState : ToolStates
{
    Wire wire;
    public CollectStationState(ToolStateMachine stateMachine, Tools toolLogicController, ToolControllers toolPickController, GameObject tool, UIIndicator indicator, Wire wire) : base(stateMachine, toolLogicController, toolPickController, tool, indicator)
    {
        this.wire = wire;
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
        float distance = Vector3.Distance(Tool.transform.position, ToolLogicController.ToolMachine.transform.position);
        wire.totalLength = distance + 1;
        wire.UpdateLength();
        if (ToolPickController.CurrentTool == ToolLogicController)
        {
            StateMachine.ChangeState(ToolLogicController.CollectIdleState);
        }
    }
}
