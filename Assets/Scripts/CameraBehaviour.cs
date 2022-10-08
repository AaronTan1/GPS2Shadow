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
        if (state == "2D")
        {
            state = "3D";
            cm.m_Follow = alice3D;
            cm.LookAt = alice3D;
            cm.m_Lens.FieldOfView = 80;
        }else if (state == "3D")
        {
            state = "2D";
            cm.m_Follow = alice2D;
            cm.LookAt = alice2D;
            cm.m_Lens.FieldOfView = 30;
        }
    }
}
