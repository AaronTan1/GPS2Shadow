using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBlightRangeDetect : MonoBehaviour
{
    public ScaleBlightBehaviour sbb;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sbb.PlayerInRange();
        }
    }
}
