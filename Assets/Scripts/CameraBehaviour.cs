using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraBehaviour : MonoBehaviour
{
    private CinemachineVirtualCamera cm;
    private string state = "3D";
    [SerializeField] private Transform alice3D;
    [SerializeField] private Transform alice2D;
    [SerializeField] private Button button;
    private void Awake()
    {
        cm = GetComponent<CinemachineVirtualCamera>();
        button.onClick.AddListener(SwapTarget);
    }

    public void SwapTarget()
    {
        if (state == "2D" && playerCandleScript.restrictMode == false)
        {
            state = "3D";
            StartCoroutine(LerpFov(80));
            cm.m_Follow = alice3D;
            cm.LookAt = alice3D;
        }else if (state == "3D" && playerCandleScript.restrictMode == false && cabinetPuzzle.switchFunction == false)
        {
            state = "2D";
            StartCoroutine(LerpFov(30));
            cm.m_Follow = alice2D;
            cm.LookAt = alice2D;
        }
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
}
