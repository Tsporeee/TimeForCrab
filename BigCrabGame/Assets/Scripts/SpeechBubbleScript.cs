using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleScript : MonoBehaviour
{
    public CustomerScript customerScript;
    public float timer = 5f;

    void Start()
    {
        StartCoroutine(speechBubble());
    }
    
    void Update()
    {
        if (customerScript.hasCelebrated)
        {
            gameObject.SetActive(false); 
        }
    }

    IEnumerator speechBubble()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }
}
