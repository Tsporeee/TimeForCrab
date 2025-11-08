using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientGrabScript : MonoBehaviour
{
    private Rigidbody ingredientRigidbody;
    private Transform clawGrabPointTransform;

    private void Awake()
    {
        ingredientRigidbody = GetComponent<Rigidbody>();
    }

    public void Grab(Transform clawGrabPointTransform)
    {

        // Update 
        this.clawGrabPointTransform = clawGrabPointTransform;
    }

    private void FixedUpdate()
    {
        if (clawGrabPointTransform != null)
        {
            ingredientRigidbody.MovePosition(clawGrabPointTransform.position);
        }
    }

}
