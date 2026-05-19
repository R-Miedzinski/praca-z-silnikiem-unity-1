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
        { EItemUsageType.Hold, ETriggerType.Active3 },
        { EItemUsageType.Release, ETriggerType.Active3Release },
    };

    private Dictionary<ESlotsInEquipment, Item> activeHoldItems = new Dictionary<ESlotsInEquipment, Item>();

    private void Update()
    {
        foreach (var kvp in activeHoldItems)
        {
            ESlotsInEquipment slot = kvp.Key;
            Item item = kvp.Value;

            ItemTriggerEventSystem.Instance.SendTriggerEvent(
                ETriggerType.Active3,
                new ItemTriggerEventContext(targettedPosition: PlayerCharacter.Instance.TargetingWidget.transform.position, itemActivated: item.Id)
            );
        }
    }

    public void EquipItem(ESlotsInEquipment slot, Item item)
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

    public void UseItem(ESlotsInEquipment slot, EItemUsageType usageType, Vector3 targettedPostion)
    {
        if (equippedItems.TryGetValue(slot, out Item item))
        {
            if (usageTypeToTriggerTypeMap.TryGetValue(usageType, out ETriggerType triggerType))
            {
                if (triggerType == ETriggerType.Active1 || triggerType == ETriggerType.Active2)
                {
                    ItemTriggerEventSystem.Instance.SendTriggerEvent(triggerType, new ItemTriggerEventContext(targettedPosition: targettedPostion, itemActivated: item.Id));
                }
                else if (triggerType == ETriggerType.Active3)
                {
                    StartHoldItem(slot);
                }
                else if (triggerType == ETriggerType.Active3Release)
                {
                    ReleaseHoldItem(slot);
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

    private void StartHoldItem(ESlotsInEquipment slot)
    {
        if (equippedItems.TryGetValue(slot, out Item item))
        {
            activeHoldItems[slot] = item;
        }
    }

    private void ReleaseHoldItem(ESlotsInEquipment slot)
    {
        if (equippedItems.TryGetValue(slot, out Item item))
        {
            activeHoldItems.Remove(slot);

            ItemTriggerEventSystem.Instance.SendTriggerEvent(
                ETriggerType.Active3Release,
                new ItemTriggerEventContext(targettedPosition: PlayerCharacter.Instance.TargetingWidget.transform.position, itemActivated: item.Id)
            );
        }
    }
}
