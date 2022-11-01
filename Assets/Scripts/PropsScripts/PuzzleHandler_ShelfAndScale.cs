using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler_ShelfAndScale : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private PuzzleHandler_PushPull handler;
    
    private Transform player;
    private Transform playerParent;

    private bool inRange;
    private bool inSelection;
    private bool isMoving;
    [SerializeField] private Transform leftWaypoint;
    [SerializeField] private Transform rightWaypoint;
    [SerializeField] private Transform lookAtWaypoint;

    private void Awake()
    {
        //Cache all renderer components
        renderers = GetComponentsInChildren<MeshRenderer>();
        handler = GetComponent<PuzzleHandler_PushPull>();
    }

    public void MoveObject()
    {
        if (!inRange) return;
        
        switch (inSelection)
        {
            case false:
                inSelection = PuzzleManager.Instance.DisableMovement(true);
                PuzzleManager.Instance.DisableShadow(true);
                handler.isActive = true;
                MovePlayer();
                break;
            case true:
                inSelection = PuzzleManager.Instance.DisableMovement(false);
                PuzzleManager.Instance.DisableShadow(false);
                handler.isActive = false;
                player.parent = playerParent;
                break;
        }
    }
    
    private void MovePlayer()
    {
        if (isMoving) return;

        var position = player.position;
        var left = Vector3.Distance(position, leftWaypoint.position);
        var right = Vector3.Distance(position, rightWaypoint.position);
        Transform standWaypoint = null;
        if (left < right) standWaypoint = leftWaypoint;
        else if (right < left) standWaypoint = rightWaypoint;
        
        var waypoint = new Vector3(standWaypoint.position.x, position.y, standWaypoint.position.z);
        StartCoroutine(LerpPlayer(waypoint, 0.5f));
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
        
        var objToLookAt = lookAtWaypoint.position;
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
