using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocalChildBreakable : MonoBehaviour, IBreakable, ICollectable
{

    public List<LocalChildBreakable> connectedToObject;
    public LocalChildBreakable rootObject;


    private LocalBreakableManager manager;
    int childTimer;

    public bool IsCollectable { get; set; }

    void Awake()
    {
        childTimer = connectedToObject.Count;
        manager = GetComponentInParent<LocalBreakableManager>();
    }
    public void Break(Vector3 forceDirection)
    {
        if (!IsCollectable)
        {
            manager.HandleLogicOfChild(this, forceDirection);
            CollectState();
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
            CollectState();
            if (transform.GetComponent<Rigidbody>() == null)
                transform.AddComponent<Rigidbody>();
            if (rootObject != null)
            {
                rootObject.RecieveMessageFromChild(this);
            }
        }
    }
    public void CollectState()
    {
        IsCollectable = true;
        manager.OnChildDeath(this);
    }

    public void Collect()
    {
        throw new NotImplementedException();
    }
}
