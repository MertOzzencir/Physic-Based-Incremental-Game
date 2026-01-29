using System;
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
        StationState = new ToolStationState(StateMachine, this, ToolController, ToolGameObject, Indicator, HammerIdleState);
        HammerBreakState = new HammerBreakState(StateMachine, this, ToolController, ToolGameObject, Indicator, AnimatorController);
        StateMachine.Initilize(StationState);
    }
}

