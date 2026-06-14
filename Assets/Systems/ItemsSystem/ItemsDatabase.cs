using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

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

    // Handle scriptable object items
    FileInfo[] soItems = dir.GetFiles("*.asset");
    foreach (FileInfo f in soItems)
    {
      try
      {
        string assetPath = Path.Combine("Assets", itemsDataPath, f.Name);
        ItemDataObject itemDataObject = AssetDatabase.LoadAssetAtPath<ItemDataObject>(assetPath);
        if (itemDataObject != null)
        {
          AddItem(itemDataObject);
        }
        else
        {
          Debug.LogError($"Failed to load ItemDataObject from {f.Name} \n path: {assetPath}");
        }
      }
      catch (System.Exception ex)
      {
        Debug.LogError($"Error loading ScriptableObject file {f.Name}: {ex.Message}");
      }
    }
  }
}