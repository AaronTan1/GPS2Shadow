using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RefreshStaticShadows : MonoBehaviour
{
    //Refresh all static shadows
    //More paramaters can be added, need to pass an object of a class with all the parameters
    [SerializeField] Transform initialLightPosition;
    [SerializeField] Transform initialWallPosition;
    Transform[] trList;

    public void RefreshAllStaticShadows()
    {
        trList = new Transform[] { initialWallPosition, initialLightPosition };

        BroadcastMessage("CastFakeShadow", trList);
    }
}
