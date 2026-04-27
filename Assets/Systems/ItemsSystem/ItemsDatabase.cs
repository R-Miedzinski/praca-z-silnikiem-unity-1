using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ItemsDatabase : MonoBehaviour
{
  public static ItemsDatabase Instance { get { return instance; } private set { instance = value; } }

  private static ItemsDatabase instance;
  private Dictionary<string, Item> items;
  private Dictionary<string, ItemData> itemsData;
  [SerializeField]
  private string itemsDataPath = "Systems/ItemsSystem/ItemData";
  private string itemDataDefinitionsPath = "";

  public ItemsDatabase()
  {
    items = new Dictionary<string, Item>();
    itemsData = new Dictionary<string, ItemData>();
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
    if (!itemsData.ContainsKey(itemData.Id))
    {
      itemsData[itemData.Id] = itemData;
      items[itemData.Id] = new Item(itemData);
    }
    else
    {
      UnityEngine.Debug.LogWarning($"Item with ID {itemData.Id} already exists in the database.");
    }
  }

  public Item GetItemById(string id)
  {
    if (items.TryGetValue(id, out Item item))
    {
      return item;
    }

    UnityEngine.Debug.LogWarning($"Item with ID {id} not found in the database.");
    return default(Item);
  }

  private void InitializeDatabase()
  {
    DirectoryInfo dir = new DirectoryInfo(itemDataDefinitionsPath);
    FileInfo[] info = dir.GetFiles("*.json");
    foreach (FileInfo f in info)
    {
      string json = File.ReadAllText(f.FullName);
      ItemData itemData = ItemsUtils.parseJsonToItemData(json);
      AddItem(itemData);
    }
  }
}