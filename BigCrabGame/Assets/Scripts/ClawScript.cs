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
    public Transform clawTransform = null;
    public Transform clawGrabPointTransform;
    public float horizontalMaxPosition = 2f;
    public float verticalMaxPosition = 2f;
    public float speed = 5f;
    private Vector3 inputPosition;

    // Passing other scripts
    //private IngredientGrabScript ingredientGrabScript;
    public Animator animator;

    private bool isDipping;


    // Shaking
    public float shakeDuration = 5f;
    public AnimationCurve curve;

    private Coroutine clawShakeCoroutine = null;

    // Start is called before the first frame update
    public void Start()
    {
        
        // Fake infinite loop
        InvokeRepeating(nameof(shakeClawMethod), 0f, 5f);
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
        moveClaw();
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

        inputPosition = clawTransform.position + (new Vector3(horz, 0f, vert) * speed * Time.deltaTime);
        clawTransform.position = inputPosition;

    }
    
    public void shakeClawMethod()
    {
        
        // Move this to a normal method to invoke repeating
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
            float xNoise = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f);
            float zNoise = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f);
            
            float shakeStrength = curve.Evaluate(elapsedTime/shakeDuration);

            Vector3 newPosition = inputPosition + (new Vector3(xNoise, 0f, zNoise) * shakeStrength);
            clawTransform.position = newPosition;


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
        StopShakeClaw();

        float elapsedTime = 0f;
        float dipDownDuration = 1f;
        float dipUpDuration = 0.5f;

        float startPositionY = clawTransform.position.y;

        isDipping = true;

        while (elapsedTime < dipDownDuration)
        {
            elapsedTime += Time.deltaTime;

            // could do this later
            // float dipStrength = curve.Evaluate(elapsedTime / shakeDuration);
            float lerpSpeed = 2f;

            Vector3 downPosition = clawTransform.position + new Vector3(0f, -1f, 0f);
            Vector3 lerpedDownPosition = Vector3.Lerp(clawTransform.position, downPosition, Time.deltaTime * lerpSpeed);
            clawTransform.position = lerpedDownPosition;

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

            Vector3 upPosition = clawTransform.position + new Vector3(0f, 2f, 0f);
            Vector3 lerpedUpPosition = Vector3.Lerp(clawTransform.position, upPosition, Time.deltaTime * lerpSpeed);
            clawTransform.position = lerpedUpPosition;

            yield return null;
        }

        clawTransform.position = new Vector3(clawTransform.position.x, startPositionY, clawTransform.position.z);
        isDipping = false;

    }
}

// Graveyard... 
// Vector3 startPosition = clawTransform.localPosition;
// clawTransform.localPosition = startPosition + new Vector3(xNoise, 0f, zNoise) * shakeStrength;
// float xNoise = ((Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2);
// float zNoise = ((Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2);
//Vector3 lerpedNewPosition = Vector3.Lerp(clawTransform.localPosition, newPosition, Time.deltaTime * lerpSpeed);
//Vector3 lerpedNewPosition = Vector3.Lerp(clawTransform.localPosition, newPosition, Time.deltaTime * lerpSpeed);