using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    [SerializeField] private HotbarCell[] cells;
    [SerializeField] private Inventory inventory;

    private int selectedIndex = 0;

    public int SelectedIndex => selectedIndex;
    public ItemDataSO SelectedItem => inventory.Slots[selectedIndex].item;

    private void OnEnable()
    {
        inventory.OnInventoryChanged += Refresh;
        GameInput.Instance.OnHotbarSelectPerformed += HandleHotbarSelect;
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= Refresh;
        GameInput.Instance.OnHotbarSelectPerformed -= HandleHotbarSelect;
    }

    private void Start()
    {
        Refresh();
        UpdateSelection();
    }

    private void HandleHotbarSelect(int index)
    {
        if (index < 0 || index >= cells.Length) return;
        selectedIndex = index;
        UpdateSelection();
    }

    private void Refresh()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            ItemDataSO item = i < inventory.Slots.Count ? inventory.Slots[i].item : null;
            cells[i].SetItem(item);
        }
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < cells.Length; i++)
            cells[i].SetSelected(i == selectedIndex);
    }
}