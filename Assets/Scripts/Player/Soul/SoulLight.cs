using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SoulLight : MonoBehaviour
{
    [SerializeField] private Light2D soulLight;
    [SerializeField] private float maxIntensity = 1.2f;
    [SerializeField] private Color fullColor = new Color(0.4f, 1f, 0.4f);
    [SerializeField] private Color depletedColor = new Color(0.1f, 0.4f, 0.1f);

    public void UpdateLight(float normalizedDistance)
    {
        float normalized = 1f - normalizedDistance;
        soulLight.intensity = Mathf.Lerp(0.2f, maxIntensity, normalized);
        soulLight.color = Color.Lerp(depletedColor, fullColor, normalized);
    }
}
