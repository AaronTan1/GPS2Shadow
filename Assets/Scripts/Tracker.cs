using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Transform trackedObject;
    public float updateSpeed = 10f;
    public Vector3 trackingOffset;
    
    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, trackedObject.position + trackingOffset,
            updateSpeed * Time.deltaTime);
    }
}
