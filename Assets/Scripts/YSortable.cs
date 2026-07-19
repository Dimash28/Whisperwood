using UnityEngine;

public class YSortable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform sortPoint;
    [SerializeField] private bool updateEveryFrame = false;

    private const int SortingPrecision = 1000;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortPoint = transform.Find("SortPoint") ?? transform;
    }

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError($"No SpriteRenderer found on {name}");
            enabled = false;
            return;
        }

        if (sortPoint == null)
        {
            Debug.LogError($"SortPoint is missing on {name}");
            enabled = false;
            return;
        }

        SetSortingOrder();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying && spriteRenderer != null && sortPoint != null)
        {
            SetSortingOrder();
        }
    }

    private void LateUpdate()
    {
        if (!updateEveryFrame)
            return;

        SetSortingOrder();
    }

    private void SetSortingOrder()
    {
        spriteRenderer.sortingOrder = -Mathf.FloorToInt(sortPoint.position.y * SortingPrecision);
    }
}
