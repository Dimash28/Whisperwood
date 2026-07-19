using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event Action OnLeaveBodyPerformed;
    public event Action OnInteractPerformed;
    public event Action OnRunStartedAction;
    public event Action OnRunCanceledAction;
    public event Action OnInventoryPerformed;
    public event Action<int> OnHotbarSelectPerformed;

    private PlayerInputSystem playerInputSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInputSystem = new PlayerInputSystem();
        playerInputSystem.Player.Enable();
        playerInputSystem.Player.LeaveBody.performed += _ => OnLeaveBodyPerformed?.Invoke();
        playerInputSystem.Player.Run.started += _ => OnRunStartedAction?.Invoke();
        playerInputSystem.Player.Run.canceled += _ => OnRunCanceledAction?.Invoke();
        playerInputSystem.Player.Interact.performed += _ => OnInteractPerformed?.Invoke();
        playerInputSystem.Player.Inventory.performed += _ => OnInventoryPerformed?.Invoke();

        playerInputSystem.Player.Hotbar1.performed += _ => OnHotbarSelectPerformed?.Invoke(0);
        playerInputSystem.Player.Hotbar2.performed += _ => OnHotbarSelectPerformed?.Invoke(1);
        playerInputSystem.Player.Hotbar3.performed += _ => OnHotbarSelectPerformed?.Invoke(2);
        playerInputSystem.Player.Hotbar4.performed += _ => OnHotbarSelectPerformed?.Invoke(3);
        playerInputSystem.Player.Hotbar5.performed += _ => OnHotbarSelectPerformed?.Invoke(4);
        playerInputSystem.Player.Hotbar6.performed += _ => OnHotbarSelectPerformed?.Invoke(5);
    }

    private void OnDestroy()
    {
        playerInputSystem.Player.LeaveBody.performed -= _ => OnLeaveBodyPerformed?.Invoke();
        playerInputSystem.Player.Run.started -= _ => OnRunStartedAction?.Invoke();
        playerInputSystem.Player.Run.canceled -= _ => OnRunCanceledAction?.Invoke();
        playerInputSystem.Player.Interact.performed -= _ => OnInteractPerformed?.Invoke();
        playerInputSystem.Player.Inventory.performed -= _ => OnInventoryPerformed?.Invoke();
        playerInputSystem.Dispose();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputSystem.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }
}