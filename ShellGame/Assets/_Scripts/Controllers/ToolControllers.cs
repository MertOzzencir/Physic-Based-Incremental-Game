using System;
using UnityEditor;
using UnityEngine;

public class ToolControllers : MonoBehaviour
{
    [SerializeField] private Tools[] tools;

    public Tools CurrentTool;
    void OnEnable()
    {
        InputManager.OnToolPick += PickTool;
    }

    void OnDisable()
    {
        InputManager.OnToolPick -= PickTool;
    }
    private void PickTool(int obj)
    {
        SetTool(tools[obj - 1]);
    }
    public void SetTool(Tools nextTool)
    {

        if (CurrentTool == nextTool)
        {
            CurrentTool.DeEquippedLogic();
            CurrentTool = null;
        }
        else if (CurrentTool == null)
        {
            CurrentTool = nextTool;
            CurrentTool.EquippedLogic();
        }
        else
        {
            CurrentTool.DeEquippedLogic();
            CurrentTool = nextTool;
            CurrentTool.EquippedLogic();
        }
    }
}
