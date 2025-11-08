using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClawScript : MonoBehaviour
{
    public Transform clawTransform = null;
    public float horizontalMaxPosition = 2f;
    public float verticalMaxPosition = 2f;
    public float speed = 1f;
    public float shakePosition = 0.5f;

    // Start is called before the first frame update
    public void Start()
    {
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

        clawTransform.localPosition += (new Vector3(horz, 0f, vert) * speed);

        clawTransform.localPosition = new Vector3(
            Mathf.Clamp(clawTransform.localPosition.x, -horizontalMaxPosition, horizontalMaxPosition),
            clawTransform.localPosition.y,
            Mathf.Clamp(clawTransform.localPosition.z, -verticalMaxPosition, verticalMaxPosition));

        shakeClaw();

    }

    public void shakeClaw()
    {
        clawTransform.localPosition += (new Vector3(Random.Range(0f, 0.2f), 0f, Random.Range(0f, 0.2f))) * shakePosition;
    }
}
