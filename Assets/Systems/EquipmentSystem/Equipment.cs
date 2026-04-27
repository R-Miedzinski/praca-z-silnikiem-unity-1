using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour
{
    private List<Item> backpackItems = new List<Item>();
    private int maxBackpackSize = 2;
    private Dictionary<ESlotsInEquipment, Item> equippedItems = new Dictionary<ESlotsInEquipment, Item>();
    private Dictionary<EItemUsageType, ETriggerType> usageTypeToTriggerTypeMap = new Dictionary<EItemUsageType, ETriggerType>()
    {
        { EItemUsageType.Default, ETriggerType.Active1 },
        { EItemUsageType.Alternative, ETriggerType.Active2 },
        { EItemUsageType.Hold, ETriggerType.Active3 }
    };

    public void EquipItem(ESlotsInEquipment slot, Item item)
    {
        if (!equippedItems.ContainsKey(slot))
        {
            equippedItems[slot] = item;
            // item.Equip();
        }
    }

    public void UnequipItem(ESlotsInEquipment slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            Item item = equippedItems[slot];
            // item.Unequip();
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

    public void UseItem(ESlotsInEquipment slot, EItemUsageType usageType, Vector3 targettedPostion, object context = null)
    {
        if (equippedItems.TryGetValue(slot, out Item item))
        {
            if (usageTypeToTriggerTypeMap.TryGetValue(usageType, out ETriggerType triggerType))
            {
                item.Effects.TryGetValue(triggerType, out List<ItemEffect> effectsToApply);
                if (effectsToApply != null)
                {
                    foreach (var effect in effectsToApply)
                    {
                        if (!effect.CanUse())
                            continue;

                        TargettingMode targettingMode = effect.TargettingMode;
                        TargettingStrategy strategy = TargettingStrategyUtils.GetTargettingStrategy(targettingMode.TargettingType);
                        Unit[] targets = strategy.Target(targettingMode, targettedPostion, transform.position);

                        foreach (var target in targets)
                        {
                            effect.Effect.ApplyEffect(PlayerCharacter.Instance, target);
                        }
                        effect.StartCooldown();
                    }
                }
            }
            else
            {
                Debug.LogWarning($"No trigger type mapped for usage type {usageType}");
            }
        }
        else
        {
            Debug.Log($"No item equipped in {slot} to use.");
        }
    }
}
