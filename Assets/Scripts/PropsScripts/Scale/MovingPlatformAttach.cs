using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatformAttach : MonoBehaviour
{
    //Works for shadow player only
    private float player_Z_PositionCache;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player_Z_PositionCache = collision.transform.position.z;
            collision.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Transform thisTr = collision.transform;
            thisTr.parent = null;
            thisTr.position = new Vector3(thisTr.position.x, thisTr.position.y, player_Z_PositionCache);
        }
    }
}
