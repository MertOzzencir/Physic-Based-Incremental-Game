using System;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    private enum InteractMode
    {
        NormalMode,
        BreakMode
    }
    public static Vector3 LastInteractedPosition;
    private void InteractLogic(bool interactState)
    {
        if (!interactState)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (var a in hits)
        {
            if (a.collider.gameObject.TryGetComponent(out IInteractable interacted))
            {
                LastInteractedPosition = a.point;
                interacted.Interact();
                break;
            }
        }
    }


    void OnEnable()
    {
        InputManager.OnLeftClickAction += InteractLogic;
    }



    void OnDisable()
    {
        InputManager.OnLeftClickAction -= InteractLogic;
    }

}