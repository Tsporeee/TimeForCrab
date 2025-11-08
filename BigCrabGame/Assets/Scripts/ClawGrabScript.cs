using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawGrabScript : MonoBehaviour
{

    [SerializeField] private Transform clawTransform;
    [SerializeField] private Transform clawGrabPointTransform;
    [SerializeField] private LayerMask clawLayerMask;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IngredientGrabScript ingredientGrabScript))
        {
            ingredientGrabScript.Grab(clawGrabPointTransform);
        }
    }


}

// Graveyard
//if (Input.GetKeyDown(KeyCode.E))
     // {
     //     float pickUpDistance = 0.5f;
    //     if (Physics.Raycast(clawTransform.position, clawTransform.forward, out RaycastHit raycasthit, pickUpDistance))
     //     {
     //         if (raycasthit.transform.TryGetComponent(out IngredientGrabScript ingredientGrabScript))
     //         {
     //             ingredientGrabScript.Grab(clawGrabPointTransform);
    //         }
                
     //     }
     // }