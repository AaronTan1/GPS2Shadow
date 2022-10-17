using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tablePuzzle : MonoBehaviour
{
    [SerializeField] GameObject tableShadow; //shadow of the table
    [SerializeField] GameObject pullLimit; //hidden collider to restrict pull 
    private bool interact, range, disableInteract;
    

    void Start()
    {
        interact = false;
        range = false;
        disableInteract = false;
    }

    public void ToggleInteract()
    {
        if(interact == false && range && playerCandleScript.restrictMode == false) 
        {
            tableShadow.transform.localRotation = new Quaternion(0,180,0,0);
            interact = true;
        }
        else if (interact && range)
        {
            interact = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && range == false && playerCandleScript.restrictMode == false) 
        {
            range = true;
        }

        if (other.CompareTag("Player") && interact && range)
        {
            disableInteract = true;
            this.gameObject.GetComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();            

            if(this.gameObject.GetComponent<FixedJoint>().connectedBody == other.GetComponent<Rigidbody>())
            {
                pullLimit.SetActive(true);
                tableShadow.transform.localScale = new Vector3((tableShadow.transform.position.x - gameObject.transform.position.z) - 0.2f, (tableShadow.transform.position.y - gameObject.transform.position.z), 4.895162f);
            }
        }
        else if(other.CompareTag("Player") && interact == false && range && disableInteract)
        {           
            this.gameObject.GetComponent<FixedJoint>().connectedBody = null;
            disableInteract = false;
            pullLimit.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            this.gameObject.GetComponent<FixedJoint>().connectedBody = null;
            interact = false;
            range = false;
        }
    }
}
