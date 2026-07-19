using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SoulController soulController;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        soulController.Initialize(transform);
    }

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
        playerMovement.enabled = newState == PlayerControlState.Body;

        if (newState == PlayerControlState.Soul)
            soulController.Activate();
        else
            soulController.Deactivate();
    }
}