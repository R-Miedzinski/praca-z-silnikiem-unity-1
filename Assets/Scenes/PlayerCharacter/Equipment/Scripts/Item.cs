using UnityEngine;
using System.Collections.Generic;
using System;

public class ItemOld : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public Dictionary<EItemUsageType, List<ItemEffectOld>> effects = new Dictionary<EItemUsageType, List<ItemEffectOld>>();

    public ItemOld(int id, string itemName, string description, ItemEffectOld[] effects)
    {
        this.id = id;
        this.itemName = itemName;
        this.description = description;
        Array.ForEach(effects, new Action<ItemEffectOld>(effect =>
        {
            if (!this.effects.ContainsKey(effect.type))
            {
                this.effects[effect.type] = new List<ItemEffectOld> { effect };
            }
            else
            {
                this.effects[effect.type].Add(effect);
            }
        }));
    }

    public void Equip()
    {
        Debug.Log($"Equipping item: {itemName}");

        if (!effects.ContainsKey(EItemUsageType.Passive))
            return;

        foreach (var itemEffect in effects[EItemUsageType.Passive])
        {
            Debug.Log($"Subscribing to passive trigger: {itemEffect.passiveTrigger} for item: {itemName}");
            switch (itemEffect.passiveTrigger)
            {
                case EPassiveTrigger.OnMove:
                    ItemTriggerEventSystem.Instance.MoveTriggerEvent += itemEffect.ActivateEffect;
                    break;
                case EPassiveTrigger.OnHit:
                    ItemTriggerEventSystem.Instance.HitTriggerEvent += itemEffect.ActivateEffect;
                    break;
                case EPassiveTrigger.OnDamageTaken:
                    ItemTriggerEventSystem.Instance.DamageTakenTriggerEvent += itemEffect.ActivateEffect;
                    break;
                case EPassiveTrigger.OnRoomEnter:
                    ItemTriggerEventSystem.Instance.RoomEnterTriggerEvent += itemEffect.ActivateEffect;
                    break;
            }
        }
    }

    public void Unequip()
    {
        if (!effects.ContainsKey(EItemUsageType.Passive))
            return;

        foreach (var itemEffect in effects[EItemUsageType.Passive])
        {
            switch (itemEffect.passiveTrigger)
            {
                case EPassiveTrigger.OnMove:
                    ItemTriggerEventSystem.Instance.MoveTriggerEvent -= itemEffect.ActivateEffect;
                    break;
                case EPassiveTrigger.OnHit:
                    ItemTriggerEventSystem.Instance.HitTriggerEvent -= itemEffect.ActivateEffect;
                    break;
                case EPassiveTrigger.OnDamageTaken:
                    ItemTriggerEventSystem.Instance.DamageTakenTriggerEvent -= itemEffect.ActivateEffect;
                    break;
                case EPassiveTrigger.OnRoomEnter:
                    ItemTriggerEventSystem.Instance.RoomEnterTriggerEvent -= itemEffect.ActivateEffect;
                    break;
            }
        }

    }
}
