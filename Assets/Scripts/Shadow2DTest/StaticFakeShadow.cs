using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StaticFakeShadow : MonoBehaviour
{
    [SerializeField] Transform shadowTR;

    [Header("Wall and Light Ref")]
    [SerializeField] Transform wallTR;
    [SerializeField] Transform lightTR;

    [Tooltip("1 by default, 0.5 to offset it by 50%, 2 to 200%")]
    [Range(0.0f, 2.0f)]
    [SerializeField] float xSizePercentageOffset = 1;
    [Range(0.0f, 2.0f)]
    [SerializeField] float ySizePercentageOffset = 1;
    [Range(-20f, 20f)]
    [SerializeField] float xOffset = 0;
    [Range(-20f, 20f)]
    [SerializeField] float yOffset = 0;
    public void CastFakeShadow(Transform[] trList)
    {
        if(trList != null)
        {
            wallTR = trList[0];
            lightTR = trList[1];
        }
        else
        {
            if(wallTR == null || lightTR == null)
            {
                Debug.LogWarning("Wall or Light reference not set");
                return;
            }
        }
        


        if (shadowTR == null)
        {
            Transform foundTR = transform.Find("Shadow");
            if (foundTR != null)
            {
                shadowTR = foundTR;
            }
            else
            {
                Debug.Log($"Shadow not present in {gameObject.name}");
                return;
            }
        }

        float wallZ = wallTR.position.z;
        float distanceToWall = wallZ - transform.position.z;
        float distanceToLight = transform.position.z - lightTR.position.z;

        if (transform.position.z <= lightTR.position.z)
        {
            shadowTR.localScale = new Vector3(2, 2, 2);//Clamp max size and whenever object is behind light source instead, it's not logical but it's made to fit the level layout in case
        }
        else if (distanceToWall > distanceToLight)
        {
            shadowTR.localScale = new Vector3(1, 1, 1) * (distanceToWall / distanceToLight);
        }      
        else
        {
            shadowTR.localScale = new Vector3(1, 1, 1);//Clamp minimum size
        }
        shadowTR.localScale = new Vector3(shadowTR.localScale.x * xSizePercentageOffset, shadowTR.localScale.y * ySizePercentageOffset, shadowTR.localScale.z);
        shadowTR.position = new Vector3((transform.position.x + xOffset) * 1, transform.position.y + yOffset, wallZ - 0.01f);
    }

}
