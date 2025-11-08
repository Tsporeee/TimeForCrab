using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClawScript : MonoBehaviour
{
    public Transform clawTransform = null;
    public float horizontalMaxPosition = 2f;
    public float verticalMaxPosition = 2f;
    public float speed = 1f;
    public Vector3 currentPosition;

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
        clawTransform.localPosition += (new Vector3(horz, 0f, vert) * speed * Time.deltaTime);

        currentPosition = clawTransform.localPosition;
        
        // Container for movement
        clawTransform.localPosition = new Vector3(
            Mathf.Clamp(clawTransform.localPosition.x, -horizontalMaxPosition, horizontalMaxPosition),
            clawTransform.localPosition.y,
            Mathf.Clamp(clawTransform.localPosition.z, -verticalMaxPosition, verticalMaxPosition));

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
            
            // -0.5f centers noise, makes half as strong, multiply it by two
            // Makes a bit of a drift but better than the super jittery other options for now
            // If we have time: FIX IT
            float xNoise = ((Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2);
            float zNoise = ((Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2);
            
            float shakeStrength = curve.Evaluate(elapsedTime/shakeDuration);

            clawTransform.localPosition = currentPosition + new Vector3(xNoise, 0f, zNoise) * shakeStrength;

            // Container for movement
            clawTransform.localPosition = new Vector3(
            Mathf.Clamp(clawTransform.localPosition.x, -horizontalMaxPosition, horizontalMaxPosition),
            clawTransform.localPosition.y,
            Mathf.Clamp(clawTransform.localPosition.z, -verticalMaxPosition, verticalMaxPosition));

            yield return null;
        }
    }
}

// Graveyard... 
// Vector3 startPosition = clawTransform.localPosition;
// clawTransform.localPosition = startPosition + new Vector3(xNoise, 0f, zNoise) * shakeStrength;
// float xNoise = ((Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2);
// float zNoise = ((Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2);