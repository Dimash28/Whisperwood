using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Inventory inventory;
    [SerializeField] private HotbarCell[] cells;

    private void OnEnable()
    {
        GameInput.Instance.OnInventoryPerformed += Toggle;
        inventory.OnInventoryChanged += Refresh;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnInventoryPerformed -= Toggle;
        inventory.OnInventoryChanged -= Refresh;
    }

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    private void Toggle()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
            Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            int slotIndex = i + 6;
            ItemDataSO item = slotIndex < inventory.Slots.Count ? inventory.Slots[slotIndex].item : null;
            cells[i].SetItem(item);
        }
    }
}