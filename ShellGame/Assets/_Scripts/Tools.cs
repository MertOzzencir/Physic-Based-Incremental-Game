using System;
using System.Collections;
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


    public ToolMachineManager ToolMachine;
    public ToolControllers ToolController { get; set; }
    public HoverController HoverIndicatorController { get; set; }

    public Animator AnimatorController;

    //HAMMER STATES
    public HammerIdleState HammerIdleState { get; set; }
    public HammerPrepareState HammerPrepareState { get; set; }
    public HammerBreakState HammerBreakState { get; set; }
    public HammerStationState HammerStationState { get; set; }


    //COLLECTSTATES
    public CollectIdleState CollectIdleState { get; set; }
    public CollectPrepareState CollectPrepareState { get; set; }
    public CollectStationState CollectStationState { get; set; }


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
    }
    public virtual void DeEquippedLogic()
    {
        ToolBackToStationAnimation(ToolController, ToolGameObject.transform, ToolMachine.ToolPlacement.position, ToolMachine.ToolPlacement.forward);
    }
    public float CalculateDistanceToLength(Transform dropTransform)
    {
        return Vector3.Distance(ToolGameObject.transform.position, dropTransform.position);
    }
    public void ToolBackToStationAnimation(ToolControllers ToolPickController, Transform toolTransform, Vector3 placementPosition, Vector3 directionVector)
    {
        StartCoroutine(HandleToolAnimation(ToolPickController, toolTransform, placementPosition, directionVector));
    }
    public void CollectAnimationCaller(Transform toolTransform, Transform dropTransform, float totalDistance, Action OnComplete)
    {
        StartCoroutine(CollectAnimation(toolTransform, dropTransform, totalDistance, OnComplete));
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
    }

    IEnumerator CollectAnimation(Transform toolTransform, Transform dropTransform, float distance, Action OnComplete)
    {
        float yOffSet = 0;
        float currentProgressToEnd = 0;
        float progress = 0;
        Vector3 toolYNormalizedPosition;
        Vector3 dropYNormalizedPosition;
        while (Vector3.Distance(toolTransform.position, dropTransform.position) > 0.1f)
        {

            toolYNormalizedPosition = new Vector3(toolTransform.position.x, 0, toolTransform.position.z);
            dropYNormalizedPosition = new Vector3(dropTransform.position.x, 0, dropTransform.position.z);

            currentProgressToEnd = Vector3.Distance(toolYNormalizedPosition, dropYNormalizedPosition);
            progress = 1 - (currentProgressToEnd / distance);
            progress = Mathf.Clamp01(progress);
            yOffSet = 4f * progress * (1 - progress) * 2;
            toolTransform.position = Vector3.Lerp(toolTransform.position, dropTransform.position + new Vector3(0, yOffSet, 0), 15f * Time.deltaTime);
            yield return null;
        }
        OnComplete?.Invoke();

    }





}
