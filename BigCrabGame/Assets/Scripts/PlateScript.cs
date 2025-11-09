using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour
{
    // ok so were gonna need a win boolean to broadcast to a game manager and a for loop checking each slot if it has won 
    public SlotScript[] slots;

    void Update()
    {

    }

    public bool isPlateCorrect()
    {
        foreach (SlotScript slotScript in slots)
        {
            if (!slotScript.isFilled)
            {
                return false;
            }
        }
        return true;
    }

}
