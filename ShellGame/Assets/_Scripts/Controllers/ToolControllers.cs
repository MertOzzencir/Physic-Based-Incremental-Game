using System;
using UnityEditor;
using UnityEngine;

public class ToolControllers : MonoBehaviour
{
    [SerializeField] private Tools[] tools;

    private Tools currentTool;
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
        int index = 1;
        foreach (var a in tools)
        {
            if (index == obj)
            {
                if (a.enabled == true)
                {
                    a.OnDeEquippedLogic();
                    currentTool = null;
                }
                else
                {
                    if (currentTool != null)
                        currentTool.OnDeEquippedLogic();
                    a.OnEquippedLogic();
                    currentTool = a;
                }
                break;
            }
            index++;
        }
    }
}
