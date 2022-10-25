using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierBehaviour : MonoBehaviour
{
    [SerializeField] Transform chandBody;
    [SerializeField] Transform chandShadow;

    [SerializeField] private float shakeTimePerInterval;
    [SerializeField] private int shakeFrame;
    [SerializeField] private float shakeAngle;

    private bool isSwingingToRight = true;
    private bool stopSwing = true;

    Coroutine swingCr = null;
    public void ShakeChandelierRemote()
    {       
        if(swingCr == null)
        {
            stopSwing = false;
            swingCr = StartCoroutine(ShakeChandelier(shakeAngle / 2, shakeTimePerInterval / 2, false));
        }      
    }

    public void StopChandelier()
    {
        stopSwing = true;
    }
    IEnumerator ShakeChandelier(float angle, float shakeInterv, bool breakThisIteration = false)
    {
        

        float shakeSplit = angle / shakeFrame;
        float durSplit = shakeInterv / shakeFrame;
        int localFrame = shakeFrame;

        while (localFrame > 0)
        {
            chandBody.localEulerAngles += new Vector3(0,0,shakeSplit);
            chandShadow.localEulerAngles += new Vector3(0, 0, shakeSplit);
            localFrame--;
            yield return new WaitForSeconds(durSplit);
        }
        if (breakThisIteration)
        {
            swingCr = null; 
            yield break;
        }


            if (isSwingingToRight)
        {
            swingCr = StartCoroutine(ShakeChandelier(stopSwing ? -shakeAngle / 2 : -shakeAngle, shakeTimePerInterval, stopSwing));
            isSwingingToRight = stopSwing;
        }
        else
        {
            swingCr = StartCoroutine(ShakeChandelier(stopSwing ? shakeAngle / 2 : shakeAngle, shakeTimePerInterval, stopSwing));
            isSwingingToRight = true;
        }
        

    }
}
