using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocalChildBreakable : MonoBehaviour, IBreakable,ICollectable
{
    public event Action<LocalChildBreakable,Vector3> OnSendMessage;

    [SerializeField] private MeshCollider childCollider;
    public List<LocalChildBreakable> connectedToObject;
    public LocalChildBreakable rootObject;



    int childTimer;

    public bool IsCollectable { get ; set; }

    void Awake()
    {
        childTimer = connectedToObject.Count;
    }
    public void Break(Vector3 forceDirection)
    {
        if (!IsCollectable)
        {
            OnSendMessage?.Invoke(this,forceDirection);
            if(childCollider != null)
            {
                childCollider.enabled = false;
            }
            CollectState(true);
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
            CollectState(true);
            if (transform.GetComponent<Rigidbody>() == null)
                transform.AddComponent<Rigidbody>();
            if (rootObject != null)
            {
                rootObject.RecieveMessageFromChild(this);
            }
        }
    }
    public void CollectState(bool state)
    {
        IsCollectable = state;
    }

    public void Collect()
    {
        throw new NotImplementedException();
    }
}
