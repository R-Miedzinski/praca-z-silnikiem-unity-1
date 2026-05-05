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
      effects[effectData.TriggerType].Add(new ItemEffect(effectData.Effect, effectData.TriggerType, effectData.Cooldown, effectData.TargettingMode));
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
      Effect newEffect = IdToEffectMap.GetEffectById(effectData.EffectId, effectData.EffectParams);
      TargettingMode targettingMode = new TargettingMode(effectData.TargettingMode);
      if (!effects.ContainsKey(effectData.TriggerType))
      {
        effects[effectData.TriggerType] = new List<ItemEffect>();
      }
      effects[effectData.TriggerType].Add(new ItemEffect(newEffect, effectData.TriggerType, effectData.Cooldown, targettingMode));
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
      foreach (var effect in effectsToApply)
      {
        if (!effect.CanUse())
          continue;

        TargettingMode targettingMode = effect.TargettingMode;
        TargettingStrategy strategy = TargettingStrategyUtils.GetTargettingStrategy(targettingMode.TargettingType);
        Unit[] targets = strategy.Target(targettingMode, targettedPosition, PlayerCharacter.Instance.transform.position);

        foreach (var target in targets)
        {
          effect.Effect.ApplyEffect(PlayerCharacter.Instance, target);
        }
        effect.StartCooldown();
      }
    }
  }

  public void Use(ETriggerType triggerType, Unit target, ItemTriggerEventContext context = null)
  {
    effects.TryGetValue(triggerType, out List<ItemEffect> effectsToApply);
    if (effectsToApply != null)
    {
      foreach (var effect in effectsToApply)
      {
        if (!effect.CanUse())
          continue;

        effect.Effect.ApplyEffect(PlayerCharacter.Instance, target);
        effect.StartCooldown();
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
}