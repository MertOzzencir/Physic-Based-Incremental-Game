// using System.Collections;
// using UnityEngine;

// public class CollectController : Tools
// {
//     [SerializeField] private float toolMoveDistance;
//     [SerializeField] private GameObject toolPlacement;
//     [SerializeField] private GameObject collectTool;
//     [SerializeField] private LayerMask groundMask;



//     void Update()
//     {
//         if (state != ToolState.Equiped)
//             return;

//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
//         {
//             collectTool.transform.position = Vector3.Lerp(collectTool.transform.position, hit.point, 15 * Time.deltaTime);
//             if (Vector3.Distance(collectTool.transform.position, toolPlacement.transform.position) > toolMoveDistance)
//             {
//                 Debug.Log("Warning! Tool Too Far away");
//                 OnDeEquippedLogic();
//             }


//         }
//     }
//     public override void OnEquippedLogic()
//     {
//         state = ToolState.Equiped;
//         this.enabled = true;
//     }
//     public override void OnDeEquippedLogic()
//     {
//         state = ToolState.DeEquiped;
//         StartCoroutine(HandleToolAnimation(collectTool.transform, toolPlacement.transform.position, toolPlacement.transform.forward, false));
//     }

// }
