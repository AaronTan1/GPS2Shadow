using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StaticFakeShadow : MonoBehaviour
{
    [SerializeField] Transform shadowTR;
    private Transform wallTR;
    private Transform lightTR;

    [Tooltip("1 by default, 0.5 to offset it by 50%, 2 to 200%")]
    [Range(0.0f, 2.0f)]
    [SerializeField] float sizePercentageOffset = 1;

    public void CastFakeShadow(Transform[] trList)
    {
        wallTR = trList[0];
        lightTR = trList[1];


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

        if (distanceToWall > distanceToLight)
        {
            shadowTR.localScale = new Vector3(1, 1, 1) * (distanceToWall / distanceToLight);
        }
        else
        {
            shadowTR.localScale = new Vector3(1, 1, 1);//Clamp minimum size
        }
        shadowTR.localScale *= sizePercentageOffset;
        shadowTR.position = new Vector3(transform.position.x, transform.position.y, wallZ - 0.01f);
    }

}
