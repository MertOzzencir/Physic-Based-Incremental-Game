using UnityEngine;

public class HammerIdleState : ToolStates

{
    private LayerMask groundLayerMask;
    private LayerMask breakableObjectLayer;
    private Vector3 hammerHeightOffSet;


    private Vector3 lastPoint;
    private Vector3 lastDirection;
    public HammerIdleState(ToolStateMachine stateMachine, Tools toolController, ToolControllers toolPickController, GameObject tool, UIIndicator indiactor, LayerMask groundLayer, LayerMask breakableLayer, Vector3 verticalOffSet) : base(stateMachine, toolController, toolPickController, tool, indiactor)
    {
        groundLayerMask = groundLayer;
        hammerHeightOffSet = verticalOffSet;
        breakableObjectLayer = breakableLayer;
    }

    public override void Enter()
    {
        base.Enter();
        Indicator.SetIndicator(CursorIndicator.IdleMode);
    }
    public override void Exit()
    {
        base.Exit();
    }
    private float yOffSet = 0;
    private bool hasLastPoint;
    private Vector3 lookrotation;

    public override void Update()
    {
        base.Update();
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
            Tool.transform.position = Vector3.Lerp(Tool.transform.position, hit.point + hammerHeightOffSet + new Vector3(0, yOffSet, 0), 15f * Time.deltaTime);
            Tool.transform.rotation = Quaternion.Lerp(Tool.transform.rotation, lookRotation, 15f * Time.deltaTime);
            lastPoint = hit.point;
        }
        if (RightClickState)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, breakableObjectLayer /*| groundLayermask*/, QueryTriggerInteraction.Collide))
            {
                StateMachine.ChangeState(ToolLogicController.PrepareState);
            }
        }
        if (ToolPickController.CurrentTool != ToolLogicController)
        {
            StateMachine.ChangeState(ToolLogicController.StationState);
        }
    }

}

