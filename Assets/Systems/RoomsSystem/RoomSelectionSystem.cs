using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Loads room definitions, indexes them by ID, and creates a randomized room order at game start.
// Other systems can read the generated order through RoomSelectionSystem.Instance.
public class RoomSelectionSystem : MonoBehaviour
{
  private const string RoomsDataPath = "Assets/DataObjects/Rooms";

  [SerializeField]
  [Tooltip("When enabled, all available rooms from the database are used in the generated order.")]
  private bool useAllRooms = true;
  [SerializeField]
  [Tooltip("Room IDs to use when Use All Rooms is disabled.")]
  private string[] roomIds;
  [SerializeField]
  [Tooltip("Runtime-safe room definitions. Fill this list for builds, because AssetDatabase only works in the Unity Editor.")]
  private RoomDataObject[] roomDefinitions;
  [SerializeField]
  [Tooltip("Use a fixed seed to make the random room order repeatable.")]
  private bool useRandomSeed;
  [SerializeField]
  [Tooltip("Seed used when Use Random Seed is enabled.")]
  private int randomSeed;

  // Global access point for the active room selection system in the current scene.
  public static RoomSelectionSystem Instance { get; private set; }

  // Randomized room order generated during Awake or by calling GenerateRoomOrder.
  public IReadOnlyList<RoomDataObject> RoomOrder => roomOrder;

  private readonly Dictionary<string, RoomDataObject> roomsById = new Dictionary<string, RoomDataObject>();
  private readonly List<RoomDataObject> roomOrder = new List<RoomDataObject>();

  // Initializes the singleton, loads room definitions, and generates the first room order.
  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
      return;
    }

    InitializeRooms();
    GenerateRoomOrder();
  }

  // Returns a loaded room definition by its unique ID.
  // Room ID is stored in RoomDataObject.Id.
  // Returns null when no room with this ID exists.
  public RoomDataObject GetRoomById(string id)
  {
    if (roomsById.TryGetValue(id, out RoomDataObject room))
    {
      return room;
    }

    Debug.LogWarning($"Room with ID {id} not found.");
    return null;
  }

  // Builds and shuffles the room order from either all available rooms or the selected room ID list.
  public IReadOnlyList<RoomDataObject> GenerateRoomOrder()
  {
    roomOrder.Clear();

    if (useAllRooms)
    {
      foreach (RoomDataObject room in roomsById.Values)
      {
        if (room != null && room.AvailableInRoomPool)
        {
          roomOrder.Add(room);
        }
      }
    }
    else
    {
      foreach (string roomId in roomIds)
      {
        if (string.IsNullOrWhiteSpace(roomId))
        {
          continue;
        }

        RoomDataObject room = GetRoomById(roomId);
        if (room != null && room.AvailableInRoomPool)
        {
          roomOrder.Add(room);
        }
      }
    }

    roomOrder.Sort((firstRoom, secondRoom) => string.CompareOrdinal(firstRoom.Id, secondRoom.Id));
    Shuffle(roomOrder);
    return RoomOrder;
  }

  // Returns the current generated room order without changing it.
  public IReadOnlyList<RoomDataObject> GetRoomOrder()
  {
    return RoomOrder;
  }


  // Clears and rebuilds the room lookup dictionary from editor assets and serialized room definitions.
  private void InitializeRooms()
  {
    roomsById.Clear();

#if UNITY_EDITOR
    LoadRoomsFromDataObjectsFolder();
#endif

    if (roomDefinitions == null)
    {
      return;
    }

    foreach (RoomDataObject roomDefinition in roomDefinitions)
    {
      AddRoom(roomDefinition);
    }
  }

#if UNITY_EDITOR
  // Editor-only helper that loads all RoomDataObject assets from Assets/DataObjects/Rooms.
  private void LoadRoomsFromDataObjectsFolder()
  {
    string[] roomGuids = AssetDatabase.FindAssets("t:RoomDataObject", new[] { RoomsDataPath });
    foreach (string roomGuid in roomGuids)
    {
      string roomPath = AssetDatabase.GUIDToAssetPath(roomGuid);
      RoomDataObject roomDefinition = AssetDatabase.LoadAssetAtPath<RoomDataObject>(roomPath);
      AddRoom(roomDefinition);
    }
  }
#endif

  // Adds a room definition to the lookup dictionary when it has a valid unique ID.
  private void AddRoom(RoomDataObject roomDefinition)
  {
    if (roomDefinition == null)
    {
      return;
    }

    if (string.IsNullOrWhiteSpace(roomDefinition.Id))
    {
      Debug.LogWarning($"RoomDataObject {roomDefinition.name} has empty ID.");
      return;
    }

    if (roomsById.ContainsKey(roomDefinition.Id))
    {
      Debug.LogWarning($"Room with ID {roomDefinition.Id} already exists in the room selection system.");
      return;
    }

    roomsById.Add(roomDefinition.Id, roomDefinition);
  }

  // Randomizes room order in place using Fisher-Yates shuffle.
  private void Shuffle(List<RoomDataObject> rooms)
  {
    System.Random random = useRandomSeed ? new System.Random(randomSeed) : new System.Random();

    for (int i = rooms.Count - 1; i > 0; i--)
    {
      int randomIndex = random.Next(i + 1);
      RoomDataObject currentRoom = rooms[i];
      rooms[i] = rooms[randomIndex];
      rooms[randomIndex] = currentRoom;
    }
  }
}
