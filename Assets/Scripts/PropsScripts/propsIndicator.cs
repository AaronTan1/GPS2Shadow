using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propsIndicator : MonoBehaviour
{
    [SerializeField] public Material matSwapColor;
    [SerializeField] public Material matOri;
    [SerializeField] public GameObject tempGameObj;

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
                Debug.Log("ONE");
                tempGameObj.transform.GetChild(i).GetComponent<Renderer>().material = matSwapColor;
            }

        }
        if (other.tag == "Player" && this.name != "stationaryCandle" && playerCandleScript.restrictMode == false)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Debug.Log("TWO");
                this.transform.GetChild(i).GetComponent<Renderer>().material = matSwapColor;
            }
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
        else 
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Renderer>().material = matOri;
            }
        }

    }
}
