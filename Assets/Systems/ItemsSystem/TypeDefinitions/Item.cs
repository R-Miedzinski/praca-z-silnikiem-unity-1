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
}