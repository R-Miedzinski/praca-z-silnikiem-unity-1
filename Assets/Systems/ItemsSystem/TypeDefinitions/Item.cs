using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Item
{
  public string Id { get { return id; } }
  public string ItemName { get { return itemName; } }
  public EEquipmentSlot EquipmentSlot { get { return equipmentSlot; } }
  public Dictionary<TriggerVector, List<ItemEffect>> TriggerEffects { get { return triggerEffects; } }
  private string id;
  private string itemName;
  private EEquipmentSlot equipmentSlot;
  private Dictionary<TriggerVector, List<ItemEffect>> triggerEffects;

  public Item(string _id, string _name, EEquipmentSlot _equipmentSlot, ItemEffect[] _effects)
  {
    id = _id;
    itemName = _name;
    equipmentSlot = _equipmentSlot;
    triggerEffects = new Dictionary<TriggerVector, List<ItemEffect>>();
    foreach (var effectData in _effects)
    {
      ItemEffect newItemEffect = new ItemEffect(effectData.Effects, effectData.EffectTriggers, effectData.TargetingMode, effectData.Cooldown, effectData.Charges);

      TriggerVector triggerVector = new TriggerVector();
      foreach (ETriggerType triggerType in effectData.EffectTriggers)
      {
        triggerVector.Activate(triggerType);
      }
      if (!triggerEffects.ContainsKey(triggerVector))
      {
        triggerEffects[triggerVector] = new List<ItemEffect>();
      }
      triggerEffects[triggerVector].Add(newItemEffect);
    }
  }

  public Item(ItemData itemData)
  {
    id = itemData.Id;
    itemName = itemData.ItemName;
    equipmentSlot = itemData.EquipmentSlot;
    triggerEffects = new Dictionary<TriggerVector, List<ItemEffect>>();
    foreach (var effectData in itemData.Effects)
    {
      List<Effect> newEffects = new List<Effect>();
      for (int i = 0; i < effectData.EffectIds.Length; i++)
      {
        Effect effect = IdToEffectMap.GetEffectById(effectData.EffectIds[i], effectData.EffectParams[i]);
        effect.Id = effectData.EffectIds[i] + "_" + id + "_" + string.Join("_", effectData.EffectTriggers.Select(t => t.ToString())); // Ensure unique ID for each effect instance

        newEffects.Add(effect);
      }

      TargetingMode targetingMode = new TargetingMode(effectData.TargetingMode);
      ItemEffect newItemEffect = new ItemEffect(newEffects.ToArray(), effectData.EffectTriggers, targetingMode, effectData.Cooldown, effectData.Charges);

      TriggerVector triggerVector = new TriggerVector();
      foreach (ETriggerType triggerType in effectData.EffectTriggers)
      {
        triggerVector.Activate(triggerType);
      }
      if (!triggerEffects.ContainsKey(triggerVector))
      {
        triggerEffects[triggerVector] = new List<ItemEffect>();
      }
      triggerEffects[triggerVector].Add(newItemEffect);
    }
  }

  public Item(ItemDataObject itemDataObject)
  {
    id = itemDataObject.Id;
    itemName = itemDataObject.ItemName;
    equipmentSlot = itemDataObject.EquipmentSlot;
    triggerEffects = new Dictionary<TriggerVector, List<ItemEffect>>();
    foreach (var effectData in itemDataObject.Effects)
    {
      List<Effect> newEffects = new List<Effect>();
      foreach (EffectIdParamPair effectPair in effectData.Effects)
      {
        Effect effect = IdToEffectMap.GetEffectById(effectPair.EffectId, effectPair.EffectParams);
        effect.Id = effectPair.EffectId + "_" + id + "_" + string.Join("_", effectData.EffectTriggers.Select(t => t.ToString())); // Ensure unique ID for each effect instance

        newEffects.Add(effect);
      }

      TargetingMode targetingMode = new TargetingMode(effectData.TargetingMode);
      ItemEffect newItemEffect = new ItemEffect(newEffects.ToArray(), effectData.EffectTriggers, targetingMode, effectData.Cooldown, effectData.Charges);

      TriggerVector triggerVector = new TriggerVector();
      foreach (ETriggerType triggerType in effectData.EffectTriggers)
      {
        triggerVector.Activate(triggerType);
      }
      if (!triggerEffects.ContainsKey(triggerVector))
      {
        triggerEffects[triggerVector] = new List<ItemEffect>();
      }
      triggerEffects[triggerVector].Add(newItemEffect);
    }
  }

  public void Equip()
  {
    ItemTriggerEventSystem.Instance.ProcessTriggersEvent += ReactToItemTriggerEvent;
  }

  public void Unequip()
  {
    ItemTriggerEventSystem.Instance.ProcessTriggersEvent -= ReactToItemTriggerEvent;
  }

  public void Use(TriggerVector triggerVector, Vector3 targetedPosition)
  {
    triggerEffects.TryGetValue(triggerVector, out List<ItemEffect> effectsToApply);
    if (effectsToApply != null)
    {
      foreach (var itemEffects in effectsToApply)
      {
        if (!itemEffects.CanUse())
          continue;
        itemEffects.StartCooldown(PlayerCharacter.Instance);

        TargetingMode targetingMode = itemEffects.TargetingMode;
        TargetingStrategy strategy = TargetingStrategyUtils.GetTargetingStrategy(targetingMode.TargetingType);
        strategy.Target(targetingMode, targetedPosition, PlayerCharacter.Instance, itemEffects.Effects);
      }
    }
  }

  public void Use(TriggerVector triggerVector, Unit[] targets)
  {
    triggerEffects.TryGetValue(triggerVector, out List<ItemEffect> effectsToApply);
    if (effectsToApply != null)
    {
      foreach (var itemEffects in effectsToApply)
      {
        if (!itemEffects.CanUse())
          continue;
        itemEffects.StartCooldown(PlayerCharacter.Instance);

        foreach (var effect in itemEffects.Effects)
        {
          foreach (var target in targets)
          {
            effect.ApplyEffect(PlayerCharacter.Instance, target);
          }
        }
      }
    }
  }

  private void ReactToItemTriggerEvent(TriggerVector activatedTriggers, Dictionary<ETriggerType, ItemTriggerEventContext> triggerContexts)
  {
    foreach (var kvp in triggerEffects)
    {
      TriggerVector effectTriggerVector = kvp.Key;
      List<ItemEffect> effectsToApply = kvp.Value;

      if (!activatedTriggers.Check(effectTriggerVector))
        continue;

      if (!IsActivationValid(effectTriggerVector, triggerContexts))
        continue;

      TargetInfo targetInfo = GetTargetInfo(effectTriggerVector, triggerContexts);
      if (targetInfo.Type == Targetype.Position)
      {
        Use(effectTriggerVector, targetInfo.Position);
      }
      else
      {
        // TODO: Ensure this works, when OnHit effects are implemented
        Use(effectTriggerVector, targetInfo.Units);
      }
    }
  }

  private bool IsActivationValid(TriggerVector effectTriggerVector, Dictionary<ETriggerType, ItemTriggerEventContext> triggerContexts)
  {
    bool isValid = true;

    if (
      effectTriggerVector.HasActiveTrigger(ETriggerType.Active1) ||
      effectTriggerVector.HasActiveTrigger(ETriggerType.Active2) ||
      effectTriggerVector.HasActiveTrigger(ETriggerType.Active3) ||
      effectTriggerVector.HasActiveTrigger(ETriggerType.Active3Release))
    {
      ETriggerType activeTriggerType =
        effectTriggerVector.HasActiveTrigger(ETriggerType.Active1) ? ETriggerType.Active1 :
        effectTriggerVector.HasActiveTrigger(ETriggerType.Active2) ? ETriggerType.Active2 :
        effectTriggerVector.HasActiveTrigger(ETriggerType.Active3) ? ETriggerType.Active3 :
        ETriggerType.Active3Release;

      if (
        triggerContexts.TryGetValue(activeTriggerType, out ItemTriggerEventContext context) &&
        context != null &&
        context is ActivateItemEventContext activateContext
      )
      {
        isValid &= activateContext.ItemsActivated.Contains(Id);
      }
      else
      {
        Debug.Log($"No valid context found for trigger type {activeTriggerType}. Activation is invalid.");
        return false;
      }
    }

    if (
      effectTriggerVector.HasActiveTrigger(ETriggerType.OnSelfActive1) ||
      effectTriggerVector.HasActiveTrigger(ETriggerType.OnSelfActive2) ||
      effectTriggerVector.HasActiveTrigger(ETriggerType.OnSelfActive3))
    {
      ETriggerType selfActiveTriggerType =
        effectTriggerVector.HasActiveTrigger(ETriggerType.OnSelfActive1) ? ETriggerType.OnSelfActive1 :
        effectTriggerVector.HasActiveTrigger(ETriggerType.OnSelfActive2) ? ETriggerType.OnSelfActive2 :
        ETriggerType.OnSelfActive3;

      if (
        triggerContexts.TryGetValue(selfActiveTriggerType, out ItemTriggerEventContext context) &&
        context != null &&
        context is ActivateItemEventContext activateContext
      )
      {
        isValid &= activateContext.ItemsActivated.Contains(Id);
      }
      else
      {
        Debug.Log($"No valid context found for trigger type {selfActiveTriggerType}. Activation is invalid.");
        return false;
      }
    }

    return isValid;
  }

  private TargetInfo GetTargetInfo(TriggerVector effectTriggerVector, Dictionary<ETriggerType, ItemTriggerEventContext> triggerContexts)
  {
    if (effectTriggerVector.HasActiveTrigger(ETriggerType.OnHit))
    {


      if (triggerContexts.TryGetValue(ETriggerType.OnHit, out ItemTriggerEventContext context) && context is HitEventContext hitContext)
      {
        Unit[] units = hitContext.EnemyHit.Keys.ToArray();

        return TargetInfo.FromUnits(units);
      }
    }

    foreach (ItemTriggerEventContext context in triggerContexts.Values)
    {
      if (context != null && context.TargetedPosition != null)
      {
        return TargetInfo.FromPosition(context.TargetedPosition);
      }
    }

    // Player position as fallback
    return TargetInfo.FromPosition(PlayerCharacter.Instance.transform.position);
  }
}