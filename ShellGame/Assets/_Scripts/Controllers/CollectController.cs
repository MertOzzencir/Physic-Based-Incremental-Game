using System.Collections;
using UnityEngine;

public class CollectController : Tools
{
    [SerializeField] private Wire wire;

    public override void Awake()
    {
        base.Awake();
        ToolMachine.MachineTool = this;
        ToolController = GetComponent<ToolControllers>();
        HoverIndicatorController = GetComponent<HoverController>();
        //ToolGameObject.GetComponentInChildren<AnimationEventSender>().OnAnimationTrigger += BreakObject;
        StateMachine = new ToolStateMachine();
        CollectIdleState = new CollectIdleState(StateMachine, this, ToolController, ToolGameObject, Indicator, groundLayermask, new Vector3(0f, 0.5f, 0f), wire);
        CollectStationState = new CollectStationState(StateMachine, this, ToolController, ToolGameObject, Indicator, wire);
        CollectPrepareState = new CollectPrepareState(StateMachine, this, ToolController, ToolGameObject, Indicator, wire);
        StateMachine.Initilize(CollectStationState);
        //StartCoroutine(EnableWireDelayed());

    }


    public override void EquippedLogic()
    {
        base.EquippedLogic();
        StateMachine.ChangeState(CollectStationState);

    }
    public override void DeEquippedLogic()
    {
        base.DeEquippedLogic();
        StateMachine.ChangeState(CollectStationState);
    }

    public override IEnumerator HandleToolAnimation(ToolControllers ToolPickController, Transform toolTransform, Vector3 placementPosition, Vector3 directionVector)
    {
        yield return base.HandleToolAnimation(ToolPickController, toolTransform, placementPosition, directionVector);
        this.enabled = false;
    }

    void OnDisable()
    {
        EnableWire(false);
    }
    void OnEnable()
    {
        EnableWire(true);
    }

    void EnableWire(bool state)
    {
        if (wire != null)
        {
            wire.UpdateMesh();
            wire.enabled = state;
        }
    }

    IEnumerator EnableWireDelayed()
    {
        yield return new WaitForSeconds(.75f);
        EnableWire(false);
    }

}
