using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour
{
    private List<ItemOld> backpackItems = new List<ItemOld>();
    private int maxBackpackSize = 2;
    private Dictionary<ESlotsInEquipment, ItemOld> equippedItems = new Dictionary<ESlotsInEquipment, ItemOld>();

    public void EquipItem(ESlotsInEquipment slot, ItemOld item)
    {
        if (!equippedItems.ContainsKey(slot))
        {
            equippedItems[slot] = item;
            item.Equip();
        }
    }

    public void UnequipItem(ESlotsInEquipment slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            ItemOld item = equippedItems[slot];
            item.Unequip();
            equippedItems.Remove(slot);
        }
    }

    public void AddItemToBackpack(ItemOld item)
    {
        if (backpackItems.Count < maxBackpackSize)
        {
            backpackItems.Add(item);
        }
    }

    public void RemoveItemFromBackpack(ItemOld item)
    {
        backpackItems.Remove(item);
    }

    public void UseItem(ESlotsInEquipment slot, EItemUsageType usageType, object context = null)
    {
        if (equippedItems.ContainsKey(slot))
        {
            ItemOld item = equippedItems[slot];
            if (item.effects.ContainsKey(usageType))
            {
                foreach (ItemEffectOld effect in item.effects[usageType])
                {
                    effect.ActivateEffect(context); // Pass context if needed
                }
            }
            else
            {
                Debug.Log($"ItemOld {item.itemName} does not have effects for usage type {usageType}");
            }
        }
        else
        {
            Debug.Log($"No item equipped in {slot} to use.");
        }
    }
}
