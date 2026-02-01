using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HammerController : Tools
{
    [SerializeField] private LayerMask breakableLayerMask;
    [SerializeField] private float maxRollRotation;


    public HammerIdleState HammerIdleState { get; set; }
    public HammerPrepareState HammerPrepareState { get; set; }
    public HammerBreakState HammerBreakState { get; set; }
    public HammerStationState HammerStationState { get; set; }
    private float hammerSize;

    public override void Awake()
    {
        base.Awake();
        hammerSize = ToolGameObject.GetComponent<BoxCollider>().size.y;

        HammerStationState = new HammerStationState(StateMachine, IndicatorController, this, ToolGameObject, ToolController, GroundLayerMask, breakableLayerMask);
        HammerIdleState = new HammerIdleState(StateMachine, IndicatorController, this, ToolGameObject, ToolController, GroundLayerMask, breakableLayerMask, ToolVerticalOffSet);
        HammerPrepareState = new HammerPrepareState(StateMachine, IndicatorController, this, ToolGameObject, ToolController, GroundLayerMask, breakableLayerMask, hammerSize, maxRollRotation);
        HammerBreakState = new HammerBreakState(StateMachine, IndicatorController, this, ToolGameObject, ToolController, GroundLayerMask, breakableLayerMask, AnimatorController);
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

