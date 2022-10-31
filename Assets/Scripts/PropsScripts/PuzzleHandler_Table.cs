using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler_Table : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private PuzzleHandler_PushPull handler;
    
    private Transform player;
    private Transform playerParent;

    private bool inRange;
    private bool inSelection;
    private bool isMoving;
    [SerializeField] private Transform standWaypoint;

    private void Awake()
    {
        //Cache all renderer components
        renderers = GetComponentsInChildren<MeshRenderer>();
        handler = GetComponentInChildren<PuzzleHandler_PushPull>();

        handler.isVertical = true;
    }

    public void MoveTable()
    {
        if (!inRange) return;
        
        switch (inSelection)
        {
            case false:
                inSelection = PuzzleManager.Instance.DisableMovement(true);
                handler.isActive = true;
                MovePlayer();
                break;
            case true:
                inSelection = PuzzleManager.Instance.DisableMovement(false);
                handler.isActive = false;
                player.parent = playerParent;
                break;
        }
    }
    
    private void MovePlayer()
    {
        if (isMoving) return;
        Vector3 waypoint = new Vector3(standWaypoint.position.x, player.position.y, standWaypoint.position.z);
        StartCoroutine(LerpPlayer(waypoint, 1f));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || playerCandleScript.restrictMode) return;

        if (!player)
        {
            player = other.transform;
            playerParent = player.parent;
        }
        
        inRange = true;
        
        foreach (var mesh in renderers)
            mesh.material = PuzzleManager.Instance.ChangeMaterial();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || playerCandleScript.restrictMode) return;
        
        inRange = false;
        
        foreach (var mesh in renderers)
            mesh.material = PuzzleManager.Instance.ResetMaterial();
    }
    
    IEnumerator LerpPlayer(Vector3 targetPosition, float duration)
    {
        isMoving = true;
        float time = 0;
        
        var objToLookAt = transform.position;
        var playerPos = player.position;

        while (time < duration)
        {
            player.LookAt(new Vector3(objToLookAt.x, playerPos.y, objToLookAt.z));
            player.position = Vector3.Lerp(playerPos, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        player.position = targetPosition;
        isMoving = false;
        player.parent = handler.transform;
    }
}
