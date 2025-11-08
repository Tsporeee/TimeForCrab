using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IngredientGrabScript : MonoBehaviour
{
    private Rigidbody ingredientRigidbody;
    private Transform clawGrabPointTransform;

    private void Awake()
    {
        ingredientRigidbody = GetComponent<Rigidbody>();
        ingredientRigidbody.useGravity = true;
    }

    public void Grab(Transform clawGrabPointTransform)
    {

        // Update 
        this.clawGrabPointTransform = clawGrabPointTransform;
        ingredientRigidbody.useGravity = false;
    }

    public void Drop()
    {
        this.clawGrabPointTransform = null;
        ingredientRigidbody.useGravity = true;
    }

    private void FixedUpdate()
    {
        float lerpSpeed = 15f;
        if (clawGrabPointTransform != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, clawGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            ingredientRigidbody.MovePosition(newPosition);
        }
    }

}
