using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propsIndicator : MonoBehaviour
{
    [SerializeField] public Material matSwapColor;
    [SerializeField] public Material matOri;
    [SerializeField] public GameObject tempGameObj;
    [SerializeField] public GameObject propCloset;
    [SerializeField] public GameObject uiGuide;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && this.name == "Prop_tableA" && playerCandleScript.restrictMode == false) //restricts range detection after illumination
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Renderer>().material = matSwapColor;
            }
        }
        else if (other.tag == "Player" && this.name == "stationaryCandle")
        {
            for (int i = 0; i < tempGameObj.transform.childCount; i++)
            {
                tempGameObj.transform.GetChild(i).GetComponent<Renderer>().material = matSwapColor;
            }
        }
        else if (other.tag == "Player" && this.name == "Prop_closetA" && playerCandleScript.restrictMode == false)
        {
            uiGuide.SetActive(true);
            cabinetPuzzle.switchFunction = true;
            propCloset.GetComponent<Renderer>().material = matSwapColor;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(this.name == "stationaryCandle")
        {
            for (int i = 0; i < tempGameObj.transform.childCount; i++)
            {
                tempGameObj.transform.GetChild(i).GetComponent<Renderer>().material = matOri;
            }
        }
        else if(this.name == "Prop_closetA")
        {
            uiGuide.SetActive(false);
            propCloset.GetComponent<Renderer>().material = matOri;
        }
        else 
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Renderer>().material = matOri;
            }
        }

    }
}
