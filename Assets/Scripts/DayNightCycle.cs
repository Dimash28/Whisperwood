using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [Header("Lighting")]
    [SerializeField] private Light2D globalLight;

    [Header("Day Settings")]
    [SerializeField] private Color dayColor = new Color(1f, 0.98f, 0.9f);
    [SerializeField] private float dayIntensity = 1f;

    [Header("Evening Settings")]
    [SerializeField] private Color eveningColor = new Color(1f, 0.6f, 0.3f);
    [SerializeField] private float eveningIntensity = 0.8f;

    [Header("Night Settings")]
    [SerializeField] private Color nightColor = new Color(0.1f, 0.1f, 0.3f);
    [SerializeField] private float nightIntensity = 0.45f;

    [Header("Morning Settings")]
    [SerializeField] private Color morningColor = new Color(0.8f, 0.7f, 1f);
    [SerializeField] private float morningIntensity = 0.7f;

    private void Update()
    {
        UpdateLighting(GameTimeManager.Instance.NormalizedTime);
    }

    private void UpdateLighting(float t)
    {
        if (t < 0.25f)
        {
            float blend = t / 0.25f;
            globalLight.color = Color.Lerp(nightColor, morningColor, blend);
            globalLight.intensity = Mathf.Lerp(nightIntensity, morningIntensity, blend);
        }
        else if (t < 0.35f)
        {
            float blend = (t - 0.25f) / 0.1f;
            globalLight.color = Color.Lerp(morningColor, dayColor, blend);
            globalLight.intensity = Mathf.Lerp(morningIntensity, dayIntensity, blend);
        }
        else if (t < 0.7f)
        {
            globalLight.color = dayColor;
            globalLight.intensity = dayIntensity;
        }
        else if (t < 0.8f)
        {
            float blend = (t - 0.7f) / 0.1f;
            globalLight.color = Color.Lerp(dayColor, eveningColor, blend);
            globalLight.intensity = Mathf.Lerp(dayIntensity, eveningIntensity, blend);
        }
        else if (t < 0.9f)
        {
            float blend = (t - 0.8f) / 0.1f;
            globalLight.color = Color.Lerp(eveningColor, nightColor, blend);
            globalLight.intensity = Mathf.Lerp(eveningIntensity, nightIntensity, blend);
        }
        else
        {
            globalLight.color = nightColor;
            globalLight.intensity = nightIntensity;
        }
    }
}