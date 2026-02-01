

using UnityEngine;

public interface ICollectable
{
    public bool IsCollectable { get; set; }
    public void Collect(Transform toolTransform);
}
