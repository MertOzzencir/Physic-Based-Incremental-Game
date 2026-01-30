using Unity.VisualScripting;
using UnityEngine;

public class CollectIdleState : ToolStates
{
    private LayerMask groundLayerMask;
    private Vector3 hammerHeightOffSet;
    private float yOffSet = 0;
    private bool hasLastPoint;
    private Vector3 lastPoint;
    private Vector3 lastDirection;
    private Wire wire;
    public CollectIdleState(ToolStateMachine stateMachine, Tools toolLogicController, ToolControllers toolPickController, GameObject tool, UIIndicator indicator, LayerMask groundLayer, Vector3 verticalOffSet, Wire wire) : base(stateMachine, toolLogicController, toolPickController, tool, indicator)
    {
        hammerHeightOffSet = verticalOffSet;
        groundLayerMask = groundLayer;
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

    float currentDistance;

    public override void Update()
    {
        base.Update();
        currentDistance = Vector3.Distance(Tool.transform.position, ToolLogicController.ToolMachine.transform.position);
        wire.totalLength = currentDistance + 2f;
        wire.UpdateLength();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {

            if (!hasLastPoint)
            {
                lastPoint = hit.point;
                lastDirection = Vector3.forward.normalized;
                hasLastPoint = true;

            }
            float delta = Vector3.Distance(hit.point, lastPoint);
            float speed = delta / Time.deltaTime;

            Vector3 currentDirection = hit.point - lastPoint;
            currentDirection.y = 0;
            if (currentDirection != Vector3.zero)
            {
                lastDirection = currentDirection;
            }

            if (speed > 1.5f)
            {
                yOffSet += Time.deltaTime * 2.5f;
            }
            else
                yOffSet -= Time.deltaTime * 6f;

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;

            yOffSet = Mathf.Clamp(yOffSet, 0, 0.75f);
            Quaternion lookRotation = Quaternion.LookRotation(lastDirection);
            Tool.transform.position = Vector3.Lerp(Tool.transform.position, hit.point + hammerHeightOffSet + new Vector3(0, yOffSet, 0), 5f * Time.deltaTime);
            Tool.transform.rotation = Quaternion.Lerp(Tool.transform.rotation, lookRotation, 15f * Time.deltaTime);
            lastPoint = hit.point;
        }
        if (RightClickState)
            StateMachine.ChangeState(ToolLogicController.CollectPrepareState);
        if (ToolPickController.CurrentTool != ToolLogicController)
        {
            StateMachine.ChangeState(ToolLogicController.HammerStationState);
        }
    }
}
