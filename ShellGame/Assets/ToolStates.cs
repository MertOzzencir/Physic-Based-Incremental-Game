using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ToolStates
{
    public ToolStateMachine StateMachine;
    public Tools ToolLogicController;
    public ToolControllers ToolPickController;
    public GameObject Tool;
    public UIIndicator Indicator;
    public static bool RightClickState;
    public static bool LeftClickState;

    public ToolStates(ToolStateMachine stateMachine, Tools toolLogicController, ToolControllers toolPickController, GameObject tool, UIIndicator indicator)
    {
        StateMachine = stateMachine;
        ToolLogicController = toolLogicController;
        Tool = tool;
        Indicator = indicator;
        ToolPickController = toolPickController;
    }


    public virtual void Enter()
    {
        InputManager.OnRightClickAction += SetRightClick;
        InputManager.OnLeftClickAction +=SetLeftClick;
    }


    public virtual void Exit()
    {
        InputManager.OnRightClickAction -= SetRightClick;
        InputManager.OnLeftClickAction -=SetLeftClick;
    }

    public virtual void Update()
    {
    }
    public virtual void FixedUpdate()
    {

    }
    public void SetRightClick(bool buttonState)
    {
        RightClickState = buttonState;
    }
    public void SetLeftClick(bool buttonState)
    {
        LeftClickState = buttonState;
    }

}
