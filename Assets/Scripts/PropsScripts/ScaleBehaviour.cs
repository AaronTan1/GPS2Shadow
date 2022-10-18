using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Settings")]
    [Range(-60f, 60f)]
    [SerializeField] float currentTiltValue;
    [SerializeField] float tiltDuration;
    [SerializeField] float tiltFrames;

    [SerializeField] float testValue;

    Coroutine tiltCr = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(tiltCr != null)
            {
                StopCoroutine(tiltCr);
            }
            tiltCr = StartCoroutine(TiltScaleIncrementally(testValue));
        }
    }

    IEnumerator TiltScaleIncrementally(float newTiltValue)
    {
        float splitTiltValue = 0;
       /* if (currentTiltValue < newTiltValue)
        {
            splitTiltValue = (currentTiltValue + newTiltValue) / tiltFrames;
        }
        else if (currentTiltValue > newTiltValue)
        {
            splitTiltValue = (newTiltValue - currentTiltValue) / tiltFrames;
        }*/
        splitTiltValue = (newTiltValue - currentTiltValue) / tiltFrames;
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
        currentTiltValue = newTiltValue;
        tiltCr = null;
    }
}
