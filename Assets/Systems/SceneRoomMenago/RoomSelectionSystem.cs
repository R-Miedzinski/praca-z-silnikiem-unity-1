using System.Collections.Generic;
using UnityEngine;

// Builds one randomized room route for the current game run.
public class RoomSelectionSystem : MonoBehaviour
{
  // Path is relative to any folder named Resources.
  private const string RoomsResourcesPath = "DataObjects/Rooms";
  private const string FirstRoomName = "Docking Airlock";
  private const string LastRoomName = "Energy Core";

  // Other scripts can read the same generated room order from this instance.
  public static RoomSelectionSystem Instance { get; private set; }
  [SerializeField] private List<RoomDataObject> availableRooms = new List<RoomDataObject>();
  public List<RoomDataObject> RoomOrder = new List<RoomDataObject>();

  private void Awake()
  {
    // Keep one shared selection system.
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;

    InitializeRoomOrder();
  }

  public void InitializeRoomOrder()
  {
    if (HasValidRoomOrder())
    {
      return;
    }

    RoomDataObject[] rooms = LoadRooms();
    BuildRoomOrder(rooms);
  }

  private RoomDataObject[] LoadRooms()
  {
    availableRooms = new List<RoomDataObject>(Resources.LoadAll<RoomDataObject>(RoomsResourcesPath));

    return GetValidAvailableRooms().ToArray();
  }

  private bool HasValidRoomOrder()
  {
    if (RoomOrder == null || RoomOrder.Count == 0)
    {
      return false;
    }

    foreach (RoomDataObject room in RoomOrder)
    {
      if (!IsValidRoom(room))
      {
        return false;
      }
    }

    return true;
  }

  private List<RoomDataObject> GetValidAvailableRooms()
  {
    List<RoomDataObject> validRooms = new List<RoomDataObject>();

    if (availableRooms == null || availableRooms.Count == 0)
    {
      Debug.LogWarning($"RoomSelectionSystem did not find any rooms in Resources/{RoomsResourcesPath}.");
      return validRooms;
    }

    HashSet<RoomDataObject> addedRooms = new HashSet<RoomDataObject>();
    foreach (RoomDataObject room in availableRooms)
    {
      if (!IsValidRoom(room) || !addedRooms.Add(room))
      {
        continue;
      }

      validRooms.Add(room);
    }

    return validRooms;
  }

  private bool IsValidRoom(RoomDataObject room)
  {
    if (room == null)
    {
      Debug.LogWarning("RoomSelectionSystem found a missing room reference.");
      return false;
    }

    if (string.IsNullOrWhiteSpace(room.RoomName))
    {
      Debug.LogWarning($"RoomSelectionSystem room asset '{room.name}' has no RoomName.");
      return false;
    }

    if (room.RoomPrefab == null)
    {
      Debug.LogWarning($"RoomSelectionSystem room '{room.RoomName}' has no RoomPrefab assigned.");
      return false;
    }

    return true;
  }

  private void BuildRoomOrder(RoomDataObject[] rooms)
  {
    if (RoomOrder == null)
    {
      RoomOrder = new List<RoomDataObject>();
    }
    else
    {
      RoomOrder.Clear();
    }

    if (rooms == null || rooms.Length == 0)
    {
      Debug.LogWarning("RoomSelectionSystem did not find any RoomDataObject assets.");
      return;
    }

    // The route always starts and ends with these two important rooms.
    RoomDataObject firstRoom = FindRoomByName(rooms, FirstRoomName);
    RoomDataObject lastRoom = FindRoomByName(rooms, LastRoomName);

    if (firstRoom == null)
    {
      Debug.LogWarning($"RoomSelectionSystem could not find first room '{FirstRoomName}'.");
      return;
    }

    if (lastRoom == null)
    {
      Debug.LogWarning($"RoomSelectionSystem could not find last room '{LastRoomName}'.");
      return;
    }

    // Middle rooms are added once, then shuffled before they go in to RoomOrder.
    RoomOrder.Add(firstRoom);

    List<RoomDataObject> middleRooms = new List<RoomDataObject>();
    foreach (RoomDataObject room in rooms)
    {
      if (room == null || room == firstRoom || room == lastRoom)
      {
        continue;
      }

      middleRooms.Add(room);
    }

    ShuffleRooms(middleRooms);
    RoomOrder.AddRange(middleRooms);
    RoomOrder.Add(lastRoom);
  }

  private RoomDataObject FindRoomByName(RoomDataObject[] rooms, string roomName)
  {
    if (rooms == null || string.IsNullOrWhiteSpace(roomName))
    {
      return null;
    }

    foreach (RoomDataObject room in rooms)
    {
      if (room != null && room.RoomName == roomName)
      {
        return room;
      }
    }

    return null;
  }

  private void ShuffleRooms(List<RoomDataObject> rooms)
  {
    if (rooms == null)
    {
      return;
    }

    // Keeps every room unique while randomizing order.
    for (int i = rooms.Count - 1; i > 0; i--)
    {
      int randomIndex = Random.Range(0, i + 1);
      RoomDataObject currentRoom = rooms[i];
      rooms[i] = rooms[randomIndex];
      rooms[randomIndex] = currentRoom;
    }
  }
}
