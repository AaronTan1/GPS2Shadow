using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFakeShadow : MonoBehaviour
{
    [SerializeField] Transform shadowTR;
    [SerializeField] Transform wallTR;
    [SerializeField] Transform lightTR;
    [SerializeField] float speed;
    public Rigidbody rb;
    public Rigidbody shadowRB;
    Vector3 ogScale;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        ogScale = shadowTR.localScale;
        CastFakeShadow();
        //StartCoroutine(CastShadow());
    }

    public void CastFakeShadow()
    {
        float wallZ = wallTR.position.z;


        float distanceToWall = wallZ - transform.position.z;
        float distanceToLight = transform.position.z - lightTR.position.z;

        shadowTR.localScale = ogScale * (distanceToWall / distanceToLight);
        shadowTR.position = new Vector3(shadowTR.position.x, shadowTR.position.y, wallZ - 0.01f);
    }

    IEnumerator CastShadow()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            CastFakeShadow();
        }
    }
}
