using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler_Cabinet : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private enum Drawer
    {
        Drawer1 = 1, Drawer2 = 2, Drawer3 = 3
    }
    
    private int index = (int)Drawer.Drawer1;
    private bool inRange;
    
    private void Awake()
    {
        //Cache all renderer components
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void SwitchDrawers()
    {
        if (!inRange) return;
    }

    private void SelectDrawers()
    {
        if (!inRange) return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || playerCandleScript.restrictMode) return;
        
        inRange = true;
        
        foreach (var mesh in renderers)
            mesh.material = PuzzleManager.Instance.ChangeMaterial();
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerCandleScript.restrictMode) return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || playerCandleScript.restrictMode) return;

        inRange = false;
        
        foreach (var mesh in renderers)
            mesh.material = PuzzleManager.Instance.ResetMaterial();
    }
}
