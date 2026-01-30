using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocalChildBreakable : MonoBehaviour, IBreakable
{

    public List<LocalChildBreakable> connectedToObject;
    public LocalChildBreakable rootObject;



    private LocalBreakableManager manager;
    private LocalDropManager dropManager;
    int childTimer;

    public bool IsCollectable { get; set; }

    void Awake()
    {
        childTimer = connectedToObject.Count;
        manager = GetComponentInParent<LocalBreakableManager>();
        dropManager = GetComponent<LocalDropManager>();
    }
    public void Break(Vector3 forceDirection)
    {
        if (!IsCollectable)
        {
            manager.HandleLogicOfChild(this, forceDirection);
            BreakState();
        }
    }
    public void RecieveMessageFromChild(LocalChildBreakable objectToRemove)
    {
        if (connectedToObject != null)
        {
            connectedToObject.Remove(objectToRemove);
            HoverCheck();
        }
    }
    private void HoverCheck()
    {
        if (connectedToObject.Count == 0)
        {
            connectedToObject.Clear();
            transform.parent = null;
            BreakState();
            if (transform.GetComponent<Rigidbody>() == null)
                transform.AddComponent<Rigidbody>();
            if (rootObject != null)
            {
                rootObject.RecieveMessageFromChild(this);
            }
        }
    }

    public void BreakState()
    {
        if (IsCollectable)
            return;
        IsCollectable = true;
        manager.OnChildDeath(this);
        dropManager.SpawnDrop(transform.position);
    }

}
