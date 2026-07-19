using System;

[Serializable]
public class InventorySlot
{
    public ItemDataSO item;
    public int quantity;

    public bool IsEmpty => item == null;
    public bool IsStackable => item != null && item.isStackable;
    public bool IsFullStack => item != null && quantity >= item.maxStackSize;

    public void Set(ItemDataSO newItem, int amount = 1)
    {
        item = newItem;
        quantity = amount;
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }
}