using System;
using UnityEngine;

public class AnimationEventSender : MonoBehaviour
{
   public event Action OnAnimationTrigger;

   public void SendEvent()
    {
        OnAnimationTrigger?.Invoke();
    }
}
