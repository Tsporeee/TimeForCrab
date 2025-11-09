using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{

    public PlateScript plateScript;
    
    // Can change to get with a getter for privacy 
    public bool hasCelebrated;

    void Update()
    {
        if (plateScript.isPlateCorrect() && hasCelebrated == false)
        {
            StartCoroutine(celebrate());
        }
    }
    
    IEnumerator celebrate()
    {

        float jumpHalfDuration = 0.2f;

        Vector3 downPosition = transform.position;
        Vector3 upPosition = transform.position + new Vector3(0f, 0.5f, 0f);
        hasCelebrated = true;
        
        for (int i = 0; i < 5; i++)
        {
            float elapsedTime = 0f;
            while (elapsedTime < jumpHalfDuration)
            {
                elapsedTime += Time.deltaTime;
                float frameRateIndieTime = (elapsedTime / jumpHalfDuration);
                transform.position = Vector3.Lerp(downPosition, upPosition, frameRateIndieTime);
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < jumpHalfDuration)
            {
                elapsedTime += Time.deltaTime;
                float frameRateIndieTime = (elapsedTime / jumpHalfDuration);
                transform.position = Vector3.Lerp(upPosition, downPosition, frameRateIndieTime);
                yield return null;
            }
        }
    }
}
