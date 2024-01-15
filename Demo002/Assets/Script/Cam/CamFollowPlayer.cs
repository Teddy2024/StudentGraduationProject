using UnityEngine;
using Cinemachine;

public class CamFollowPlayer : MonoBehaviour
{
    CinemachineVirtualCamera vcam;

    private void Start() 
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = GameManager.Instance.GetPlayer().transform;
    }
}
