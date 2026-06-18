using UnityEngine;
using System.Collections.Generic;

public class ItemsDatabase : MonoBehaviour
{
  private const string itemsDataPath = "DataObjects/ItemData";

  public static ItemsDatabase Instance { get { return instance; } private set { instance = value; } }

  private static ItemsDatabase instance;
  private readonly Dictionary<string, Item> items = new Dictionary<string, Item>();

  public void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
    }

    InitializeDatabase();
  }

  public void AddItem(ItemDataObject itemDataObject)
  {
    if (!items.ContainsKey(itemDataObject.Id))
    {
      items[itemDataObject.Id] = new Item(itemDataObject);
    }
    else
    {
      Debug.LogWarning($"Item with ID {itemDataObject.Id} already exists in the database.");
    }
  }

  public Item GetItemById(string id)
  {
    if (items.TryGetValue(id, out Item item))
    {
      return item;
    }

    Debug.LogWarning($"Item with ID {id} not found in the database.");
    return default(Item);
  }

  private void InitializeDatabase()
  {
    items.Clear();

    // Resources works the same in editor and in player builds.
    ItemDataObject[] soItems = Resources.LoadAll<ItemDataObject>(itemsDataPath);

    if (soItems == null || soItems.Length == 0)
    {
      Debug.LogWarning($"No ItemDataObject assets found at Resources/{itemsDataPath}.");
      return;
    }

    foreach (ItemDataObject itemDataObject in soItems)
    {
      if (itemDataObject == null)
      {
        continue;
      }

      AddItem(itemDataObject);
    }
  }
}