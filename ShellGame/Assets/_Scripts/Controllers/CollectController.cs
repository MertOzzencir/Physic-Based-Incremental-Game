using System.Collections;
using UnityEngine;

public class CollectController : Tools
{
    [SerializeField] private Wire wire;

    private Rigidbody rb;
    public CollectIdleState CollectIdleState { get; set; }
    public CollectPrepareState CollectPrepareState { get; set; }
    public CollectStationState CollectStationState { get; set; }
    public override void Awake()
    {
        base.Awake();
        rb = ToolGameObject.GetComponent<Rigidbody>();
        //ToolGameObject.GetComponentInChildren<AnimationEventSender>().OnAnimationTrigger += BreakObject;
        CollectStationState = new CollectStationState(StateMachine, this, IndicatorController, ToolGameObject, ToolController, ToolMachine, wire, GroundLayerMask);
        CollectIdleState = new CollectIdleState(StateMachine, this, IndicatorController, ToolGameObject, ToolController, ToolMachine, wire, GroundLayerMask, new Vector3(0f, 0.5f, 0f), rb);
        CollectPrepareState = new CollectPrepareState(StateMachine, this, IndicatorController, ToolGameObject, ToolController, ToolMachine, wire, GroundLayerMask);
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
