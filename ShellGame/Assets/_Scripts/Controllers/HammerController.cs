using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HammerController : Tools
{

    private float hammerSize;

    public override void Awake()
    {
        base.Awake();
        hammerSize = ToolGameObject.GetComponent<BoxCollider>().size.y;

        StateMachine = new ToolStateMachine();
        HammerPrepareState = new HammerPrepareState(StateMachine, this, ToolController, ToolGameObject, Indicator, groundLayermask, breakableLayerMask, hammerSize, ToolMaxRollRotation);
        HammerIdleState = new HammerIdleState(StateMachine, this, ToolController, ToolGameObject, Indicator, groundLayermask, breakableLayerMask, ToolVerticalOffSet);
        HammerStationState = new HammerStationState(StateMachine, this, ToolController, ToolGameObject, Indicator);
        HammerBreakState = new HammerBreakState(StateMachine, this, ToolController, ToolGameObject, Indicator, AnimatorController);
        StateMachine.Initilize(HammerStationState);
    }
    public override void EquippedLogic()
    {
        base.EquippedLogic();
        StateMachine.ChangeState(HammerStationState);

    }
    public override void DeEquippedLogic()
    {
        base.DeEquippedLogic();
        StateMachine.ChangeState(HammerStationState);
    }
    public override IEnumerator HandleToolAnimation(ToolControllers ToolPickController, Transform toolTransform, Vector3 placementPosition, Vector3 directionVector)
    {
        yield return base.HandleToolAnimation(ToolPickController, toolTransform, placementPosition, directionVector);
        this.enabled = false;
    }
}

