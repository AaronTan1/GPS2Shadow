using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private bool range;

    [SerializeField] private Material matSwapColor;
    [SerializeField] private Material matOri;

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        range = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.name != "candleStand")
            {
                range = true;
            }
            else if (this.name == "candleStand" && playerCandleScript.placePosHandIndi)
            {
                range = true;
            }

            if (range)
            {
                foreach (var mesh in renderers)
                {
                    mesh.material = matSwapColor;
                }
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if(this.name == "candleStand")
        {
            if (other.CompareTag("Player") && playerCandleScript.placePosHandIndi == false)
            {
                range = false;
            }

            if (range == false)
            {
                foreach (var mesh in renderers)
                {
                    mesh.material = matOri;
                }
            }
        }
  
    }

    private void OnTriggerExit(Collider other)
    {
        range = false;

        if (playerCandleScript.restrictMode || !other.CompareTag("Player"))
        {
            foreach (var mesh in renderers)
            {
                mesh.material = matOri;
            }
        }
    }

}
