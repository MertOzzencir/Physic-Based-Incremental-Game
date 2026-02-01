using Unity.VisualScripting;
using UnityEngine;

public class CollectIdleState : CollectToolState
{
    private Vector3 verticalOffSet;
    private Rigidbody rb;


    private Vector3 lastPoint;
    private bool hasLastPoint;
    private float yOffSet = 0;
    public CollectIdleState(StateMachine stateMachine, CollectController controller, UIIndicator indicator, GameObject tool, ToolControllers toolControlManager, ToolMachineManager toolMachine, Wire wire, LayerMask groundLayerMask, Vector3 verticalOffSet, Rigidbody rb) : base(stateMachine, controller, indicator, tool, toolControlManager, toolMachine, wire, groundLayerMask)
    {
        this.verticalOffSet = verticalOffSet;
        this.rb = rb;
    }

    public override void Enter()
    {
        base.Enter();
        ToolGameObject.transform.rotation = Quaternion.LookRotation(ToolMachine.transform.forward);
    }
    public override void Exit()
    {
        base.Exit();
    }

    float currentDistance;

    public override void Update()
    {
        base.Update();
        currentDistance = Vector3.Distance(ToolGameObject.transform.position, ToolMachine.transform.position);
        Wire.totalLength = currentDistance;
        Wire.UpdateLength();

        // if (currentDistance > 8f)
        // {
        //     ToolPickController.CurrentTool = null;
        //     ToolLogicController.DeEquippedLogic();
        //     return;
        // }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayerMask))
        {

            if (!hasLastPoint)
            {
                lastPoint = hit.point;
                hasLastPoint = true;

            }
            float delta = Vector3.Distance(hit.point, lastPoint);
            float speed = delta / Time.deltaTime;


            if (speed > 1.5f)
            {
                yOffSet += Time.deltaTime * 2.5f;
            }
            else
                yOffSet -= Time.deltaTime * 6f;

            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;

            yOffSet = Mathf.Clamp(yOffSet, 0, 0.75f);
            movePosition = hit.point + verticalOffSet + new Vector3(0, yOffSet, 0);
            lastPoint = hit.point;
        }
        if (RightClickState)
            StateMachine.ChangeState(ToolController.CollectPrepareState);
        if (ToolPickManager.CurrentTool != ToolController)
        {
            StateMachine.ChangeState(ToolController.CollectIdleState);
        }
    }
    Vector3 movePosition;


    public override void FixedUpdate()
    {
        base.FixedUpdate();
        movePosition = Vector3.Lerp(rb.position, movePosition, 5f * Time.fixedDeltaTime);
        rb.MovePosition(movePosition);
    }
}
