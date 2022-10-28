using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler_Table : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private PuzzleHandler_PushPull handler;
    
    private Transform player;

    private void Awake()
    {
        //Cache all renderer components
        renderers = GetComponentsInChildren<MeshRenderer>();
        handler = GetComponent<PuzzleHandler_PushPull>();

        handler.isVertical = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || playerCandleScript.restrictMode) return;

        if (!player) player = other.transform;
        
        foreach (var mesh in renderers)
            mesh.material = PuzzleManager.Instance.ChangeMaterial();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || playerCandleScript.restrictMode) return;

        foreach (var mesh in renderers)
            mesh.material = PuzzleManager.Instance.ResetMaterial();
    }
    
}
