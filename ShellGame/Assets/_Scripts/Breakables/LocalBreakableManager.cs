using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocalBreakableManager : MonoBehaviour
{
    public List<LocalChildBreakable> Childs;


    public void HandleLogicOfChild(LocalChildBreakable child, Vector3 forceDirection)
    {

        //check the objects that connected to child and relaease them
        var snapshot = child.connectedToObject.ToArray();
        foreach (var a in snapshot)
        {
            if (a.rootObject != null)
            {
                if (a.connectedToObject.Count == 0)
                {
                    a.CollectState();
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


    public void OnChildDeath(LocalChildBreakable destroyedChild)
    {
        Destroy(destroyedChild.gameObject, 1f);
        Childs.Remove(destroyedChild);
        if (Childs.Count <= 0)
        {
            Destroy(gameObject);
        }
    }

}
