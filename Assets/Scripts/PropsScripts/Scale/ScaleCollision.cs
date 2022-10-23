using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86;

public class ScaleCollision : MonoBehaviour
{
    private ScaleBehaviour scaleBe;

    [SerializeField] bool isLeftPlate;
    static float tiltCooldown = 0.5f;
    static float resetTiltCooldwon = 0.1f;

    public static bool isTilting = false, isReseting = false;
    Coroutine crTimer = null, crReset = null;

    private void Awake()
    {
        scaleBe = GetComponentInParent<ScaleBehaviour>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!isTilting)
            {
                isTilting = true;
                if(crTimer!= null)
                {
                    return;
                }
                crTimer = StartCoroutine(QuickTiltCooldwon());


                if (isLeftPlate)
                {
                    scaleBe.TiltScale(scaleBe.playerTiltAngle);
                }
                else
                {
                    scaleBe.TiltScale(-scaleBe.playerTiltAngle);
                }
            }              
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isTilting && !isReseting)
            {
                if(crReset!= null)
                {
                    return;
                }
                crReset = StartCoroutine(ResetTiltCooldown());
                scaleBe.TiltScale(0);
            }
        }
    }

    IEnumerator QuickTiltCooldwon()
    {
        yield return new WaitForSeconds(tiltCooldown);
        isTilting = false;
        crTimer = null;
    }
    IEnumerator ResetTiltCooldown()
    {
        yield return new WaitForSeconds(resetTiltCooldwon);
        isReseting = false;
        crReset = null;
    }
}
