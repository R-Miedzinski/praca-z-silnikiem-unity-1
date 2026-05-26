using UnityEngine;
using System.Collections.Generic;

public class Item
{
  public string Id { get { return id; } }
  public string ItemName { get { return itemName; } }
  public EEquipmentSlot EquipmentSlot { get { return equipmentSlot; } }
  public Dictionary<ETriggerType, List<ItemEffect>> Effects { get { return effects; } }
  private string id;
  private string itemName;
  private EEquipmentSlot equipmentSlot;
  private Dictionary<ETriggerType, List<ItemEffect>> effects;

  public Item(string _id, string _name, EEquipmentSlot _equipmentSlot, ItemEffect[] _effects)
  {
    id = _id;
    itemName = _name;
    equipmentSlot = _equipmentSlot;
    effects = new Dictionary<ETriggerType, List<ItemEffect>>();
    foreach (var effectData in _effects)
    {
      if (!effects.ContainsKey(effectData.TriggerType))
      {
        effects[effectData.TriggerType] = new List<ItemEffect>();
      }
      effects[effectData.TriggerType].Add(new ItemEffect(effectData.Effects, effectData.TriggerType, effectData.Cooldown, effectData.TargetingMode));
    }
  }

  public Item(ItemData itemData)
  {
    id = itemData.Id;
    itemName = itemData.ItemName;
    equipmentSlot = itemData.EquipmentSlot;
    effects = new Dictionary<ETriggerType, List<ItemEffect>>();
    foreach (var effectData in itemData.Effects)
    {
      List<Effect> newEffects = new List<Effect>();
      for (int i = 0; i < effectData.EffectIds.Length; i++)
      {
        newEffects.Add(IdToEffectMap.GetEffectById(effectData.EffectIds[i], effectData.EffectParams[i]));
      }

      TargetingMode targetingMode = new TargetingMode(effectData.TargetingMode);
      if (!effects.ContainsKey(effectData.TriggerType))
      {
        effects[effectData.TriggerType] = new List<ItemEffect>();
      }
      effects[effectData.TriggerType].Add(new ItemEffect(newEffects.ToArray(), effectData.TriggerType, effectData.Cooldown, targetingMode));
    }
  }

  public Item(ItemDataObject itemDataObject)
  {
    id = itemDataObject.Id;
    itemName = itemDataObject.ItemName;
    equipmentSlot = itemDataObject.EquipmentSlot;
    effects = new Dictionary<ETriggerType, List<ItemEffect>>();
    foreach (var effectData in itemDataObject.Effects)
    {
      List<Effect> newEffects = new List<Effect>();
      foreach (EffectIdParamPair effectPair in effectData.Effects)
      {
        newEffects.Add(IdToEffectMap.GetEffectById(effectPair.EffectId, effectPair.EffectParams));
      }

      TargetingMode targetingMode = new TargetingMode(effectData.TargetingMode);
      if (!effects.ContainsKey(effectData.TriggerType))
      {
        effects[effectData.TriggerType] = new List<ItemEffect>();
      }
      effects[effectData.TriggerType].Add(new ItemEffect(newEffects.ToArray(), effectData.TriggerType, effectData.Cooldown, targetingMode));
    }
  }

  public void Equip()
  {
    var presentEffectTriggers = new List<ETriggerType>(effects.Keys);
    foreach (var triggerType in presentEffectTriggers)
    {
      ConnectEffectTrigger(triggerType);
    }
  }

  public void Unequip()
  {
    var presentEffectTriggers = new List<ETriggerType>(effects.Keys);
    foreach (var triggerType in presentEffectTriggers)
    {
      DisconnectEffectTrigger(triggerType);
    }
  }

  public void Use(ETriggerType triggerType, Vector3 targettedPosition, ItemTriggerEventContext context = null)
  {
    effects.TryGetValue(triggerType, out List<ItemEffect> effectsToApply);
    if (effectsToApply != null)
    {
      foreach (var itemEffects in effectsToApply)
      {
        if (!itemEffects.CanUse())
          continue;
        itemEffects.StartCooldown(PlayerCharacter.Instance);

        TargetingMode targetingMode = itemEffects.TargetingMode;
        TargetingStrategy strategy = TargetingStrategyUtils.GetTargetingStrategy(targetingMode.TargetingType);
        strategy.Target(targetingMode, targettedPosition, PlayerCharacter.Instance, itemEffects.Effects);
      }
    }
  }

  public void Use(ETriggerType triggerType, Unit target, ItemTriggerEventContext context = null)
  {
    effects.TryGetValue(triggerType, out List<ItemEffect> effectsToApply);
    if (effectsToApply != null)
    {
      foreach (var itemEffects in effectsToApply)
      {
        if (!itemEffects.CanUse())
          continue;
        itemEffects.StartCooldown(PlayerCharacter.Instance);

        foreach (var effect in itemEffects.Effects)
        {
          effect.ApplyEffect(PlayerCharacter.Instance, target);
        }
      }
    }
  }

  private void ConnectEffectTrigger(ETriggerType triggerType)
  {
    switch (triggerType)
    {
      case ETriggerType.OnMove:
        ItemTriggerEventSystem.Instance.MoveTriggerEvent += UseOnMove;
        break;
      case ETriggerType.OnHit:
        ItemTriggerEventSystem.Instance.HitTriggerEvent += UseOnHit;
        break;
      case ETriggerType.OnDamageTaken:
        ItemTriggerEventSystem.Instance.DamageTakenTriggerEvent += UseOnDamageTaken;
        break;
      case ETriggerType.OnHeatGain:
        ItemTriggerEventSystem.Instance.HeatGainTriggerEvent += UseOnHeatGain;
        break;
      case ETriggerType.Active1:
        ItemTriggerEventSystem.Instance.Active1TriggerEvent += UseOnActive1;
        break;
      case ETriggerType.Active2:
        ItemTriggerEventSystem.Instance.Active2TriggerEvent += UseOnActive2;
        break;
      case ETriggerType.Active3:
        ItemTriggerEventSystem.Instance.Active3TriggerEvent += UseOnActive3;
        break;
      case ETriggerType.Active3Release:
        ItemTriggerEventSystem.Instance.Active3ReleaseTriggerEvent += UseOnActive3Release;
        break;
    }
  }

  private void DisconnectEffectTrigger(ETriggerType triggerType)
  {
    switch (triggerType)
    {
      case ETriggerType.OnMove:
        ItemTriggerEventSystem.Instance.MoveTriggerEvent -= UseOnMove;
        break;
      case ETriggerType.OnHit:
        ItemTriggerEventSystem.Instance.HitTriggerEvent -= UseOnHit;
        break;
      case ETriggerType.OnDamageTaken:
        ItemTriggerEventSystem.Instance.DamageTakenTriggerEvent -= UseOnDamageTaken;
        break;
      case ETriggerType.OnHeatGain:
        ItemTriggerEventSystem.Instance.HeatGainTriggerEvent -= UseOnHeatGain;
        break;
      case ETriggerType.Active1:
        ItemTriggerEventSystem.Instance.Active1TriggerEvent -= UseOnActive1;
        break;
      case ETriggerType.Active2:
        ItemTriggerEventSystem.Instance.Active2TriggerEvent -= UseOnActive2;
        break;
      case ETriggerType.Active3:
        ItemTriggerEventSystem.Instance.Active3TriggerEvent -= UseOnActive3;
        break;
      case ETriggerType.Active3Release:
        ItemTriggerEventSystem.Instance.Active3ReleaseTriggerEvent -= UseOnActive3Release;
        break;
    }
  }

  private void UseOnMove(ItemTriggerEventContext context)
  {
    Use(ETriggerType.OnMove, context.TargettedPosition, context);
  }

  private void UseOnHit(ItemTriggerEventContext context)
  {
    // rework when onHit effect is introduced
    Use(ETriggerType.OnHit, context.EnemyHit, context);
  }

  private void UseOnDamageTaken(ItemTriggerEventContext context)
  {
    // rework when onDamageTaken effect is introduced
    Use(ETriggerType.OnDamageTaken, context.TargettedPosition, context);
  }

  private void UseOnHeatGain(ItemTriggerEventContext context)
  {
    // rework when onHeatGain effect is introduced
    Use(ETriggerType.OnHeatGain, context.TargettedPosition, context);
  }

  private void UseOnActive1(ItemTriggerEventContext context)
  {
    if (context.ItemActivated != Id)
      return;

    Use(ETriggerType.Active1, context.TargettedPosition, context);
    Use(ETriggerType.OnSelfActive1, context.TargettedPosition, context);
  }

  private void UseOnActive2(ItemTriggerEventContext context)
  {
    if (context.ItemActivated != Id)
      return;

    Use(ETriggerType.Active2, context.TargettedPosition, context);
    Use(ETriggerType.OnSelfActive2, context.TargettedPosition, context);
  }

  private void UseOnActive3(ItemTriggerEventContext context)
  {
    if (context.ItemActivated != Id)
      return;

    Use(ETriggerType.Active3, context.TargettedPosition, context);
    Use(ETriggerType.OnSelfActive3, context.TargettedPosition, context);
  }

  private void UseOnActive3Release(ItemTriggerEventContext context)
  {
    if (context.ItemActivated != Id)
      return;

    Use(ETriggerType.Active3Release, context.TargettedPosition, context);
  }
}