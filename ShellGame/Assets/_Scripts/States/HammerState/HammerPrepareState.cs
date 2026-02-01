using Unity.VisualScripting;
using UnityEngine;

public class HammerPrepareState : HammerState
{
    float hammerSize;
    private float rotationCorrect;
    private float hammerRollRotation;
    private LocalChildBreakable currentBreakable;

    public HammerPrepareState(StateMachine stateMachine, UIIndicator indicator, HammerController controller, GameObject tool, ToolControllers toolManager, LayerMask groundLayerMask, LayerMask breakableLayerMask, float hammerSize, float maxRotation) : base(stateMachine, indicator, controller, tool, toolManager, groundLayerMask, breakableLayerMask)
    {
        this.hammerSize = hammerSize;
        rotationCorrect = maxRotation;
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
        ToolController.HoverIndicatorController.enabled = false;

    }
    public override void Update()
    {
        base.Update();
        if (RightClickState)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, BreakableLayerMask, QueryTriggerInteraction.Collide))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
                {

                    if (hit.collider.TryGetComponent(out LocalChildBreakable breakObject))
                    {

                        if (Physics.Raycast(hit.point, Vector3.down,
                       out RaycastHit hitCorrect, hammerSize, GroundLayerMask))
                        {
                            float distance = hit.point.y - hitCorrect.point.y;
                            rotationCorrect = (1 - distance) * hammerRollRotation;
                        }
                        ToolGameObject.transform.position = Vector3.Lerp(ToolGameObject.transform.position, hit.point + hit.normal.normalized * .25f, 9f * Time.deltaTime);
                        Quaternion lookRotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
                        Quaternion roll = Quaternion.Euler(0, 0, rotationCorrect);
                        ToolGameObject.transform.rotation = Quaternion.Lerp(ToolGameObject.transform.rotation, lookRotation * roll, 9 * Time.deltaTime);

                        if (breakObject.IsCollectable)
                            return;
                        if (LeftClickState)
                            StateMachine.ChangeState(ToolController.HammerBreakState);
                        if (currentBreakable == breakObject)
                            return;
                        ToolController.HoverIndicatorController.enabled = true;
                        if (hit.transform.gameObject != null)
                            ToolController.HoverIndicatorController.HoverOnObject(hit.collider);
                        currentBreakable = breakObject;

                    }

                }
            }
            else
            {
                ToolController.HoverIndicatorController.enabled = false;
                StateMachine.ChangeState(ToolController.HammerIdleState);
            }

        }
        else
        {
            StateMachine.ChangeState(ToolController.HammerIdleState);
        }

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
