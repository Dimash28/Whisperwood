using UnityEngine;
using System;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance { get; private set; }

    [SerializeField] private float soulExitDelay = 1f;
    [SerializeField] private float soulReturnDelay = 1f;

    private PlayerControlState currentState = PlayerControlState.Body;

    public PlayerControlState CurrentState => currentState;
    public bool IsInBody => currentState == PlayerControlState.Body;
    public bool IsInSoul => currentState == PlayerControlState.Soul;
    public bool IsInVessel => currentState == PlayerControlState.Vessel;

    public event Action<PlayerControlState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void OnEnable()
    {
        GameInput.Instance.OnLeaveBodyPerformed += HandleLeaveBody;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnLeaveBodyPerformed -= HandleLeaveBody;
    }

    private void HandleLeaveBody()
    {
        if (currentState == PlayerControlState.Body)
        {
            StartCoroutine(ExitBodyRoutine());
        }
        else if (currentState == PlayerControlState.Soul)
        {
            StartCoroutine(ReturnBodyRoutine());
        }
    }

    private System.Collections.IEnumerator ExitBodyRoutine()
    {
        SetState(PlayerControlState.Exiting);
        yield return new WaitForSeconds(soulExitDelay);
        SetState(PlayerControlState.Soul);
    }

    private System.Collections.IEnumerator ReturnBodyRoutine()
    {
        SetState(PlayerControlState.Returning);
        yield return new WaitForSeconds(soulReturnDelay);
        SetState(PlayerControlState.Body);
    }

    public void SetState(PlayerControlState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        OnStateChanged?.Invoke(currentState);
    }

    public void StartReturnBody()
    {
        StartCoroutine(ReturnBodyRoutine());
    }
}