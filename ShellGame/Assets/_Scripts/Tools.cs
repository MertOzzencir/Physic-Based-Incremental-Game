using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class Tools : MonoBehaviour
{
    public GameObject ToolGameObject;
    public LayerMask groundLayermask;
    public LayerMask breakableLayerMask;
    public Vector3 ToolVerticalOffSet;
    public float ToolMaxRollRotation;

    public UIIndicator Indicator;


    private ToolStateMachine stateMachine;
    public HammerStationState StationState { get; set; }
    public HammerIdleState IdleState { get; set; }
    public HammerPrepareState PrepareState { get; set; }
    public HammerBreakState BreakState { get; set; }

    public ToolMachineManager ToolMachine;
    public ToolControllers ToolController { get; private set; }
    public HoverController HoverIndicatorController { get; private set; }

    private float hammerSize;
    private Animator anim;


    public virtual void Awake()
    {
        ToolMachine.MachineTool = this;
        hammerSize = ToolGameObject.GetComponent<BoxCollider>().size.y;
        anim = ToolGameObject.GetComponentInChildren<Animator>();

        ToolController = GetComponent<ToolControllers>();
        HoverIndicatorController = GetComponent<HoverController>();
        //ToolGameObject.GetComponentInChildren<AnimationEventSender>().OnAnimationTrigger += BreakObject;
        stateMachine = new ToolStateMachine();
        IdleState = new HammerIdleState(stateMachine, this, ToolController, ToolGameObject, Indicator, groundLayermask, breakableLayerMask, ToolVerticalOffSet);
        PrepareState = new HammerPrepareState(stateMachine, this, ToolController, ToolGameObject, Indicator, groundLayermask, breakableLayerMask, hammerSize, ToolMaxRollRotation);
        StationState = new HammerStationState(stateMachine, this, ToolController, ToolGameObject, Indicator);
        BreakState = new HammerBreakState(stateMachine,this,ToolController,ToolGameObject,Indicator,anim);
        stateMachine.Initilize(StationState);
    }
    void Update()
    {
        stateMachine.Update();
    }

    public virtual void EquippedLogic(bool stationState)
    {
        this.enabled = stationState;
        HoverIndicatorController.enabled = stationState;
        stateMachine.ChangeState(StationState);
    }
    public void ToolBackToStationAnimation(ToolControllers ToolPickController, Transform toolTransform, Vector3 placementPosition, Vector3 directionVector)
    {
        StartCoroutine(HandleToolAnimation(ToolPickController, toolTransform, placementPosition, directionVector));
    }
    public virtual IEnumerator HandleToolAnimation(ToolControllers ToolPickController, Transform toolTransform, Vector3 placementPosition, Vector3 directionVector)
    {
        ToolPickController.enabled = false;
        Quaternion lookRotation = Quaternion.LookRotation(directionVector, Vector3.up);
        while (Vector3.Distance(toolTransform.position, placementPosition) > 0.1f)
        {
            toolTransform.position = Vector3.Lerp(toolTransform.position, placementPosition, 15 * Time.deltaTime);
            toolTransform.rotation = Quaternion.Lerp(toolTransform.rotation, lookRotation, 15 * Time.deltaTime);
            yield return null;
        }
        toolTransform.position = placementPosition;
        toolTransform.transform.rotation = lookRotation;
        ToolPickController.enabled = true;
    }




}
