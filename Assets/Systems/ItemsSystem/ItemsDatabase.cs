using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ItemsDatabase : MonoBehaviour
{
  [SerializeField]
  private const string itemsDataPath = "DataObjects/ItemData";

  public static ItemsDatabase Instance { get { return instance; } private set { instance = value; } }

  private static ItemsDatabase instance;
  private Dictionary<string, Item> items;
  private string itemDataDefinitionsPath = "";

  public ItemsDatabase()
  {
    items = new Dictionary<string, Item>();
    itemDataDefinitionsPath = Path.Combine(Application.dataPath, itemsDataPath);
  }

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

  public void AddItem(ItemData itemData)
  {
    if (!items.ContainsKey(itemData.Id))
    {
      items[itemData.Id] = new Item(itemData);
    }
    else
    {
      Debug.LogWarning($"Item with ID {itemData.Id} already exists in the database.");
    }
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
    DirectoryInfo dir = new DirectoryInfo(itemDataDefinitionsPath);

    // Handle json items
    FileInfo[] jsonItems = dir.GetFiles("*.json");
    foreach (FileInfo f in jsonItems)
    {
      string json = File.ReadAllText(f.FullName);
      if (json != null)
      {
        ItemData itemData = ItemsUtils.ParseJsonToItemData(json);
        AddItem(itemData);
      }
      else
      {
        Debug.LogError($"Failed to parse ItemData from {f.Name} \n path: {f.FullName}");
      }
    }

    // Handle scriptable object items
    FileInfo[] soItems = dir.GetFiles("*.asset");
    foreach (FileInfo f in soItems)
    {
      string itemPath = $"Assets/{itemsDataPath}/{f.Name}";

      ItemDataObject itemDataObject = AssetDatabase.LoadAssetAtPath<ItemDataObject>(itemPath);
      if (itemDataObject != null)
      {
        AddItem(itemDataObject);
      }
      else
      {
        Debug.LogError($"Failed to load ItemDataObject from {f.Name} \n path: {itemPath}");
      }
    }
  }
}
