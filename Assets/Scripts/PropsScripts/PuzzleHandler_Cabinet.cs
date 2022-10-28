using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler_Cabinet : MonoBehaviour
{
    private MeshRenderer[] renderers;
    private PuzzleHandler_PushPull[] handlers;
    private enum Drawer
    {
        Drawer1 = 1, Drawer2 = 2, Drawer3 = 3
    }
    
    private int index = 0;
    private bool inSelection;
    private static bool inRange;

    
    private Transform player;
    [SerializeField] private Transform standWaypoint;
    private bool isMoving;
    
    private void Awake()
    {
        //Cache all renderer components
        renderers = GetComponentsInChildren<MeshRenderer>();
        handlers = GetComponentsInChildren<PuzzleHandler_PushPull>();
    }

    public void SwitchDrawers()
    {
        if (!inRange) return;
        if (inSelection) return;

        if (index < 3)
            index++;
        else
            index = 1;
        
        HighlightDrawer();
    }

    public void SelectDrawers()
    {
        if (!inRange) return;
        if (index == 0) return;
        
        switch (inSelection)
        {
            case false:
                inSelection = PuzzleManager.Instance.DisableMovement(true);
                handlers[index - 1].isActive = true;
                MovePlayer();
                break;
            case true:
                inSelection = PuzzleManager.Instance.DisableMovement(false);
                handlers[index - 1].isActive = false;
                break;
        }
    }

    private void MovePlayer()
    {
        if (isMoving) return;
        Vector3 waypoint = new Vector3(standWaypoint.position.x, player.position.y, standWaypoint.position.z);
        StartCoroutine(LerpPlayer(waypoint, 1f));
    }

    private void HighlightDrawer()
    {
        foreach (var mesh in renderers)
        {
            mesh.material = mesh.transform.parent.parent.name == Enum.GetName(typeof(Drawer), index) ? PuzzleManager.Instance.ChangeMaterial() : PuzzleManager.Instance.ResetMaterial();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || playerCandleScript.restrictMode) return;

        if (!player) player = other.transform;
        
        inRange = PuzzleManager.Instance.DisableShadow(true);
        
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

        inRange = PuzzleManager.Instance.DisableShadow(false);
        
        foreach (var mesh in renderers)
            mesh.material = PuzzleManager.Instance.ResetMaterial();

        index = 0;
    }
    
    IEnumerator LerpPlayer(Vector3 targetPosition, float duration)
    {
        isMoving = true;
        float time = 0;
        
        var objToLookAt = handlers[index - 1].transform.position;
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
    }
}
