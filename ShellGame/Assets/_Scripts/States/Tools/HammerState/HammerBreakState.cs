using System;
using Unity.VisualScripting;
using UnityEngine;

public class HammerBreakState : HammerState
{
    private Animator toolAnimator;

    public HammerBreakState(StateMachine stateMachine, UIIndicator indicator, HammerController controller, GameObject tool, ToolControllers toolManager, LayerMask groundLayerMask, LayerMask breakableLayerMask, Animator anim) : base(stateMachine, indicator, controller, tool, toolManager, groundLayerMask, breakableLayerMask)
    {
        toolAnimator = anim;
    }

    public override void Enter()
    {
        base.Enter();
        toolAnimator.SetTrigger("canBreak");
        toolAnimator.transform.GetComponent<AnimationEventSender>().OnAnimationTrigger += BreakLogic;
        Indicator.BreakCursorInitiaze();
    }


    public override void Exit()
    {
        base.Exit();
        toolAnimator.transform.GetComponent<AnimationEventSender>().OnAnimationTrigger -= BreakLogic;

    }
    public override void Update()
    {
        base.Update();

    }
    private void BreakLogic()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent(out LocalChildBreakable breakablePiece))
            {
                if (breakablePiece != null)
                {
                    breakablePiece.Break(ToolGameObject.transform.forward);
                    Indicator.BreakCursorFinish();
                }
            }
        }
        StateMachine.ChangeState(ToolController.HammerPrepareState);
    }

}
