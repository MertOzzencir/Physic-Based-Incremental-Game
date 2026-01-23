using Unity.VisualScripting;
using UnityEngine;

public class LocalBreakableManager : MonoBehaviour
{
    [SerializeField] private LocalChildBreakable[] childs;


    private void HandleLogicOfChild(LocalChildBreakable child, Vector3 forceDirection)
    {

        //check the objects that connected to child and relaease them
        var snapshot = child.connectedToObject.ToArray();
        foreach (var a in snapshot)
        {
            if (a.rootObject != null)
            {
                if (a.connectedToObject.Count == 0)
                {
                    a.CollectState(true);
                }

                a.rootObject.RecieveMessageFromChild(a);
            }
            Rigidbody _childRb;
            if (a.GetComponent<Rigidbody>() == null)
            {
                _childRb = a.transform.AddComponent<Rigidbody>();
            }
            else
                continue;
                
            if (_childRb != null)
            {
                _childRb.AddForce((forceDirection + Vector3.up) * 3.5f, ForceMode.Impulse);
            }
            a.transform.parent = null;
        }

        //check child's root object, if it's exist. Send message.

        Rigidbody mainChild;
        if (child.transform.GetComponent<Rigidbody>() == null)
        {
            mainChild = child.transform.AddComponent<Rigidbody>();

            mainChild.AddForce((forceDirection + Vector3.up) * 3.5f, ForceMode.Impulse);
        }
        child.transform.parent = null;
        if (child.rootObject != null)
        {
            child.rootObject.RecieveMessageFromChild(child);
        }
    }


    private void OnEnable()
    {
        foreach (var a in childs)
        {
            a.OnSendMessage += HandleLogicOfChild;
        }
    }
    private void OnDisable()
    {
        foreach (var a in childs)
        {
            a.OnSendMessage -= HandleLogicOfChild;
        }
    }
}
