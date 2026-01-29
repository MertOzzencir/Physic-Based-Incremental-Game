using UnityEngine;

public class ToolStationState : ToolStates
{
    ToolStates toolIdleState;
    public ToolStationState(ToolStateMachine stateMachine, Tools toolController, ToolControllers toolPickController, GameObject tool, UIIndicator indicator,ToolStates idleState) : base(stateMachine, toolController, toolPickController, tool, indicator)
    {
        toolIdleState = idleState;
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
        if (ToolPickController.CurrentTool == ToolLogicController)
        {
            StateMachine.ChangeState(toolIdleState);
        }
    }


}
