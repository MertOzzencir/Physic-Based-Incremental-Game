using System.Collections;
using UnityEditor;
using UnityEngine;

public abstract class Tools : MonoBehaviour
{


    public abstract void OnEquippedLogic();

    public abstract void OnDeEquippedLogic();

    public ToolState state;
    public ToolControllers toolController;
    public virtual void Awake()
    {
        toolController = GetComponent<ToolControllers>();
    }

    public virtual IEnumerator HandleToolAnimation(Transform toolTransform, Vector3 placementPosition, Vector3 directionVector, bool stateMood)
    {
        toolController.enabled = false;
        Debug.Log(toolTransform.name);
        Quaternion lookRotation = Quaternion.LookRotation(directionVector, Vector3.up);
        while (Vector3.Distance(toolTransform.position, placementPosition) > 0.1f)
        {
            toolTransform.position = Vector3.Lerp(toolTransform.position, placementPosition, 15 * Time.deltaTime);
            toolTransform.rotation = Quaternion.Lerp(toolTransform.rotation, lookRotation, 15* Time.deltaTime);
            yield return null;
        }
        toolTransform.position = placementPosition;
        toolTransform.transform.rotation = lookRotation;
        toolController.enabled = true;
        this.enabled = stateMood;
    }
    public enum ToolState
    {
        Equiped,
        DeEquiped
    }

}
