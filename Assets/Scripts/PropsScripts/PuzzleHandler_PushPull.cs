using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler_PushPull : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 endPosition;
    [SerializeField] private Vector3 moveDelta;
    
    private bool isVertical;
    private float input;
    private float moveSpeed = 1f;

    [HideInInspector] public bool isActive;
    private float inputOld;
    private float inputNew;
    
    private void Start()
    {
        initialPosition = transform.localPosition;
        endPosition = initialPosition + moveDelta;
        Debug.Log(initialPosition);
        Debug.Log(endPosition);
    }

    void Update()
    {
        if (!isActive) return;
        input = !isVertical ? joystickManager.Instance.InputHorizontal() : joystickManager.Instance.InputVertical();

        if (input != 0)
        {
            inputOld = inputNew;
            inputNew = input;
        }
        
        switch (input)
        {
            case > 0:
                MoveObject(endPosition, moveSpeed);
                break;
            case < 0:
                MoveObject(initialPosition, moveSpeed);
                break;
        }
    }

    void MoveObject(Vector3 pos, float t)
    {
        if (inputOld * inputNew < 0)
            StopAllCoroutines();
        StartCoroutine(LerpPosition(pos,CalculateMoveTime(pos)));
    }

    private float CalculateMoveTime(Vector3 pos)
    {
        var totalDistance = Vector3.Distance(endPosition, initialPosition);
        var currentDistance = Vector3.Distance(transform.localPosition, pos);
        if (Math.Abs(currentDistance - totalDistance) < 0.01) return moveSpeed;
        
        var t = currentDistance/totalDistance * moveSpeed;
        return t;
    }
    
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.localPosition;
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;
    }
}
