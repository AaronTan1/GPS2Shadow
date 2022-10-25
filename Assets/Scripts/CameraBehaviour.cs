using System;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraBehaviour : MonoBehaviour
{
    private CinemachineVirtualCamera cm;
    private CinemachineConfiner cmConfiner;
    private CinemachineTrackedDolly dolly;
    
    private string state = "3D";
    [SerializeField] private Transform alice3D;
    [SerializeField] private Transform alice2D;
    [SerializeField] private Button button;
    
    private void Awake()
    {
        cm = GetComponent<CinemachineVirtualCamera>();
        cmConfiner = GetComponent<CinemachineConfiner>();
        dolly = cm.GetCinemachineComponent<CinemachineTrackedDolly>();
        button.onClick.AddListener(SwapTarget);
    }

    public void SwapTarget()
    {
        if(playerCandleScript.restrictMode) return;
        if(PuzzleHandler_Cabinet.inRange) return;

        switch (state)
        {
            case "2D":
                state = "3D";
                StartCoroutine(LerpFov(80));
                cm.m_Follow = alice3D;
                cm.LookAt = alice3D;
                break;
            case "3D":
                state = "2D";
                StartCoroutine(LerpFov(30));
                cm.m_Follow = alice2D;
                cm.LookAt = alice2D;
                break;
        }
    }

    public void SwapRooms(string roomID)
    {
        GameObject roomCam = GameObject.Find(roomID);
        dolly.m_Path = roomCam.GetComponentInChildren<CinemachineSmoothPath>();
        cmConfiner.m_BoundingVolume = roomCam.GetComponentInChildren<BoxCollider>();
    }
    
    IEnumerator LerpFov(float target)
    {
        float t = 0f;
        float initial = cm.m_Lens.FieldOfView;
        while (t < 1f)
        {
            cm.m_Lens.FieldOfView = Mathf.Lerp(initial, target, t);
            t += Time.deltaTime;
            yield return null;
        }
        cm.m_Lens.FieldOfView = target;
    }

    private void Update()
    {

    }
}
