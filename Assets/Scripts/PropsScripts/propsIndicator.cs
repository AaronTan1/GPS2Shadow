using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propsIndicator : MonoBehaviour
{
    [SerializeField] Material matSwapColor;
    [SerializeField] Material matOri;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && playerCandleScript.restrictMode == false)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Renderer>().material = matSwapColor;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerCandleScript.restrictMode == false)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                this.transform.GetChild(i).GetComponent<Renderer>().material = matOri;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
