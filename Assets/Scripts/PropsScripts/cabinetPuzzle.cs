using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cabinetPuzzle : MonoBehaviour
{
    [SerializeField] public Material matOri, matSwapColor;
    [SerializeField] public GameObject[] drawerObj;
    public static bool switchFunction = false; //purple button switches function (Drawer Switch)
    private int index;
    private bool range, switchDraw, selectDraw;

    void Start()
    {
        index = 0;
        switchFunction = false;
        range = false;
        switchDraw = false;
        selectDraw = false;
    }

    public void toggleSwitchDraw()
    {
        if(switchDraw == false && range && playerCandleScript.restrictMode == false)
        {
            Debug.Log("TEST");
            
            if(index == 3)
            {
                index = 1;
            }
            else if(index >= 0)
            {
                index++;
            }

            switchDraw = true;

        }
        else
        {
            switchDraw = false;
        }
    }

    public void toggleSelectDraw()
    {
        if(selectDraw == false && range && playerCandleScript.restrictMode == false)
        {
            selectDraw = true;
        }
        else
        {
            selectDraw = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (range == false && playerCandleScript.restrictMode == false && switchFunction == false)
        {
            range = true;
            switchFunction = true;

        }

        if (other.tag == "Player" && switchDraw && range)
        {
            if(index == 1)
            {
                drawerObj[0].GetComponent<Renderer>().material = matSwapColor;
                drawerObj[1].GetComponent<Renderer>().material = matSwapColor;

                drawerObj[2].GetComponent<Renderer>().material = matOri;
                drawerObj[3].GetComponent<Renderer>().material = matOri;
                drawerObj[4].GetComponent<Renderer>().material = matOri;
                drawerObj[5].GetComponent<Renderer>().material = matOri;
            }
            else if(index == 2)
            {
                drawerObj[2].GetComponent<Renderer>().material = matSwapColor;
                drawerObj[3].GetComponent<Renderer>().material = matSwapColor;

                drawerObj[0].GetComponent<Renderer>().material = matOri;
                drawerObj[1].GetComponent<Renderer>().material = matOri;
                drawerObj[4].GetComponent<Renderer>().material = matOri;
                drawerObj[5].GetComponent<Renderer>().material = matOri;
            }
            else if(index == 3)
            {
                drawerObj[4].GetComponent<Renderer>().material = matSwapColor;
                drawerObj[5].GetComponent<Renderer>().material = matSwapColor;

                drawerObj[0].GetComponent<Renderer>().material = matOri;
                drawerObj[1].GetComponent<Renderer>().material = matOri;
                drawerObj[2].GetComponent<Renderer>().material = matOri;
                drawerObj[3].GetComponent<Renderer>().material = matOri;

            }
        }

        if(other.tag == "Player" && selectDraw && range)
        {
            if(index == 1)
            {
                drawerObj[0].GetComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
                drawerObj[2].GetComponent<FixedJoint>().connectedBody = null;
                drawerObj[4].GetComponent<FixedJoint>().connectedBody = null;
            }
            else if(index == 2){
                drawerObj[2].GetComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
                drawerObj[0].GetComponent<FixedJoint>().connectedBody = null;
                drawerObj[4].GetComponent<FixedJoint>().connectedBody = null;
            }
            else if(index == 3)
            {
                drawerObj[4].GetComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
                drawerObj[2].GetComponent<FixedJoint>().connectedBody = null;
                drawerObj[4].GetComponent<FixedJoint>().connectedBody = null;
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            switchFunction = false;

            drawerObj[0].GetComponent<FixedJoint>().connectedBody = null;
            drawerObj[2].GetComponent<FixedJoint>().connectedBody = null;
            drawerObj[4].GetComponent<FixedJoint>().connectedBody = null;

            for (int i = 0; i < drawerObj.Length; i++)
            {
                drawerObj[i].GetComponent<Renderer>().material = matOri;
            }
        }

    }

}
