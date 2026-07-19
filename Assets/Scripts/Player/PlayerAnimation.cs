using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerMovement playerMovement;

    private static readonly int MoveXHash = Animator.StringToHash("XDirection");
    private static readonly int MoveYHash = Animator.StringToHash("YDirection");
    private static readonly int LastMoveXHash = Animator.StringToHash("XLastDirection");
    private static readonly int LastMoveYHash = Animator.StringToHash("YLastDirection");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int IsExitingHash = Animator.StringToHash("IsExiting");

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        Vector2 direction = playerMovement.MovementDirection;
        Vector2 lastDirection = playerMovement.LastMovementDirection;

        playerAnimator.SetFloat(MoveXHash, direction.x);
        playerAnimator.SetFloat(MoveYHash, direction.y);

        playerAnimator.SetFloat(LastMoveXHash, lastDirection.x);
        playerAnimator.SetFloat(LastMoveYHash, lastDirection.y);

        bool canMove = PlayerStateManager.Instance.CurrentState == PlayerControlState.Body;
        playerAnimator.SetBool(IsMovingHash, canMove && playerMovement.IsMoving);
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
        if (newState == PlayerControlState.Exiting || newState == PlayerControlState.Soul)
        {
            playerAnimator.SetBool(IsExitingHash, true);
        }
        else
            playerAnimator.SetBool(IsExitingHash, false);
    }
}
