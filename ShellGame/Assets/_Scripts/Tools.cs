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


    public ToolStateMachine StateMachine;
    public ToolStationState StationState { get; set; }


    public ToolMachineManager ToolMachine;
    public ToolControllers ToolController { get; set; }
    public HoverController HoverIndicatorController { get; set; }

    public Animator AnimatorController;

    //HAMMER STATES
    public HammerIdleState HammerIdleState { get; set; }
    public HammerPrepareState HammerPrepareState { get; set; }
    public HammerBreakState HammerBreakState { get; set; }

    //COLLECTSTATES
    public CollectIdleState CollectIdleState { get; set; }


    public virtual void Awake()
    {
        ToolMachine.MachineTool = this;
        AnimatorController = ToolGameObject.GetComponentInChildren<Animator>();

        ToolController = GetComponent<ToolControllers>();
        HoverIndicatorController = GetComponent<HoverController>();

    }
    public virtual void Update()
    {
        StateMachine.Update();
    }

    public virtual void EquippedLogic()
    {
        this.enabled = true;
        StateMachine.ChangeState(StationState);
    }
    public virtual void DeEquippedLogic()
    {
        ToolBackToStationAnimation(ToolController, ToolGameObject.transform, ToolMachine.ToolPlacement.position, ToolMachine.ToolPlacement.forward);
        StateMachine.ChangeState(StationState);
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
            toolTransform.position = Vector3.Lerp(toolTransform.position, placementPosition, 5 * Time.deltaTime);
            toolTransform.rotation = Quaternion.Lerp(toolTransform.rotation, lookRotation, 5 * Time.deltaTime);
            yield return null;
        }
        toolTransform.position = placementPosition;
        toolTransform.transform.rotation = lookRotation;
        ToolPickController.enabled = true;
        this.enabled = false;

    }




}
