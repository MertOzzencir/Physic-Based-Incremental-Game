using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HammerController : Tools
{
    [SerializeField] private float hammerRollRotation;
    [SerializeField] private float cancelLimit;
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject hammerPlacementPoint;
    [SerializeField] private LayerMask groundLayermask;
    [SerializeField] private LayerMask breakableLayerMask;
    [SerializeField] private Vector3 hammerHeightOffSet;
    [SerializeField] private Animator visualAnimator;
    [SerializeField] private UIndicator indicator;

    public LocalChildBreakable CurrentBreakable;

    private HoverController hoverIndicatorController;
    private float rotationCorrect;
    private float hammerSize;
    private bool canCollide;
    private bool onAction;
    private bool hoverMouseButton;
    private float cancelTimer;
    public override void Awake()
    {
        base.Awake();
        hammer.GetComponentInChildren<AnimationEventSender>().OnAnimationTrigger += BreakObject;
        hoverIndicatorController = GetComponent<HoverController>();
    }
    public void Start()
    {
        hammerSize = hammer.GetComponent<BoxCollider>().size.y;
        Cursor.visible = false;

    }
    void Update()
    {
        if (state != ToolState.Equiped)
            return;

        HammerLogic();
    }


    private void HammerLogic()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!canCollide)
        {
            if (hoverMouseButton)
            {
                Debug.Log("hoveredButton");
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, breakableLayerMask /*| groundLayermask*/, QueryTriggerInteraction.Collide))
                {
                    InitializeBreak();
                }
            }
            HammerFreeMove(ray);
        }
        else
        {
            BreakableShellCheck(ray);
        }
    }
    private void HammerFreeMove(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayermask))
        {
            hammer.transform.position = Vector3.Lerp(hammer.transform.position, hit.point + hammerHeightOffSet, 15f * Time.deltaTime);
            hammer.transform.rotation = Quaternion.Lerp(hammer.transform.rotation, Quaternion.Euler(0, 0, 0), 15f * Time.deltaTime);

        }
        indicator.SetIndicator(1);
    }
    private void BreakableShellCheck(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, breakableLayerMask /*| groundLayermask*/, QueryTriggerInteraction.Collide))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
            {

                if (hit.collider.TryGetComponent(out LocalChildBreakable breakObject) && canCollide)
                {
                  
                    cancelTimer = 0;
                    if (Physics.Raycast(hit.point, Vector3.down,
                   out RaycastHit hitCorrect, hammerSize, groundLayermask))
                    {
                        float distance = hit.point.y - hitCorrect.point.y;
                        rotationCorrect = (1 - distance) * hammerRollRotation;
                    }
                    hammer.transform.position = Vector3.Lerp(hammer.transform.position, hit.point + hit.normal.normalized * .25f, 15f * Time.deltaTime);
                    Quaternion lookRotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
                    Quaternion roll = Quaternion.Euler(0, 0, rotationCorrect);
                    hammer.transform.rotation = Quaternion.Lerp(hammer.transform.rotation, lookRotation * roll, 15 * Time.deltaTime);
                    Debug.Log("Found the object");
                    hoverIndicatorController.HoverOnObject(hit.collider);
                    hoverIndicatorController.enabled = true;
                    indicator.SetIndicator(2);

                }
                else
                    CurrentBreakable = null;
            }
            else
                indicator.SetIndicator(1);


        }
        else
        {
            cancelTimer += Time.deltaTime;
            if (cancelTimer > cancelLimit)
            {
                canCollide = false;
                hoverIndicatorController.enabled = false;
                cancelTimer = 0;
                indicator.SetIndicator(1);
            }
        }
    }

    private void InitializeBreak()
    {
        hoverMouseButton = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, breakableLayerMask, QueryTriggerInteraction.Collide))
            {
                canCollide = true;
            }
        }
        else
            canCollide = false;
    }
    private void CancelHammer()
    {
        hoverMouseButton = false;
        canCollide = false;
        hoverIndicatorController.enabled = false;

    }

    private void TryToBreakObject()
    {
        if (onAction)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
        {

            if (hit.collider.TryGetComponent(out LocalChildBreakable breakObject) && canCollide)
            {
                onAction = true;
                visualAnimator.SetTrigger("canBreak");
                indicator.BreakCursorInitiaze();
                CurrentBreakable = breakObject;

            }
        }
    }
    private void BreakObject()
    {
        if (CurrentBreakable != null)
        {
            indicator.BreakCursorFinish();
            CurrentBreakable.Break(hammer.transform.forward);
            onAction = false;
        }
    }


    public override void OnEquippedLogic()
    {
        state = ToolState.Equiped;
        this.enabled = true;
    }
    public override void OnDeEquippedLogic()
    {
        hoverIndicatorController.enabled = false;
        state = ToolState.DeEquiped;
        StartCoroutine(HandleToolAnimation(hammer.transform, hammerPlacementPoint.transform.position, hammerPlacementPoint.transform.forward, false));
    }


    private void OnEnable()
    {

        InputManager.OnBreakBeginning += InitializeBreak;
        InputManager.OnBreakCanceled += CancelHammer;
        InputManager.OnInteract += TryToBreakObject;
    }


    private void OnDisable()
    {
        InputManager.OnBreakBeginning -= InitializeBreak;
        InputManager.OnBreakCanceled -= CancelHammer;
        InputManager.OnInteract -= TryToBreakObject;
    }

}

