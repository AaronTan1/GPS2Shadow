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

    [Header("Blight Related")]
    [SerializeField] ScaleBlightBehaviour sbb;

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

    private void Start()
    {
        
    }
    void Update()
    {
        /*//Would be removed for build
        if(Input.GetKeyDown(KeyCode.R))
        {
            TiltScale(testValue);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerLaunch();
        }*/
    }
    public void TiltScale(float tiltVal, float delay = 0f)
    {
        if (tiltCr != null)
        {
            StopCoroutine(tiltCr);
        }
        tiltCr = StartCoroutine(TiltScaleIncrementally(tiltVal, delay));
    }
    public void BlightPrepare()
    {
        //sbb.PrepareToAttack();
        Invoke("BlightSlam", 2f);
    }
    public void BlightSlam()
    {
        sbb.SlamOnScale();
        TiltScale(-30, 1f);
        Invoke("LaunchPlayer", 0.8f);
        Invoke("ReturnToNormalTilt", 3.5f);
    }
    public void LaunchPlayer()
    {
        if (leftPlateCollider.IsTouching(playerShadowCollider))
        {
            Vector2 targetDir = new Vector2(launchDirectionX, launchDirectionY).normalized;
            playerShadowRB.AddForce(targetDir * launchStrength * 10);
        }
    }
    public void ReturnToNormalTilt()
    {
        TiltScale(0);
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

    
    IEnumerator TiltScaleIncrementally(float newTiltValue, float delay)
    {
        yield return new WaitForSeconds(delay);

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

    /*IEnumerator BlightPlaysWithScale()
    {

    }*/
}
