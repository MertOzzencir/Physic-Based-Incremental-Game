using UnityEngine;

public class HammerStationState : ToolStates
{

    public HammerStationState(ToolStateMachine stateMachine, Tools toolController, ToolControllers toolPickController, GameObject tool, UIIndicator indicator) : base(stateMachine, toolController, toolPickController, tool, indicator)
    {
    }
    public override void Enter()
    {
        base.Enter();
        ToolLogicController.ToolBackToStationAnimation(ToolPickController, Tool.transform, ToolLogicController.ToolMachine.ToolPlacement.position, ToolLogicController.ToolMachine.ToolPlacement.forward);
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
            StateMachine.ChangeState(ToolLogicController.IdleState);
        }
    }


}
