using UnityEditor.EditorTools;
using UnityEngine;

public class HammerIdleState : HammerState

{
    private Vector3 hammerHeightOffSet;


    private Vector3 lastPoint;
    private Vector3 lastDirection;

    public HammerIdleState(StateMachine stateMachine, UIIndicator indicator, HammerController controller, GameObject tool, ToolControllers toolManager, LayerMask groundLayerMask, LayerMask breakableLayerMask, Vector3 verticalOffSet) : base(stateMachine, indicator, controller, tool, toolManager, groundLayerMask, breakableLayerMask)
    {
        hammerHeightOffSet = verticalOffSet;
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



    public override void Update()
    {
        base.Update();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayerMask))
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
            ToolGameObject.transform.position = Vector3.Lerp(ToolGameObject.transform.position, hit.point + hammerHeightOffSet + new Vector3(0, yOffSet, 0), 5f * Time.deltaTime);
            ToolGameObject.transform.rotation = Quaternion.Lerp(ToolGameObject.transform.rotation, lookRotation, 15f * Time.deltaTime);
            lastPoint = hit.point;
        }
        if (RightClickState)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, BreakableLayerMask /*| groundLayermask*/, QueryTriggerInteraction.Collide))
            {
                StateMachine.ChangeState(ToolController.HammerPrepareState);
            }
        }
        if (ToolPickManager.CurrentTool != ToolController)
        {
            StateMachine.ChangeState(ToolController.HammerStationState);
        }
    }

}

