using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFakeShadow : MonoBehaviour
{
    [SerializeField] Transform shadowTR;
    [SerializeField] Transform wallTR;
    [SerializeField] Transform lightTR;
    [SerializeField] float speed;

    public void CastFakeShadow(Transform[] trList)
    {
        wallTR = trList[0];
        lightTR = trList[1];
        if(shadowTR == null)
        {
            shadowTR.GetComponentInChildren<Transform>();
        }

        float wallZ = wallTR.position.z;
        float distanceToWall = wallZ - transform.position.z;
        float distanceToLight = transform.position.z - lightTR.position.z;

        if(distanceToWall > distanceToLight)
        {
            shadowTR.localScale = shadowTR.localScale * (distanceToWall / distanceToLight);
        }
        else
        {
            shadowTR.localScale = new Vector3(1,1,1);
        }
        
        shadowTR.position = new Vector3(transform.position.x, transform.position.y, wallZ - 0.01f);

        Debug.Log("casting fake shadow");
    }

}
