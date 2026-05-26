using UnityEngine;

// TODO: Move to scriptable objects and editor asset config
[System.Serializable]
public struct ItemData
{
  public string Id;
  public string ItemName;
  public string Description;
  public ItemEffectData[] Effects;
  // public List<Upgrade> Upgrades;
  // public Dictionary<int, Upgrade[]> UpgradeChoices; // key: upgrade level, value: available choices
  public EEquipmentSlot EquipmentSlot;
}