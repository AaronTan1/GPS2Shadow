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

    [SerializeField] private Collider2D playerShadowCollider;
    [SerializeField] private Rigidbody2D playerShadowRB;

    [Header("Settings")]
    [SerializeField] private float currentTiltValue;
    [SerializeField] float tiltDuration;
    [SerializeField] float tiltFrames;
    [SerializeField] public float playerTiltAngle;
    [SerializeField] float testValue;

    [Header("Launch Values")]
    [SerializeField] float launchStrength;
    [SerializeField] float launchDirectionX;
    [SerializeField] float launchDirectionY;

    Coroutine tiltCr = null;

    private void Awake()
    {
        GameObject [] playerObjs = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject obj in playerObjs)
        {
            if(obj.GetComponent<Collider2D>()!= null)
            {
                playerShadowCollider = obj.GetComponent<Collider2D>();
                playerShadowRB = obj.GetComponent<Rigidbody2D>();
            }
        }
    }
    void Update()
    {
        //Would be removed for build
        if(Input.GetKeyDown(KeyCode.R))
        {
            TiltScale(testValue);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerLaunch();
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

    public void PlayerLaunch()//TO be called by bligth when slamming
    {
        
        if (leftPlateCollider.IsTouching(playerShadowCollider))
        {
            leftPlateCollider.enabled = false;
            Vector2 targetDir = new Vector2(launchDirectionX, launchDirectionY).normalized;
            playerShadowRB.AddForce(targetDir * launchStrength * 10);
            TiltScale(-45);
            Invoke("AfterLaunch", 0.5f);//Should be bigger than 0.3f(Tilt time) most of the time in case
        }
        else
        {
            Debug.Log("Player not on left plate");
        }
    }

    public bool ComponentsAreNull()
    {
        return (playerShadowCollider == null ||playerShadowRB == null)? true : false;
    }
    public void PassComponents(Collider2D col, Rigidbody2D rbPly)
    {
        playerShadowCollider = col;
        playerShadowRB = rbPly;
    }

    void AfterLaunch()
    {
        leftPlateCollider.enabled = true;
        TiltScale(0);
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
