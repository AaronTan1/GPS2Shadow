using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cabinetPuzzle : MonoBehaviour
{
    private bool hook;

    void Start()
    {
        hook = false;    
    }

    public void toggleHook()
    {
        if (hook && playerCandleScript.restrictMode == false)
        {

        }
    }

   

    void FixedUpdate()
    {
        
    }

}
