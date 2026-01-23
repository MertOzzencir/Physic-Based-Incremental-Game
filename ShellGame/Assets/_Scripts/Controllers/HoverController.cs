using UnityEngine;

public class HoverController : MonoBehaviour
{
    [SerializeField] private GameObject hoverIndicator;



    void Update()
    {
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore);
        // System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        // foreach (var a in hits)
        // {


        // }

    }
    public void HoverOnObject(Collider targetObject)
    {
        if (targetObject.TryGetComponent(out BoxCollider hoveredCollider))
        {
            var parent = hoveredCollider.transform;
            if (hoverIndicator.transform.parent != parent)
                hoverIndicator.transform.parent = parent;

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
        Debug.Log("Hover Enabled");
        hoverIndicator.SetActive(true);
    }
    void OnDisable()
    {
        Debug.Log("Hover Disabled");
        if (hoverIndicator != null)
        {
            hoverIndicator.transform.parent = null;
            hoverIndicator.SetActive(false);
        }
    }
}
