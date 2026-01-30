using Unity.VisualScripting;
using UnityEngine;

public class HammerPrepareState : ToolStates
{
    LayerMask groundLayerMask;
    LayerMask breakableLayerMask;
    float hammerSize;
    private float rotationCorrect;
    private float hammerRollRotation;
    private LocalChildBreakable currentBreakable;
    public HammerPrepareState(ToolStateMachine stateMachine, Tools toolController, ToolControllers toolPickController, GameObject tool, UIIndicator indicator, LayerMask groundLayerMask, LayerMask breakableLayerMask, float hammerSize, float hammerMaxRollRotation) : base(stateMachine, toolController, toolPickController, tool, indicator)
    {
        this.groundLayerMask = groundLayerMask;
        this.breakableLayerMask = breakableLayerMask;
        this.hammerSize = hammerSize;
        hammerRollRotation = hammerMaxRollRotation;
    }

    public override void Enter()
    {
        base.Enter();
        Indicator.SetIndicator(CursorIndicator.BreakMode);

        currentBreakable = null;

    }

    public override void Exit()
    {
        base.Exit();
        ToolLogicController.HoverIndicatorController.enabled = false;

    }
    public override void Update()
    {
        base.Update();
        if (RightClickState)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, breakableLayerMask, QueryTriggerInteraction.Collide))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
                {

                    if (hit.collider.TryGetComponent(out LocalChildBreakable breakObject))
                    {

                        if (Physics.Raycast(hit.point, Vector3.down,
                       out RaycastHit hitCorrect, hammerSize, groundLayerMask))
                        {
                            float distance = hit.point.y - hitCorrect.point.y;
                            rotationCorrect = (1 - distance) * hammerRollRotation;
                        }
                        Tool.transform.position = Vector3.Lerp(Tool.transform.position, hit.point + hit.normal.normalized * .25f, 9f * Time.deltaTime);
                        Quaternion lookRotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
                        Quaternion roll = Quaternion.Euler(0, 0, rotationCorrect);
                        Tool.transform.rotation = Quaternion.Lerp(Tool.transform.rotation, lookRotation * roll, 9 * Time.deltaTime);

                        if (breakObject.IsCollectable)
                            return;
                        if (LeftClickState)
                            StateMachine.ChangeState(ToolLogicController.HammerBreakState);
                        if (currentBreakable == breakObject)
                            return;
                        ToolLogicController.HoverIndicatorController.enabled = true;
                        if (hit.transform.gameObject != null)
                            ToolLogicController.HoverIndicatorController.HoverOnObject(hit.collider);
                        currentBreakable = breakObject;

                    }

                }
            }
            else
            {
                ToolLogicController.HoverIndicatorController.enabled = false;
                StateMachine.ChangeState(ToolLogicController.HammerIdleState);
            }

        }
        else
        {
            StateMachine.ChangeState(ToolLogicController.HammerIdleState);
        }

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
