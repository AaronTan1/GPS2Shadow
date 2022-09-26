using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshStaticShadows : MonoBehaviour
{
    //Refresh all static shadows
    //More paramaters to be added
    [SerializeField] Transform initialLightPosition;
    [SerializeField] Transform initialWallPosition;
    Transform[] trList;

    public void RefreshAllStaticShadows()
    {
        trList = new Transform[] { initialWallPosition, initialLightPosition };

        BroadcastMessage("CastFakeShadow", trList, SendMessageOptions.DontRequireReceiver);
    }
}
