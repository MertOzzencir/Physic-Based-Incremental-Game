using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectPrepareState : CollectToolState
{

    public CollectPrepareState(StateMachine stateMachine, CollectController controller, UIIndicator indicator, GameObject tool, ToolControllers toolControlManager, ToolMachineManager toolMachine, Wire wire, LayerMask groundLayerMask) : base(stateMachine, controller, indicator, tool, toolControlManager, toolMachine, wire, groundLayerMask)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }



    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (RightClickState)
        {
            Debug.Log("saa");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayerMask))
            {
                Collider[] hits = Physics.OverlapBox(hit.point, Vector3.one * 2, Quaternion.identity);

                int i = 0;
                foreach (var a in hits)
                {
                    if (a.gameObject.TryGetComponent(out ICollectable collectableObject))
                    {
                        i++;
                        collectableObject.Collect(ToolGameObject.transform);
                        //collectableObject.Collect(Tool.transform);
                        //Debug.Log($"{i}. Object: " + a.name);
                    }
                }
                ToolGameObject.transform.position = Vector3.Lerp(ToolGameObject.transform.position, hit.point + new Vector3(0, 0.5f, 0), 6f * Time.deltaTime);
                float wireOutDistance = Vector3.Distance(ToolGameObject.transform.position, ToolMachine.transform.position);
                Wire.totalLength = wireOutDistance;
                Wire.UpdateLength();

            }
        }
        else
            StateMachine.ChangeState(ToolController.CollectIdleState);
    }



}
