using UnityEngine;
using Cinemachine;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance { get; private set; }

    [SerializeField] private GameObject exterior;
    [SerializeField] private GameObject interiors;
    [SerializeField] private Transform player;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void EnterInterior(Transform spawnPoint)
    {
        player.position = spawnPoint.position;
        exterior.SetActive(false);
        interiors.SetActive(true);
        SnapCamera();
    }

    public void ExitToExterior(Transform spawnPoint)
    {
        player.position = spawnPoint.position;
        exterior.SetActive(true);
        interiors.SetActive(false);
        SnapCamera();
    }

    private void SnapCamera()
    {
        virtualCamera.OnTargetObjectWarped(player, player.position - virtualCamera.transform.position);
    }
}