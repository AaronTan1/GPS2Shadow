using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp
{
    private bool isMoving;
    
    public IEnumerator LerpCoroutine(Transform obj, Vector3 targetPosition, float duration, Transform lookAtTarget = null)
    {
        isMoving = true;
        float time = 0;
        
        while (time < duration)
        {
            if (lookAtTarget != null) obj.LookAt(lookAtTarget);
            
            obj.position = Vector3.Lerp(obj.position, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        obj.position = targetPosition;
        isMoving = false;
    }
}
