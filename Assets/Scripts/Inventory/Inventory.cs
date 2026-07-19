using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int slotCount = 12;

    private List<InventorySlot> slots = new List<InventorySlot>();

    public int SlotCount => slotCount;
    public IReadOnlyList<InventorySlot> Slots => slots;

    public event Action OnInventoryChanged;

    private void Awake()
    {
        for (int i = 0; i < slotCount; i++)
            slots.Add(new InventorySlot());
    }

    public bool TryAddItem(ItemDataSO item, int amount = 1)
    {
        if (item.isStackable)
        {
            if (TryAddToExistingStack(item, amount))
                return true;
        }

        return TryAddToEmptySlot(item, amount);
    }

    private bool TryAddToExistingStack(ItemDataSO item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item && !slot.IsFullStack)
            {
                int spaceInStack = item.maxStackSize - slot.quantity;
                int amountToAdd = Mathf.Min(amount, spaceInStack);
                slot.AddQuantity(amountToAdd);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    private bool TryAddToEmptySlot(ItemDataSO item, int amount)
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.Set(item, amount);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public bool TryRemoveItem(ItemDataSO item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                if (slot.quantity < amount) return false;

                slot.AddQuantity(-amount);
                if (slot.quantity <= 0)
                    slot.Clear();

                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public bool HasItem(ItemDataSO item, int amount = 1)
    {
        int total = 0;
        foreach (var slot in slots)
        {
            if (slot.item == item)
                total += slot.quantity;
        }
        return total >= amount;
    }

    public bool IsFull()
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty) return false;
        }
        return true;
    }
}