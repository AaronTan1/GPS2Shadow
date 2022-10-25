using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierCollision : MonoBehaviour
{
    ChandelierBehaviour parentScript;
    private void Awake()
    {
        parentScript = GetComponentInParent<ChandelierBehaviour>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            parentScript.ShakeChandelierRemote();
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            parentScript.StopChandelier();
    }
}
