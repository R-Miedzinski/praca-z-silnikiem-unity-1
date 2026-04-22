using UnityEngine;

public class ItemsUtils
{
  public static ItemData parseJsonToItemData(string json)
  {
    return JsonUtility.FromJson<ItemData>(json);
  }
}