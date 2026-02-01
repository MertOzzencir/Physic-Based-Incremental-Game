using System.Collections;
using UnityEngine;

public class Drops : MonoBehaviour, ICollectable
{
    public bool IsCollectable { get; set; } = true;

    public void Collect(Transform toolTransform)
    {
        if (IsCollectable)
        {
            IsCollectable = false;
            StartCoroutine(CollectAnimation(toolTransform));
        }
    }

    IEnumerator CollectAnimation(Transform toolTransform)
    {
        while (Vector3.Distance(toolTransform.position, transform.position) > 0.6f)
        {
            transform.position = Vector3.Lerp(transform.position, toolTransform.position, 12f * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);

    }



}
