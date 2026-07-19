using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Transform soulTransform;

    private void OnEnable()
    {
        PlayerStateManager.Instance.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        PlayerStateManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(PlayerControlState newState)
    {
        if (newState == PlayerControlState.Soul)
            virtualCamera.Follow = soulTransform;
        else
            virtualCamera.Follow = bodyTransform;
    }
}