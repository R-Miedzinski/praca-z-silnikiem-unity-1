using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour
{
    private List<Item> backpackItems = new List<Item>();
    private int maxBackpackSize = 2;
    private Dictionary<EEquipmentSlot, Item> equippedItems = new Dictionary<EEquipmentSlot, Item>();
    // Dictionary of: effect trigger -> list of (item name, effect)
    private Dictionary<EPassiveTrigger, List<(string, ItemEffect)>> passiveEffects = new Dictionary<EPassiveTrigger, List<(string, ItemEffect)>>();

    public void EquipItem(EEquipmentSlot slot, Item item)
    {
        if (!equippedItems.ContainsKey(slot))
        {
            equippedItems[slot] = item;
            ApplyItemEffects(item);
        }
    }

    public void UnequipItem(EEquipmentSlot slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            Item item = equippedItems[slot];
            RemoveItemEffects(item);
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

    public void UseItem(EEquipmentSlot slot, EItemUsageType usageType)
    {
        if (equippedItems.ContainsKey(slot))
        {
            Item item = equippedItems[slot];
            if (item.effects.ContainsKey(usageType))
            {
                foreach (ItemEffect effect in item.effects[usageType])
                {
                    effect.ActivateEffect(null); // Pass context if needed
                }
            } else {
                Debug.Log($"Item {item.itemName} does not have effects for usage type {usageType}");
            }
        } else {
            Debug.Log($"No item equipped in {slot} to use.");
        }
    }

    // TODO: rework to work on events. Add context.
    public void TriggerPassiveEffects(EPassiveTrigger trigger)
    {
        if (passiveEffects.ContainsKey(trigger))
        {
            foreach (var (itemName, effect) in passiveEffects[trigger])
            {
                effect.ActivateEffect(null); // Pass context if needed
            }
        }
    }

    private void ApplyItemEffects(Item item)
    {
        if (!item.effects.ContainsKey(EItemUsageType.Passive))
            return;

        foreach (ItemEffect effect in item.effects[EItemUsageType.Passive])
        {
            if (effect.passiveTrigger.HasValue)
            {
                if (!passiveEffects.ContainsKey(effect.passiveTrigger.Value))
                {
                    passiveEffects[effect.passiveTrigger.Value] = new List<(string, ItemEffect)>();
                }
                passiveEffects[effect.passiveTrigger.Value].Add((item.itemName, effect));
            }
        }
    }

    private void RemoveItemEffects(Item item)
    {
        if (!item.effects.ContainsKey(EItemUsageType.Passive))
            return;

        foreach (ItemEffect effect in item.effects[EItemUsageType.Passive])            
        {
            if (effect.passiveTrigger.HasValue)
                {
                    passiveEffects[effect.passiveTrigger.Value].RemoveAll(e => e.Item1 == item.itemName && e.Item2 == effect);
                    if (passiveEffects[effect.passiveTrigger.Value].Count == 0)
                    {
                        passiveEffects.Remove(effect.passiveTrigger.Value);
                    }
                }
        }
    }
}
