using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeedMultiplier = 1.4f;
    [SerializeField] private SoulController soulController;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;

    public Vector2 MovementDirection => moveInput;

    public Vector2 LastMovementDirection => lastMoveDirection;

    public bool IsMoving => moveInput.sqrMagnitude > 0.001f;

    private bool isRunning = false;
    private float CurrentMoveSpeed => isRunning ? moveSpeed * runSpeedMultiplier : moveSpeed;
    public bool IsRunning => isRunning && IsMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        soulController.Initialize(transform);
    }

    private void OnEnable()
    {
        GameInput.Instance.OnRunStartedAction += HandleRunStarted;
        GameInput.Instance.OnRunCanceledAction += HandleRunCanceled;

        PlayerStateManager.Instance.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnRunStartedAction -= HandleRunStarted;
        GameInput.Instance.OnRunCanceledAction -= HandleRunCanceled;

        PlayerStateManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    private void HandleRunStarted() => isRunning = true;
    private void HandleRunCanceled() => isRunning = false;
    private void HandleStateChanged(PlayerControlState state)
    {
        isRunning = false;
    }

    private void Update()
    {
        moveInput = GameInput.Instance.GetMovementVectorNormalized();
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = rb.position + moveInput * CurrentMoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
}