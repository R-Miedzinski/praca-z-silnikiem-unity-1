using UnityEngine;
using System.Collections.Generic;

public class Equipment : MonoBehaviour
{
    [SerializeField]
    private int maxBackpackSize = 2;
    [SerializeField]
    private float swapLoadoutCooldown = 1f;
    private float lastSwapLoadoutTime = -Mathf.Infinity;
    private readonly List<Item> backpackItems = new List<Item>();
    private readonly Dictionary<ESlotsInEquipment, Item> equippedItems = new();
    private readonly Dictionary<ESlotsInEquipment, Item> inactiveLoadout = new()
    {
        { ESlotsInEquipment.LeftHand, null },
        { ESlotsInEquipment.RightHand, null },
    };
    private readonly Dictionary<EItemUsageType, ETriggerType> usageTypeToTriggerTypeMap = new()
    {
        { EItemUsageType.Default, ETriggerType.Active1 },
        { EItemUsageType.Alternative, ETriggerType.Active2 },
        { EItemUsageType.Hold, ETriggerType.Active3 },
        { EItemUsageType.Release, ETriggerType.Active3Release },
    };

    private readonly Dictionary<ESlotsInEquipment, Item> activeHoldItems = new();

    private void Update()
    {
        foreach (var kvp in activeHoldItems)
        {
            ESlotsInEquipment slot = kvp.Key;
            Item item = kvp.Value;

            ItemTriggerEventSystem.Instance.SendTriggerEvent(
                ETriggerType.Active3,
                new ActivateItemEventContext(targetedPosition: PlayerCharacter.Instance.TargetingWidget.transform.position, itemActivated: item.Id)
            );
            ItemTriggerEventSystem.Instance.SendTriggerEvent(
                ETriggerType.OnSelfActive3,
                new ActivateItemEventContext(targetedPosition: PlayerCharacter.Instance.TargetingWidget.transform.position, itemActivated: item.Id)
            );
        }
    }

    public void EquipItem(ESlotsInEquipment slot, Item item)
    {
        if (item == null)
        {
            Debug.LogWarning($"Cannot equip item in {slot} because item is null.");
            return;
        }

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

    public void SwapLoadout()
    {
        if (Time.time - lastSwapLoadoutTime < swapLoadoutCooldown)
        {
            Debug.Log("Swap loadout is on cooldown.");
            return;
        }
        lastSwapLoadoutTime = Time.time;

        var slotsToSwap = new List<ESlotsInEquipment>(inactiveLoadout.Keys);
        foreach (var slot in slotsToSwap)
        {
            Item itemInInactiveLoadout = inactiveLoadout[slot];

            if (equippedItems.TryGetValue(slot, out Item currentlyEquippedItem))
            {
                currentlyEquippedItem.Unequip();
                inactiveLoadout[slot] = currentlyEquippedItem;
            }
            else
            {
                inactiveLoadout[slot] = null;
            }

            if (itemInInactiveLoadout != null)
            {
                itemInInactiveLoadout.Equip();
                equippedItems[slot] = itemInInactiveLoadout;
            }
            else
            {
                equippedItems.Remove(slot);
            }
        }
    }

    public void UseItem(ESlotsInEquipment slot, EItemUsageType usageType, Vector3 targetedPostion)
    {
        if (equippedItems.TryGetValue(slot, out Item item))
        {
            if (usageTypeToTriggerTypeMap.TryGetValue(usageType, out ETriggerType triggerType))
            {
                switch (triggerType)
                {
                    case ETriggerType.Active1:
                        ItemTriggerEventSystem.Instance.SendTriggerEvent(triggerType, new ActivateItemEventContext(targetedPosition: targetedPostion, itemActivated: item.Id));
                        ItemTriggerEventSystem.Instance.SendTriggerEvent(ETriggerType.OnSelfActive1, new ActivateItemEventContext(targetedPosition: targetedPostion, itemActivated: item.Id));
                        break;
                    case ETriggerType.Active2:
                        ItemTriggerEventSystem.Instance.SendTriggerEvent(triggerType, new ActivateItemEventContext(targetedPosition: targetedPostion, itemActivated: item.Id));
                        ItemTriggerEventSystem.Instance.SendTriggerEvent(ETriggerType.OnSelfActive2, new ActivateItemEventContext(targetedPosition: targetedPostion, itemActivated: item.Id));
                        break;
                    case ETriggerType.Active3:
                        StartHoldItem(slot);
                        break;
                    case ETriggerType.Active3Release:
                        ReleaseHoldItem(slot);
                        break;
                    default:
                        Debug.LogWarning($"Unhandled trigger type {triggerType} for usage type {usageType}");
                        break;
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
                new ActivateItemEventContext(targetedPosition: PlayerCharacter.Instance.TargetingWidget.transform.position, itemActivated: item.Id)
            );
        }
    }
}
