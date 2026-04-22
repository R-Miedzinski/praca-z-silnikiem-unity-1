using System.Collections.Generic;

public class Item
{
  public string Id { get { return id; } }
  public string ItemName { get { return itemName; } }
  public EEquipmentSlot EquipmentSlot { get { return equipmentSlot; } }
  private string id;
  private string itemName;
  private EEquipmentSlot equipmentSlot;
  private List<ItemEffect> effects;

  public Item(string id, string name, EEquipmentSlot equipmentSlot, ItemEffect[] effects)
  {
    this.id = id;
    this.itemName = name;
    this.equipmentSlot = equipmentSlot;
    this.effects = new List<ItemEffect>();
    foreach (var effectData in effects)
    {
      this.effects.Add(new ItemEffect(effectData.Effect, effectData.TriggerType, effectData.Cooldown, effectData.TargettingMode));
    }
  }

  public Item(ItemData itemData)
  {
    this.id = itemData.Id;
    this.itemName = itemData.ItemName;
    this.equipmentSlot = itemData.EquipmentSlot;
    this.effects = new List<ItemEffect>();
    foreach (var effectData in itemData.Effects)
    {
      Effect newEffect = IdToEffectMap.GetEffectById(effectData.EffectId, effectData.EffectParams);
      TargettingMode targettingMode = new TargettingMode(effectData.TargettingMode);
      this.effects.Add(new ItemEffect(newEffect, effectData.TriggerType, effectData.Cooldown, targettingMode));
    }
  }
}