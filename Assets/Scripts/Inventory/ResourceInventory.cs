using UnityEngine;
using System;
using System.Collections.Generic;

public class ResourceInventory : MonoBehaviour
{
    private Dictionary<ItemDataSO, int> resources = new Dictionary<ItemDataSO, int>();

    public event Action OnResourceChanged;

    public bool TryAddResource(ItemDataSO item, int amount = 1)
    {
        if (item.itemType != ItemType.Resource)
        {
            Debug.LogWarning($"{item.itemName} is not a resource");
            return false;
        }

        if (resources.ContainsKey(item))
            resources[item] += amount;
        else
            resources[item] = amount;

        OnResourceChanged?.Invoke();
        return true;
    }

    public bool TryRemoveResource(ItemDataSO item, int amount = 1)
    {
        if (!resources.ContainsKey(item) || resources[item] < amount)
            return false;

        resources[item] -= amount;

        if (resources[item] <= 0)
            resources.Remove(item);

        OnResourceChanged?.Invoke();
        return true;
    }

    public bool HasResource(ItemDataSO item, int amount = 1)
    {
        return resources.ContainsKey(item) && resources[item] >= amount;
    }

    public int GetAmount(ItemDataSO item)
    {
        return resources.ContainsKey(item) ? resources[item] : 0;
    }

    public IReadOnlyDictionary<ItemDataSO, int> GetAllResources()
    {
        return resources;
    }
}