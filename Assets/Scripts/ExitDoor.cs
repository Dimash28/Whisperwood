using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerMovement>(out _)) return;
        LocationManager.Instance.ExitToExterior(spawnPoint);
    }
}