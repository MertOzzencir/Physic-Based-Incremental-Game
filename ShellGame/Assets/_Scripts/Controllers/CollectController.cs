using System.Collections;
using UnityEngine;

public class CollectController : Tools
{
    [SerializeField] private Wire collectorWire;

    public override void Awake()
    {
        base.Awake();
        ToolMachine.MachineTool = this;
        ToolController = GetComponent<ToolControllers>();
        HoverIndicatorController = GetComponent<HoverController>();
        //ToolGameObject.GetComponentInChildren<AnimationEventSender>().OnAnimationTrigger += BreakObject;
        StateMachine = new ToolStateMachine();
        CollectIdleState = new CollectIdleState(StateMachine, this, ToolController, ToolGameObject, Indicator, groundLayermask, breakableLayerMask, new Vector3(0f, 0.5f, 0f));
        StationState = new ToolStationState(StateMachine, this, ToolController, ToolGameObject, Indicator, CollectIdleState);
        StateMachine.Initilize(StationState);
        StartCoroutine(EnableWireDelayed());

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
        collectorWire.enabled = state;
    }

    IEnumerator EnableWireDelayed()
    {
        yield return new WaitForSeconds(.75f);
        EnableWire(false);
    }

}
