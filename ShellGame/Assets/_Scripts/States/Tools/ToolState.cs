using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ToolState : StateBase
{
    protected UIIndicator Indicator;
    protected GameObject ToolGameObject;
    protected ToolControllers ToolPickManager;

    protected static bool RightClickState;
    protected static bool LeftClickState;

    public ToolState(StateMachine stateMachine, UIIndicator indicator, GameObject tool, ToolControllers toolControlManager)
        : base(stateMachine)
    {
        Indicator = indicator;
        ToolGameObject = tool;
        ToolPickManager = toolControlManager;
    }

    public override void Enter()
    {
        base.Enter();
        InputManager.OnRightClickAction += SetRightClick;
        InputManager.OnLeftClickAction += SetLeftClick;
    }

    public override void Exit()
    {
        base.Exit();
        InputManager.OnRightClickAction -= SetRightClick;
        InputManager.OnLeftClickAction -= SetLeftClick;
    }

    private void SetRightClick(bool buttonState)
    {
        RightClickState = buttonState;
    }

    private void SetLeftClick(bool buttonState)
    {
        LeftClickState = buttonState;
    }
}