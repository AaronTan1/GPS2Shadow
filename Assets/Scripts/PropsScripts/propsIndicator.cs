using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class propsIndicator : MonoBehaviour
{
    [SerializeField] public Material matSwapColor;
    [SerializeField] public Material matOri;
    [SerializeField] public GameObject tempGameObj;
    [SerializeField] public GameObject propCloset;
    [SerializeField] public TMP_Text uiGuide;
    private Color aColor, vColor;
    private void Start()
    {
        aColor.a = 0.0f;
        vColor.a = 1.0f;

        if(this.name == "Prop_closetA")
        {
            uiGuide = uiGuide.GetComponent<TMP_Text>();
        }
        uiGuide.color = aColor;
    }
    
    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player") && this.name == "Prop_closetA" && playerCandleScript.restrictMode == false)
       {
           uiGuide.color = vColor;
       }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && this.name == "Prop_tableA" && playerCandleScript.restrictMode == false) //restricts range detection after illumination
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Renderer>().material = matSwapColor;
            }
        }
        else if (other.CompareTag("Player") && this.name == "stationaryCandle")
        {
            for (int i = 0; i < tempGameObj.transform.childCount; i++)
            {
                tempGameObj.transform.GetChild(i).GetComponent<Renderer>().material = matSwapColor;
            }
        }
        else if (other.CompareTag("Player") && this.name == "Prop_closetA" && playerCandleScript.restrictMode == false)
        {
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
            uiGuide.color = aColor;
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
