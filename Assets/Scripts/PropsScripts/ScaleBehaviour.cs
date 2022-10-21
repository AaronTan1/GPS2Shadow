using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class ScaleBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject leftPlate;
    [SerializeField] GameObject rightPlate;
    [SerializeField] GameObject upperBody;

    // This is seperated from the parent as to not rotate relative to the parent
    [SerializeField] GameObject leftPlateShadow;
    [SerializeField] GameObject rightPlateShadow;
    [SerializeField] GameObject upperBodyShadow;

    [SerializeField] BoxCollider2D leftPlateCollider;
    [SerializeField] BoxCollider2D rightPlateCollider;

    [Header("Settings")]
    [SerializeField] private float currentTiltValue;
    [SerializeField] float tiltDuration;
    [SerializeField] float tiltFrames;
    [SerializeField] public float playerTiltAngle;
    [SerializeField] float testValue;

    Coroutine tiltCr = null;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            TiltScale(testValue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == leftPlateCollider)
        {
            Debug.Log("Left");
        }
        if (collision == rightPlateCollider)
        {
            Debug.Log("Right");
        }
    }

    public void TiltScale(float tiltVal)
    {
        if (tiltCr != null)
        {
            StopCoroutine(tiltCr);
        }
        tiltCr = StartCoroutine(TiltScaleIncrementally(tiltVal));
    }
    IEnumerator TiltScaleIncrementally(float newTiltValue)
    {
        float splitTiltValue = (newTiltValue - currentTiltValue) / tiltFrames;
        float splitTiltDuration = tiltDuration / tiltFrames;
        float localTiltFrame = tiltFrames;

        while(localTiltFrame > 0)
        {
            upperBody.transform.localEulerAngles += new Vector3(0, 0, splitTiltValue);
            upperBodyShadow.transform.localEulerAngles += new Vector3(0, 0, splitTiltValue);

            leftPlate.transform.localEulerAngles += new Vector3(0, 0, -splitTiltValue);
            rightPlate.transform.localEulerAngles += new Vector3(0, 0, -splitTiltValue);

            leftPlateShadow.transform.localEulerAngles += new Vector3(0, 0, -splitTiltValue);
            rightPlateShadow.transform.localEulerAngles += new Vector3(0, 0, -splitTiltValue);

            localTiltFrame--;
            yield return new WaitForSeconds(splitTiltDuration);
        }
        upperBody.transform.localEulerAngles = new Vector3(0, 0, newTiltValue);
        upperBodyShadow.transform.localEulerAngles = new Vector3(0, 0, newTiltValue);

        leftPlate.transform.localEulerAngles = new Vector3(0, 0, -newTiltValue);
        rightPlate.transform.localEulerAngles = new Vector3(0, 0, -newTiltValue);

        leftPlateShadow.transform.localEulerAngles = new Vector3(0, 0, -newTiltValue);
        rightPlateShadow.transform.localEulerAngles = new Vector3(0, 0, -newTiltValue);

        currentTiltValue = newTiltValue;
        tiltCr = null;
    }
}
