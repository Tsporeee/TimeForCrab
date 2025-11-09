using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleScript : MonoBehaviour
{
    public CustomerScript customerScript;

    void Start()
    {
         gameObject.SetActive(true); 
    }
    
    void Update()
    {
        if (customerScript.hasCelebrated)
        {
            gameObject.SetActive(false); 
        }
    }
}
