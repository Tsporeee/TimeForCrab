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

    // Shaking
    public float shakeDuration = 5f;
    public AnimationCurve curve;

    // Start is called before the first frame update
    public void Start()
    {
        InvokeRepeating(nameof(shakeClawMethod), 0f, 5f);
    }
    public void Update()
    {
    }

    public void FixedUpdate()
    {
        moveClaw();
    }

    public void moveClaw()
    {
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");

        clawTransform.localPosition += (new Vector3(horz, 0f, vert) * speed * Time.deltaTime);

        clawTransform.localPosition = new Vector3(
            Mathf.Clamp(clawTransform.localPosition.x, -horizontalMaxPosition, horizontalMaxPosition),
            clawTransform.localPosition.y,
            Mathf.Clamp(clawTransform.localPosition.z, -verticalMaxPosition, verticalMaxPosition));

    }
    public void shakeClawMethod()
    {
        StartCoroutine(shakeClaw());
    }


    IEnumerator shakeClaw()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float shakeStrength = curve.Evaluate(shakeDuration);
            clawTransform.localPosition += (new Vector3(Random.Range(-1f, 1f) * shakeStrength, 0f, (Random.Range(-1f, 1f)) * shakeStrength));
            yield return null;
        }
    }
}