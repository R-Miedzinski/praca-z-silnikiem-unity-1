using UnityEngine;

public class ItemsUtils
{
  public static ItemData ParseJsonToItemData(string json)
  {
    return JsonUtility.FromJson<ItemData>(json);
  }
}