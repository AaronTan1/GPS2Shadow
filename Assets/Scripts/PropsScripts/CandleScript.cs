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
        if(other.CompareTag("Player"))
        {
            range = true;
        }

        if (other.CompareTag("Player") && range)
        {
            foreach (var mesh in renderers)
            {
                mesh.material = matSwapColor;
            }               
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerCandleScript.restrictMode || !other.CompareTag("Player"))
        {
            foreach (var mesh in renderers)
            {
                mesh.material = matOri;
            }
                
        }
    }

}
