using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler_PushPull : MonoBehaviour
{
    private Transform lookAtObject;
    private Vector3 initialPosition;
    private Vector3 endPosition;

    private bool isVertical;
    private float input;

    void Update()
    {
        input = !isVertical ? joystickManager.Instance.InputHorizontal() : joystickManager.Instance.InputVertical();

        switch (input)
        {
            case > 0:
                MoveObject(endPosition, 1);
                break;
            case < 0:
                MoveObject(initialPosition, 1);
                break;
        }
    }

    void MoveObject(Vector3 pos, float t)
    {
        StopAllCoroutines();
        StartCoroutine(LerpPosition(pos,t));
    }
    
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
