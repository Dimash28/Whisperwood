using UnityEngine;

public class EntranceDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform spawnPoint;

    public void Interact()
    {
        LocationManager.Instance.EnterInterior(spawnPoint);
    }

    public string GetInteractionPrompt() => "Press E to Enter";
}