using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClawScript : MonoBehaviour
{
    public Transform clawTransform = null;
    public float horizontalMaxPosition = 2f;
    public float verticalMaxPosition = 2f;
    public float speed = 5f;


    // Shaking
    public float shakeDuration = 5f;
    public AnimationCurve curve;

    // Start is called before the first frame update
    public void Start()
    {
        
        // Fake infinite loop
        InvokeRepeating(nameof(shakeClawMethod), 0f, 5f);
    }

    public void FixedUpdate()
    {
        moveClaw();
    }

    public void moveClaw()
    {
        
        // Vertical and horizontal, but in game: moving x and z
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");

        float lerpSpeed = 20f;
        Vector3 newPosition = clawTransform.localPosition + (new Vector3(horz, 0f, vert) * speed * Time.deltaTime);
        Vector3 lerpedNewPosition = Vector3.Lerp(clawTransform.localPosition, newPosition, Time.deltaTime * lerpSpeed);
        clawTransform.localPosition = lerpedNewPosition;

    }
    
    public void shakeClawMethod()
    {
        
        // Move this to a normal method to invoke repeating
        StartCoroutine(shakeClaw());
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
            float lerpSpeed = 20f;

            Vector3 newPosition = clawTransform.localPosition + new Vector3(xNoise, 0f, zNoise) * shakeStrength;
            Vector3 lerpedNewPosition = Vector3.Lerp(clawTransform.localPosition, newPosition, Time.deltaTime * lerpSpeed);
            clawTransform.localPosition = lerpedNewPosition;


            // Container for movement
            clawTransform.localPosition = new Vector3(
            Mathf.Clamp(clawTransform.localPosition.x, -horizontalMaxPosition, horizontalMaxPosition),
            clawTransform.localPosition.y,
            Mathf.Clamp(clawTransform.localPosition.z, -verticalMaxPosition, verticalMaxPosition));

            yield return null;
        }
    }

    public void freezeClaw()
    {
        Rigidbody clawRigidbody = GetComponent<Rigidbody>();
        clawRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }
}

// Graveyard... 
// Vector3 startPosition = clawTransform.localPosition;
// clawTransform.localPosition = startPosition + new Vector3(xNoise, 0f, zNoise) * shakeStrength;
// float xNoise = ((Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2);
// float zNoise = ((Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2);