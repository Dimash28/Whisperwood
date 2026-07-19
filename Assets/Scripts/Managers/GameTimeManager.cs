using UnityEngine;
using System;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [SerializeField] private float dayDurationSeconds = 120f;
    [SerializeField] [Range(0f, 1f)] private float startTimeNormalized = 0.25f;

    private float currentTime;
    private int currentDay = 1;
    private bool isNight = false;

    public float NormalizedTime => currentTime / dayDurationSeconds;
    public int CurrentDay => currentDay;
    public bool IsNight => isNight;
    public float CurrentHour => NormalizedTime * 24f;

    public event Action OnDayStarted;
    public event Action OnNightStarted;
    public event Action<int> OnNewDay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentTime = startTimeNormalized * dayDurationSeconds;
    }

    private void Update()
    {
        float previousTime = NormalizedTime;
        currentTime += Time.deltaTime;

        if (currentTime >= dayDurationSeconds)
        {
            currentTime -= dayDurationSeconds;
            currentDay++;
            OnNewDay?.Invoke(currentDay);
        }

        CheckDayNightTransition(previousTime, NormalizedTime);
    }

    private void CheckDayNightTransition(float previousNormalized, float currentNormalized)
    {
        float nightStart = 0.75f;
        float dayStart = 0.25f;

        if (previousNormalized < nightStart && currentNormalized >= nightStart)
        {
            isNight = true;
            OnNightStarted?.Invoke();
        }

        if (previousNormalized < dayStart && currentNormalized >= dayStart)
        {
            isNight = false;
            OnDayStarted?.Invoke();
        }
    }
}