using UnityEngine;

public class HoverController : MonoBehaviour
{
    [SerializeField] private GameObject hoverIndicator;

    Collider currentTarget;

    void Update()
    {
        
    }
    public void HoverOnObject(Collider targetObject)
    {
        if (targetObject.TryGetComponent(out BoxCollider hoveredCollider))
        {
            var parent = hoveredCollider.transform;
            currentTarget = targetObject;
            hoverIndicator.transform.position = hoveredCollider.transform.TransformPoint(hoveredCollider.center);
            hoverIndicator.transform.rotation = hoveredCollider.gameObject.transform.rotation;
            Vector3 worldSize = Vector3.Scale(hoveredCollider.size, hoveredCollider.transform.lossyScale) + Vector3.one * 0.01f;
            Vector3 p = parent.lossyScale;

            hoverIndicator.transform.localScale = new Vector3(
                worldSize.x / p.x,
                worldSize.y / p.y,
                worldSize.z / p.z
            );
        }
    }

    void OnEnable()
    {
        hoverIndicator.SetActive(true);
    }
    void OnDisable()
    {
        if (hoverIndicator != null)
        {
            hoverIndicator.transform.parent = null;
            hoverIndicator.SetActive(false);
        }
    }
}
