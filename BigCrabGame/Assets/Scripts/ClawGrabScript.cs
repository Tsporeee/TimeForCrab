using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ClawGrabScript : MonoBehaviour
{

    [SerializeField] private Transform clawTransform;
    [SerializeField] private Transform clawGrabPointTransform;
    [SerializeField] private LayerMask clawLayerMask;

    // Passing other scripts
    public IngredientGrabScript ingredientGrabScript;
    public ClawScript clawScript;
    public Animator animator;

    private bool isDipping;

    void Start()
    {
        //animator = GetComponent<Animator>();
        //animator.SetTrigger("Dip");
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ingredientGrabScript != null)
            {
                ingredientGrabScript.Drop();
                ingredientGrabScript = null;
            }

            else
            {
                StartCoroutine(dipClaw());
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IngredientGrabScript ingredientGrabScript))
        {
            ingredientGrabScript.Grab(clawGrabPointTransform);
            this.ingredientGrabScript = ingredientGrabScript;
        }
    }
    public void freezeClaw()
    {
        Rigidbody clawRigidbody = GetComponent<Rigidbody>();
        clawRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    public void unfreezeClaw()
    {
        Rigidbody clawRigidbody = GetComponent<Rigidbody>();
        clawRigidbody.constraints = RigidbodyConstraints.None;
    }

    IEnumerator dipClaw()
    {
        clawScript.StopShakeClaw();
       
        float elapsedTime = 0f;
        float dipDownDuration = 2f;
        float dipUpDuration = 0.5f;

        freezeClaw();
        Vector3 startPosition = clawTransform.position;

        while (elapsedTime < dipDownDuration)
        {
            elapsedTime += Time.deltaTime;

            // could do this later
            // float dipStrength = curve.Evaluate(elapsedTime / shakeDuration);
            float lerpSpeed = 2f;

            Vector3 downPosition = clawTransform.localPosition + new Vector3(0f, -1f, 0f);
            Vector3 lerpedDownPosition = Vector3.Lerp(clawTransform.localPosition, downPosition, Time.deltaTime * lerpSpeed);
            clawTransform.localPosition = lerpedDownPosition;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        elapsedTime = 0f;

        while (elapsedTime < dipUpDuration)
        {
            elapsedTime += Time.deltaTime;

            // could do this later
            // float dipStrength = curve.Evaluate(elapsedTime / shakeDuration);
            float lerpSpeed = 2f;

            Vector3 upPosition = clawTransform.localPosition + new Vector3(0f, 2f, 0f);
            Vector3 lerpedUpPosition = Vector3.Lerp(clawTransform.localPosition, upPosition, Time.deltaTime * lerpSpeed);
            clawTransform.localPosition = lerpedUpPosition;

            yield return null;
        }

        unfreezeClaw();

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
//public void dipClawMethod()
//{
//    float lerpSpeed = 1f;
//    freezeClaw();
//    Vector3 newPosition = transform.localPosition + new Vector3(0f, -30, 0f);
//    Vector3 lerpedNewPosition = Vector3.Lerp(transform.localPosition, newPosition, Time.deltaTime * lerpSpeed);
//    transform.localPosition = lerpedNewPosition;
//}