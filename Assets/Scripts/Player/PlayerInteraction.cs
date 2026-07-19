using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private float interactAngle = 90f;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private PlayerMovement playerMovement;

    private IInteractable currentInteractable;

    private void OnEnable()
    {
        GameInput.Instance.OnInteractPerformed += HandleInteract;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnInteractPerformed -= HandleInteract;
    }

    private void Update()
    {
        if (!PlayerStateManager.Instance.IsInBody) return;
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            interactionPoint.position,
            interactRange,
            interactableLayer
        );

        currentInteractable = null;
        float closestAngle = interactAngle / 2f;

        foreach (Collider2D hit in hits)
        {
            Vector2 directionToObject = (hit.transform.position - interactionPoint.position).normalized;
            float angle = Vector2.Angle(playerMovement.LastMovementDirection, directionToObject);

            if (angle < closestAngle)
            {
                closestAngle = angle;
                currentInteractable = hit.GetComponent<IInteractable>();
            }
        }
    }

    private void HandleInteract()
    {
        if (!PlayerStateManager.Instance.IsInBody) return;
        currentInteractable?.Interact();
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint.position, interactRange);

        if (Application.isPlaying && playerMovement != null)
        {
            Vector3 direction = playerMovement.LastMovementDirection;
            float halfAngle = interactAngle / 2f;

            Vector3 leftBoundary = Quaternion.Euler(0, 0, halfAngle) * direction * interactRange;
            Vector3 rightBoundary = Quaternion.Euler(0, 0, -halfAngle) * direction * interactRange;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(interactionPoint.position, interactionPoint.position + leftBoundary);
            Gizmos.DrawLine(interactionPoint.position, interactionPoint.position + rightBoundary);
        }
    }
}