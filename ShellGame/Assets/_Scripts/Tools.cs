using System;
using System.Collections;
using UnityEngine;

public abstract class Tools : MonoBehaviour
{
    public GameObject ToolGameObject;
    public LayerMask GroundLayerMask;
    public Vector3 ToolVerticalOffSet;

    public UIIndicator IndicatorController;


    public StateMachine StateMachine;


    public ToolMachineManager ToolMachine;
    public ToolControllers ToolController { get; set; }
    public HoverController HoverIndicatorController { get; set; }

    public Animator AnimatorController;




    //COLLECTSTATES
   


    public virtual void Awake()
    {
        StateMachine = new StateMachine();

        ToolMachine.MachineTool = this;
        AnimatorController = ToolGameObject.GetComponentInChildren<Animator>();
        ToolController = GetComponent<ToolControllers>();
        HoverIndicatorController = GetComponent<HoverController>();
        IndicatorController = FindAnyObjectByType<UIIndicator>();

    }
    public virtual void Update()
    {
        StateMachine.Update();
    }
    public virtual void FixedUpdate()
    {
        StateMachine.FixedUpdate();
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

    public virtual IEnumerator HandleToolAnimation(ToolControllers ToolPickController, Transform toolTransform, Vector3 placementPosition, Vector3 directionVector)
    {
        ToolPickController.enabled = false;
        Quaternion lookRotation = Quaternion.LookRotation(directionVector, Vector3.up);
        while (Vector3.Distance(toolTransform.position, placementPosition) > 0.1f)
        {
            toolTransform.position = Vector3.Lerp(toolTransform.position, placementPosition, 10 * Time.deltaTime);
            toolTransform.rotation = Quaternion.Lerp(toolTransform.rotation, lookRotation, 10 * Time.deltaTime);
            yield return null;
        }
        toolTransform.position = placementPosition;
        toolTransform.transform.rotation = lookRotation;
        ToolPickController.enabled = true;
    }







}
