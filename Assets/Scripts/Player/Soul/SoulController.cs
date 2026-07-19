using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class SoulController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float moveSpeedMultiplier = 1.4f;

    [Header("Distance")]
    [SerializeField] private float baseMaxDistance = 10f;
    [SerializeField] private float pullForce = 8f;
    [SerializeField] private float pullThresholdPercent = 0.7f;

    [Header("Exit Push")]
    [SerializeField] private float exitPushForce = 4f;
    [SerializeField] private float exitPushDuration = 0.3f;

    [Header("Auto Return")]
    [SerializeField] private float autoReturnDistance = 0.3f;
    [SerializeField] private float autoReturnCooldown = 0.8f;

    private SoulLight soulLight;

    private Rigidbody2D rb;
    private Transform bodyTransform;
    private Vector2 moveInput;
    private bool isActive = false;

    private float autoReturnCooldownTimer = 0f;
    private float exitPushTimer = 0f;
    private Vector2 exitPushDirection;
    private float distanceUpgradeBonus = 0f;

    private bool isRunning = false;
    private float CurrentMoveSpeed => isRunning ? moveSpeed * moveSpeedMultiplier : moveSpeed;
    public bool IsRunning => isRunning && moveInput.sqrMagnitude > 0.001f;

    public float MaxDistance => baseMaxDistance + distanceUpgradeBonus;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        soulLight = GetComponent<SoulLight>();

        gameObject.SetActive(false);
    }

    public void Initialize(Transform body)
    {
        bodyTransform = body;
    }

    private void OnEnable()
    {
        GameInput.Instance.OnRunStartedAction += HandleRunStarted;
        GameInput.Instance.OnRunCanceledAction += HandleRunCanceled;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnRunStartedAction -= HandleRunStarted;
        GameInput.Instance.OnRunCanceledAction -= HandleRunCanceled;
    }

    private void HandleRunStarted() => isRunning = true;
    private void HandleRunCanceled() => isRunning = false;

    public void Activate()
    {
        if (bodyTransform != null)
            transform.position = bodyTransform.position;

        isActive = true;
        autoReturnCooldownTimer = autoReturnCooldown;
        exitPushTimer = exitPushDuration;
        exitPushDirection = Vector2.up;

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        isRunning = false;
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isActive) return;

        moveInput = GameInput.Instance.GetMovementVectorNormalized();

        if (autoReturnCooldownTimer > 0f)
        {
            autoReturnCooldownTimer -= Time.deltaTime;
        }
        else
        {
            CheckAutoReturn();
        }

        if (exitPushTimer > 0f)
            exitPushTimer -= Time.deltaTime;

        UpdateLight();
    }

    private void FixedUpdate()
    {
        if (!isActive) return;

        Vector2 movement;

        if (exitPushTimer > 0f)
        {
            float t = exitPushTimer / exitPushDuration;
            movement = exitPushDirection * exitPushForce * t;
        }
        else
        {
            Vector2 pull = CalculatePullForce();
            movement = moveInput * CurrentMoveSpeed + pull;
        }

        Vector2 newPosition = rb.position + movement * Time.fixedDeltaTime;
        EnforceBoundary(ref newPosition);
        rb.MovePosition(newPosition);
    }

    private void CheckAutoReturn()
    {
        if (bodyTransform == null) return;

        float distance = Vector2.Distance(rb.position, bodyTransform.position);
        if (distance <= autoReturnDistance)
        {
            PlayerStateManager.Instance.StartReturnBody();
        }
    }

    private Vector2 CalculatePullForce()
    {
        if (bodyTransform == null) return Vector2.zero;

        Vector2 toBody = (Vector2)bodyTransform.position - rb.position;
        float distance = toBody.magnitude;
        float pullThreshold = MaxDistance * pullThresholdPercent;

        if (distance < pullThreshold) return Vector2.zero;

        float t = (distance - pullThreshold) / (MaxDistance - pullThreshold);
        t = Mathf.Clamp01(t);
        t = t * t;

        return toBody.normalized * t * pullForce;
    }

    private void EnforceBoundary(ref Vector2 newPosition)
    {
        if (bodyTransform == null) return;

        Vector2 toBody = (Vector2)bodyTransform.position - newPosition;
        if (toBody.magnitude > MaxDistance)
        {
            newPosition = (Vector2)bodyTransform.position - toBody.normalized * MaxDistance;
        }
    }

    private void UpdateLight()
    {
        soulLight.UpdateLight(GetDistanceNormalized());
    }

    public void AddDistanceUpgrade(float bonus)
    {
        distanceUpgradeBonus += bonus;
    }

    public float GetDistanceNormalized()
    {
        if (bodyTransform == null) return 0f;
        float distance = Vector2.Distance(rb.position, bodyTransform.position);
        return Mathf.Clamp01(distance / MaxDistance);
    }
}