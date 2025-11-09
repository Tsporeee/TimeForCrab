using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClawScript : MonoBehaviour
{
    public Transform clawGrabPointTransform;
    public float horizontalMaxPosition = 2f;
    public float verticalMaxPosition = 2f;
    public float speed = 5f;

    // Passing other scripts
    //private IngredientGrabScript ingredientGrabScript;
    public Animator animator;

    private bool isDipping;
    private bool disableMovement;

    private float startPositionY;

    // Shaking
    public float shakeDuration = 2f;
    public AnimationCurve curve;

    private Coroutine clawShakeCoroutine = null;

    // Start is called before the first frame update
    public void Start()
    {
        
        startPositionY = transform.position.y;
        // Fake infinite loop
        InvokeRepeating(nameof(shakeClawMethod), 0f, 2f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //if (ingredientGrabScript != null)
                //{
                //    ingredientGrabScript.Drop();
                //    ingredientGrabScript = null;
                //}
                if (!isDipping)
            {
                StartCoroutine(dipClaw());
            }
        }
    }

    public void FixedUpdate()
    {
        if (!disableMovement)
        {
            moveClaw();
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent(out IngredientGrabScript ingredientGrabScript))
    //    {
    //        ingredientGrabScript.Grab(clawGrabPointTransform);
    //        this.ingredientGrabScript = ingredientGrabScript;
    //    }
    //}

    public void moveClaw()
    {
        
        // Vertical and horizontal, but in game: moving x and z
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");

        transform.position += new Vector3(horz, 0f, vert) * speed * Time.deltaTime;

    }
    
    public void shakeClawMethod()
    {
        // Move this to a normal method to invoke repeating
        // Dont restart if theres already a shake running
        if (clawShakeCoroutine == null)
        clawShakeCoroutine = StartCoroutine(shakeClaw());
    }

    public void StopShakeClaw()
    {
        if (clawShakeCoroutine != null)
        {
            StopCoroutine(clawShakeCoroutine);
            clawShakeCoroutine = null;
        }
    }

    IEnumerator shakeClaw()
    {
        
        float elapsedTime = 0f;
        float shakeSpeed = 3f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // -0.5f centers noise
            // Perlin noise makes a bit of a drift but better than the super jittery other options for now
            float xNoise = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f;
            float zNoise = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f;
            
            float shakeStrength = curve.Evaluate(elapsedTime/shakeDuration);

            Vector3 newPosition = transform.position + (new Vector3(xNoise, 0f, zNoise) * shakeStrength);
            transform.position = newPosition;


            // Container for movement
            //clawTransform.position = new Vector3(
            //Mathf.Clamp(clawTransform.position.x, -horizontalMaxPosition, horizontalMaxPosition),
            //clawTransform.position.y,
            //Mathf.Clamp(clawTransform.position.z, -verticalMaxPosition, verticalMaxPosition));

            yield return null;
        }

        clawShakeCoroutine = null;
    }
    IEnumerator dipClaw()
    {

        //BoxCollider boxCollider = GetComponent<BoxCollider>();
        //StopShakeClaw();

        float elapsedTime = 0f;
        float dipDownDuration = 1f;
        float dipUpDuration = 0.5f;

        //float startPositionY = transform.position.y;

        isDipping = true;
        disableMovement = true;
        
        while (elapsedTime < dipDownDuration)
        {
            StopShakeClaw();
            elapsedTime += Time.deltaTime;

            // could do this later
            // float dipStrength = curve.Evaluate(elapsedTime / shakeDuration);
            float lerpSpeed = 5f;

            // Base the down position on your position already pls
            Vector3 downPosition = new Vector3(transform.position.x, startPositionY - 2f, transform.position.z);
            Vector3 lerpedDownPosition = Vector3.Lerp(transform.position, downPosition, Time.deltaTime * lerpSpeed);
            transform.position = lerpedDownPosition;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        elapsedTime = 0f;

        while (elapsedTime < dipUpDuration)
        {
            StopShakeClaw();
            elapsedTime += Time.deltaTime;

            // could do this later
            // float dipStrength = curve.Evaluate(elapsedTime / shakeDuration);
            float lerpSpeed = 15f;

            Vector3 upPosition = new Vector3(transform.position.x, startPositionY, transform.position.z);
            Vector3 lerpedUpPosition = Vector3.Lerp(transform.position, upPosition, Time.deltaTime * lerpSpeed);
            transform.position = lerpedUpPosition;

            yield return null;
        }

        // Really just in case something goes wrong
        transform.position = new Vector3(transform.position.x, startPositionY, transform.position.z);
        isDipping = false;
        disableMovement = false;
        shakeClawMethod();

    }
}

// Graveyard... 
// Vector3 startPosition = clawTransform.localPosition;
// clawTransform.localPosition = startPosition + new Vector3(xNoise, 0f, zNoise) * shakeStrength;
// float xNoise = ((Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2);
// float zNoise = ((Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2);
//Vector3 lerpedNewPosition = Vector3.Lerp(clawTransform.localPosition, newPosition, Time.deltaTime * lerpSpeed);
//Vector3 lerpedNewPosition = Vector3.Lerp(clawTransform.localPosition, newPosition, Time.deltaTime * lerpSpeed);