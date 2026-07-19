using UnityEngine;
using System;

public class EnergySystem : MonoBehaviour
{
    public static EnergySystem Instance { get; private set; }

    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float startEnergy = 100f;

    private float currentEnergy;

    public float CurrentEnergy => currentEnergy;
    public float MaxEnergy => maxEnergy;
    public float NormalizedEnergy => currentEnergy / maxEnergy;
    public bool IsExhausted => currentEnergy <= 0f;

    public event Action<float> OnEnergyChanged;
    public event Action OnExhausted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentEnergy = startEnergy;
    }

    private void OnEnable()
    {
        GameTimeManager.Instance.OnNewDay += HandleNewDay;
    }

    private void OnDisable()
    {
        GameTimeManager.Instance.OnNewDay -= HandleNewDay;
    }

    public bool TrySpendEnergy(float amount)
    {
        if (currentEnergy < amount)
            return false;

        currentEnergy = Mathf.Max(0f, currentEnergy - amount);
        OnEnergyChanged?.Invoke(currentEnergy);

        if (IsExhausted)
            OnExhausted?.Invoke();

        return true;
    }

    public void RestoreEnergy(float amount)
    {
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + amount);
        OnEnergyChanged?.Invoke(currentEnergy);
    }

    private void HandleNewDay(int day)
    {
        currentEnergy = maxEnergy;
        OnEnergyChanged?.Invoke(currentEnergy);
    }
}