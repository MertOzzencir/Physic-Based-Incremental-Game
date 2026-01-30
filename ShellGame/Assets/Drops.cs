using System.Collections;
using UnityEngine;

public class Drops : MonoBehaviour, ICollectable
{
    public bool IsCollectable { get; set; } = true;

    public void Collect()
    {
        if (IsCollectable)
        {
            IsCollectable = false;
            Destroy(gameObject);
        }
    }

  

}
