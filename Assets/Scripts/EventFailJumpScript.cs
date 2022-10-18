using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFailJumpScript : MonoBehaviour
{
    [SerializeField] public GameObject hidden2DCol;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(hidden2DCol);
        }
    }
}
