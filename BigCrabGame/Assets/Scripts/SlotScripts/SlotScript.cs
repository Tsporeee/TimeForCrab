using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotScript : MonoBehaviour
{
    // Ok so were gonna have to have a collision check, an isfilled boolean, a check for if its the right ingredient?
    // cant use a tag, add an instance variable and a setter

    public String correctFood;
    private GameObject filledFood;

    // I think for now make it so the wrong one cant fill it
    public bool isFilled = false;
    public GameObject touchingFood = null;

    private void OnTriggerEnter(Collider other)
    {
         if (isFilled || !other.CompareTag("Ingredient"))
        {
            return;
        }

        IngredientGrabScript ingredient = other.GetComponent<IngredientGrabScript>();
        if (ingredient.getName() == correctFood)
        {
            isFilled = true;
            filledFood = other.gameObject;
            
            // Freeze food n disable grabbing then snap
            Rigidbody foodRigidBody = filledFood.GetComponent<Rigidbody>();
            foodRigidBody.isKinematic = true;
            ingredient.enabled = false;
            
            //filledFood.transform.position = transform.position; snap is wayyy buggy just leave it for now
        }
    }
    
}
