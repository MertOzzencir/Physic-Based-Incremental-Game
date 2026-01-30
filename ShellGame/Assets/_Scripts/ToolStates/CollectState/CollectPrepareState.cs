using System;
using UnityEngine;

public class CollectPrepareState : ToolStates
{
    private ICollectable currentCollectable;
    private event Action OnComplete;
    private Wire wire;
    public CollectPrepareState(ToolStateMachine stateMachine, Tools toolLogicController, ToolControllers toolPickController, GameObject tool, UIIndicator indicator, Wire wire) : base(stateMachine, toolLogicController, toolPickController, tool, indicator)
    {
        this.wire = wire;
    }

    public override void Enter()
    {
        base.Enter();
        OnComplete += CallBackFromTool;
    }



    public override void Exit()
    {
        base.Exit();
        OnComplete -= CallBackFromTool;
    }
    public override void Update()
    {
        base.Update();
        if (RightClickState && currentCollectable == null)
        {
            Debug.Log("saa");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ToolLogicController.groundLayermask))
            {
                Collider[] hits = Physics.OverlapBox(hit.point, Vector3.one * 2, Quaternion.identity);

                int i = 0;
                foreach (var a in hits)
                {
                    if (a.gameObject.TryGetComponent(out ICollectable collectableObject))
                    {
                        i++;
                        currentCollectable = collectableObject;
                        float distance = Vector3.Distance(Tool.transform.position, a.transform.position);
                        float wireDistance = Vector3.Distance(a.transform.position, ToolLogicController.ToolMachine.transform.position) + 2;
                        wire.totalLength = wireDistance;
                        wire.UpdateLength();
                        ToolLogicController.CollectAnimationCaller(Tool.transform, a.transform, distance, OnComplete);
                        //collectableObject.Collect(Tool.transform);
                        //Debug.Log($"{i}. Object: " + a.name);
                        return;
                    }
                }
                Tool.transform.position = Vector3.Lerp(Tool.transform.position, hit.point, 5f * Time.deltaTime);
                float wireOutDistance = Vector3.Distance(Tool.transform.position, ToolLogicController.ToolMachine.transform.position) + 2;
                wire.totalLength = wireOutDistance;
                wire.UpdateLength();

            }
        }
        else if (currentCollectable == null)
            StateMachine.ChangeState(ToolLogicController.CollectIdleState);
    }


    private void CallBackFromTool()
    {
        Debug.Log("sa?");
        currentCollectable.Collect();
        currentCollectable = null;
    }
}
