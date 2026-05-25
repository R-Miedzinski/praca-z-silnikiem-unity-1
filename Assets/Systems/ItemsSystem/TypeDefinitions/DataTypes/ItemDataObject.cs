using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataObject", menuName = "Items/ItemDataObject")]
public class ItemDataObject : ScriptableObject
{
  [Tooltip("Unique identifier for the item.")]
  public string Id;
  public string ItemName;
  public string Description;
  [Tooltip("Equipment slot for the item. Item can only be equipped in this slot.")]
  public EEquipmentSlot EquipmentSlot;
  public ItemEffectDataObject[] Effects;
  // public List<Upgrade> Upgrades;
  // public Dictionary<int, Upgrade[]> UpgradeChoices; // key: upgrade level, value: available choices
}