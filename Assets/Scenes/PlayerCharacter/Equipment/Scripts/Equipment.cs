using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour
{
    private List<Item> backpackItems = new List<Item>();
    private int maxBackpackSize = 2;
    private Dictionary<EEquipmentSlot, Item> equippedItems = new Dictionary<EEquipmentSlot, Item>();

    public void EquipItem(EEquipmentSlot slot, Item item)
    {
        if (!equippedItems.ContainsKey(slot))
        {
            equippedItems[slot] = item;
            item.Equip();
        }
    }

    public void UnequipItem(EEquipmentSlot slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            Item item = equippedItems[slot];
            item.Unequip();
            equippedItems.Remove(slot);
        }
    }

    public void AddItemToBackpack(Item item)
    {
        if (backpackItems.Count < maxBackpackSize)
        {
            backpackItems.Add(item);
        }
    }

    public void RemoveItemFromBackpack(Item item)
    {
        backpackItems.Remove(item);
    }

    public void UseItem(EEquipmentSlot slot, EItemUsageType usageType, object context = null)
    {
        if (equippedItems.ContainsKey(slot))
        {
            Item item = equippedItems[slot];
            if (item.effects.ContainsKey(usageType))
            {
                foreach (ItemEffect effect in item.effects[usageType])
                {
                    effect.ActivateEffect(context); // Pass context if needed
                }
            }
            else
            {
                Debug.Log($"Item {item.itemName} does not have effects for usage type {usageType}");
            }
        }
        else
        {
            Debug.Log($"No item equipped in {slot} to use.");
        }
    }
}
