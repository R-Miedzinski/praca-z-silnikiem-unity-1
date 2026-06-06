using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

// Defines which door side should be used as the player entry point after loading a room.
public enum RoomSpawnDoorSide
{
  LeftOrBottom,
  Left,
  Bottom
}

// Describes the side of the active room where a source door is placed.
public enum RoomDoorSide
{
  Unknown,
  Left,
  Right,
  Bottom,
  Top
}

// Scene-level system responsible for moving the player group between room prefabs.
// It follows the randomized room order prepared by RoomSelectionSystem.
public class DoorRoomTransitionSystem : MonoBehaviour
{
  [SerializeField] private RoomSelectionSystem roomSelectionSystem;
  [SerializeField] private string playerGroupObjectName = "main";
  [SerializeField] private Transform roomParent;
  [SerializeField] private Vector3 roomSpawnPosition = Vector3.zero;
  [SerializeField] private bool spawnFirstRoomOnStart = true;
  [SerializeField] private bool destroyPreviousRoom = true;
  [SerializeField] private bool loopRoomOrder = false;
  [SerializeField] private float transitionCooldown = 0.5f;
  [SerializeField] private RoomSpawnDoorSide defaultSpawnDoorSide = RoomSpawnDoorSide.LeftOrBottom;
  [SerializeField] private float doorSpawnInset = 1f;
  [SerializeField] private float doorSideDetectionTolerance = 1.5f;
  [FormerlySerializedAs("spawnPlayerInRoomCenter")]
  [SerializeField] private bool fallbackToRoomCenterWhenNoDoor = true;

  // Global access for door scripts and future gameplay systems.
  public static DoorRoomTransitionSystem Instance { get; private set; }

  // Index of the currently active room inside RoomSelectionSystem.RoomOrder.
  public int CurrentRoomIndex { get { return currentRoomIndex; } }
  public RoomDataObject CurrentRoom { get { return GetRoomAtIndex(currentRoomIndex); } }

  private int currentRoomIndex = -1;
  private GameObject activeRoomInstance;
  private float lastTransitionTime = -Mathf.Infinity;
  private readonly HashSet<int> visitedRoomIndexes = new HashSet<int>();

  // Creates the transition system automatically when it was not placed in the scene.
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
  private static void CreateSceneInstanceIfMissing()
  {
    if (Instance != null || UnityEngine.Object.FindFirstObjectByType<DoorRoomTransitionSystem>() != null)
    {
      return;
    }

    GameObject transitionSystemObject = new GameObject("DoorRoomTransitionSystem");
    transitionSystemObject.AddComponent<DoorRoomTransitionSystem>();
  }

  // Sets the singleton and optionally spawns the first room from the selected order.
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

    if (doorSpawnInset <= 0f)
    {
      doorSpawnInset = 1f;
    }

    if (doorSideDetectionTolerance <= 0f)
    {
      doorSideDetectionTolerance = 1.5f;
    }
  }

  // Starts the room flow once other Awake methods have prepared their data.
  private void Start()
  {
    ResolveRoomSelectionSystem();
    EnsureRoomOrder();

    if (spawnFirstRoomOnStart && currentRoomIndex < 0)
    {
      MoveToRoomIndex(0, PlayerCharacter.Instance, Vector2.zero, defaultSpawnDoorSide);
    }
  }

  // Moves the player group to the next room in the generated order.
  public bool MoveToNextRoom(PlayerCharacter player, Vector2 spawnOffset = default, RoomSpawnDoorSide spawnDoorSide = RoomSpawnDoorSide.LeftOrBottom)
  {
    return MoveByOffset(1, player, spawnOffset, spawnDoorSide);
  }

  // Moves the player group to the previous room in the generated order.
  public bool MoveToPreviousRoom(PlayerCharacter player, Vector2 spawnOffset = default, RoomSpawnDoorSide spawnDoorSide = RoomSpawnDoorSide.LeftOrBottom)
  {
    return MoveByOffset(-1, player, spawnOffset, spawnDoorSide);
  }

  // Moves the player group to a room with the given RoomDataObject.Id.
  public bool MoveToRoomById(string roomId, PlayerCharacter player, Vector2 spawnOffset = default, RoomSpawnDoorSide spawnDoorSide = RoomSpawnDoorSide.LeftOrBottom)
  {
    IReadOnlyList<RoomDataObject> roomOrder = GetRoomOrder();
    if (roomOrder.Count == 0)
    {
      Debug.LogWarning("Cannot move to room by ID because room order is empty.");
      return false;
    }

    for (int i = 0; i < roomOrder.Count; i++)
    {
      if (roomOrder[i] != null && roomOrder[i].Id == roomId)
      {
        return MoveToRoomIndex(i, player, spawnOffset, spawnDoorSide);
      }
    }

    Debug.LogWarning($"Room with ID {roomId} is not present in the current room order.");
    return false;
  }

  // Moves by an offset in the current order, for example +1 for next room or -1 for previous room.
  public bool MoveByOffset(int roomIndexOffset, PlayerCharacter player, Vector2 spawnOffset = default, RoomSpawnDoorSide spawnDoorSide = RoomSpawnDoorSide.LeftOrBottom)
  {
    IReadOnlyList<RoomDataObject> roomOrder = GetRoomOrder();
    if (roomOrder.Count == 0)
    {
      Debug.LogWarning("Cannot move between rooms because room order is empty.");
      return false;
    }

    int sourceIndex = currentRoomIndex < 0 ? -1 : currentRoomIndex;
    int targetIndex = sourceIndex + roomIndexOffset;

    if (loopRoomOrder)
    {
      int wrappedTargetIndex = (targetIndex % roomOrder.Count + roomOrder.Count) % roomOrder.Count;
      if (visitedRoomIndexes.Contains(wrappedTargetIndex))
      {
        Debug.Log("Reached an already visited room. Room order will not loop again.");
        return false;
      }

      targetIndex = wrappedTargetIndex;
    }
    else if (targetIndex < 0 || targetIndex >= roomOrder.Count)
    {
      Debug.Log("Reached the end of the room order.");
      return false;
    }

    if (visitedRoomIndexes.Contains(targetIndex))
    {
      Debug.Log("This room was already visited. Each room can appear only once in the current run.");
      return false;
    }

    return MoveToRoomIndex(targetIndex, player, spawnOffset, spawnDoorSide);
  }

  // Returns false when a door is on a side that should only be used as a room entry.
  public bool CanUseDoorAsExit(RoomDoorTransition sourceDoor)
  {
    RoomDoorSide doorSide = GetDoorSide(sourceDoor);
    if (doorSide == RoomDoorSide.Left || doorSide == RoomDoorSide.Bottom)
    {
      Debug.Log("Left and bottom doors are entry-only and cannot move to another room.");
      return false;
    }

    return true;
  }

  // Loads a room prefab by index and places the player group at the chosen entry door.
  public bool MoveToRoomIndex(int targetIndex, PlayerCharacter player, Vector2 spawnOffset = default, RoomSpawnDoorSide spawnDoorSide = RoomSpawnDoorSide.LeftOrBottom)
  {
    if (Time.time < lastTransitionTime + transitionCooldown)
    {
      return false;
    }

    RoomDataObject targetRoom = GetRoomAtIndex(targetIndex);
    if (targetRoom == null)
    {
      Debug.LogWarning($"Cannot move to room index {targetIndex} because room data is missing.");
      return false;
    }

    if (visitedRoomIndexes.Contains(targetIndex))
    {
      Debug.Log($"Room {targetRoom.Id} ({targetRoom.RoomName}) was already visited and cannot appear again.");
      return false;
    }

    GameObject targetRoomPrefab = ResolveRoomPrefab(targetRoom);
    if (targetRoomPrefab == null)
    {
      Debug.LogWarning($"Cannot move to room {targetRoom.Id} ({targetRoom.RoomName}) because RoomPrefab is not assigned and no matching prefab was found.");
      return false;
    }

    GameObject nextRoomInstance = InstantiateRoom(targetRoom, targetRoomPrefab);
    if (nextRoomInstance == null)
    {
      return false;
    }

    GameObject previousRoomInstance = activeRoomInstance;
    activeRoomInstance = nextRoomInstance;
    currentRoomIndex = targetIndex;
    visitedRoomIndexes.Add(targetIndex);
    lastTransitionTime = Time.time;

    MovePlayerGroupToRoom(player, targetRoom, spawnOffset, spawnDoorSide);

    if (destroyPreviousRoom && previousRoomInstance != null)
    {
      Destroy(previousRoomInstance);
    }

    return true;
  }

  // Finds or caches the room selection system used as the source of room order.
  private void ResolveRoomSelectionSystem()
  {
    if (roomSelectionSystem == null)
    {
      roomSelectionSystem = RoomSelectionSystem.Instance;
    }

    if (roomSelectionSystem == null)
    {
      roomSelectionSystem = UnityEngine.Object.FindFirstObjectByType<RoomSelectionSystem>();
    }

    if (roomSelectionSystem == null)
    {
      GameObject roomSelectionSystemObject = new GameObject("RoomSelectionSystem");
      roomSelectionSystem = roomSelectionSystemObject.AddComponent<RoomSelectionSystem>();
    }
  }

  // Makes sure RoomSelectionSystem already has an order available before doors use it.
  private void EnsureRoomOrder()
  {
    if (roomSelectionSystem == null)
    {
      Debug.LogWarning("DoorRoomTransitionSystem needs a RoomSelectionSystem in the scene.");
      return;
    }

    if (roomSelectionSystem.RoomOrder.Count == 0)
    {
      roomSelectionSystem.GenerateRoomOrder();
      visitedRoomIndexes.Clear();
      currentRoomIndex = -1;
    }
  }

  // Returns the current order prepared by RoomSelectionSystem.
  private IReadOnlyList<RoomDataObject> GetRoomOrder()
  {
    ResolveRoomSelectionSystem();
    EnsureRoomOrder();

    if (roomSelectionSystem == null)
    {
      return new List<RoomDataObject>();
    }

    return roomSelectionSystem.RoomOrder;
  }

  // Reads a room from the current order with bounds checks.
  private RoomDataObject GetRoomAtIndex(int roomIndex)
  {
    IReadOnlyList<RoomDataObject> roomOrder = GetRoomOrder();
    if (roomIndex < 0 || roomIndex >= roomOrder.Count)
    {
      return null;
    }

    return roomOrder[roomIndex];
  }

  // Returns the assigned prefab, or in the editor finds it by room name in Assets/Prefabs/Rooms.
  private GameObject ResolveRoomPrefab(RoomDataObject roomData)
  {
    if (roomData.RoomPrefab != null)
    {
      return roomData.RoomPrefab;
    }

#if UNITY_EDITOR
    string roomPrefabPath = $"Assets/Prefabs/Rooms/{roomData.RoomName}.prefab";
    GameObject roomPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(roomPrefabPath);
    if (roomPrefab != null)
    {
      return roomPrefab;
    }
#endif

    return null;
  }

  // Instantiates the selected room prefab at the configured scene position.
  private GameObject InstantiateRoom(RoomDataObject roomData, GameObject roomPrefab)
  {
    GameObject roomInstance = Instantiate(roomPrefab, roomSpawnPosition, Quaternion.identity, roomParent);
    roomInstance.name = roomData.RoomName;
    return roomInstance;
  }

  // Moves the whole main group so player, equipment, UI helpers, and related children stay together.
  private void MovePlayerGroupToRoom(PlayerCharacter player, RoomDataObject roomData, Vector2 spawnOffset, RoomSpawnDoorSide spawnDoorSide)
  {
    GameObject playerGroup = FindPlayerGroup(player);
    Vector3 targetPlayerPosition = GetRoomSpawnPoint(roomData, spawnDoorSide) + (Vector3)spawnOffset;

    if (player != null && playerGroup != null)
    {
      Vector3 movementDelta = targetPlayerPosition - player.transform.position;
      playerGroup.transform.position += movementDelta;
      return;
    }

    if (playerGroup != null)
    {
      playerGroup.transform.position = targetPlayerPosition;
      return;
    }

    if (player != null)
    {
      player.transform.position = targetPlayerPosition;
    }
  }

  // Chooses a safe player spawn point near the left or bottom door of the active room.
  private Vector3 GetRoomSpawnPoint(RoomDataObject roomData, RoomSpawnDoorSide spawnDoorSide)
  {
    if (TryGetDoorSpawnPoint(spawnDoorSide, out Vector3 doorSpawnPoint))
    {
      return doorSpawnPoint;
    }

    if (fallbackToRoomCenterWhenNoDoor && activeRoomInstance != null)
    {
      Bounds roomBounds = CalculateRoomBounds(activeRoomInstance);
      if (roomBounds.size != Vector3.zero)
      {
        return new Vector3(roomBounds.center.x, roomBounds.center.y, roomSpawnPosition.z);
      }
    }

    return roomSpawnPosition + (Vector3)roomData.DefaultSpawnPoint;
  }

  // Finds the best entry door on the requested side and offsets the spawn into the room.
  private bool TryGetDoorSpawnPoint(RoomSpawnDoorSide spawnDoorSide, out Vector3 spawnPoint)
  {
    spawnPoint = Vector3.zero;

    if (activeRoomInstance == null)
    {
      return false;
    }

    Bounds roomBounds = CalculateRoomBounds(activeRoomInstance);
    if (roomBounds.size == Vector3.zero)
    {
      return false;
    }

    if (spawnDoorSide == RoomSpawnDoorSide.LeftOrBottom)
    {
      if (TryFindDoorOnSide(RoomSpawnDoorSide.Left, roomBounds, out spawnPoint))
      {
        return true;
      }

      return TryFindDoorOnSide(RoomSpawnDoorSide.Bottom, roomBounds, out spawnPoint);
    }

    return TryFindDoorOnSide(spawnDoorSide, roomBounds, out spawnPoint);
  }

  // Searches room doors by their distance from the selected room side.
  private bool TryFindDoorOnSide(RoomSpawnDoorSide spawnDoorSide, Bounds roomBounds, out Vector3 spawnPoint)
  {
    spawnPoint = Vector3.zero;
    RoomDoorTransition[] roomDoors = activeRoomInstance.GetComponentsInChildren<RoomDoorTransition>(true);

    if (roomDoors.Length == 0)
    {
      return false;
    }

    RoomDoorTransition selectedDoor = null;
    Vector3 selectedDoorPosition = Vector3.zero;
    float bestDistance = float.MaxValue;
    float sideTolerance = Mathf.Max(doorSideDetectionTolerance, 0.1f);

    foreach (RoomDoorTransition roomDoor in roomDoors)
    {
      if (roomDoor == null)
      {
        continue;
      }

      Vector3 doorPosition = GetDoorPosition(roomDoor);
      float sideDistance = GetDoorSideDistance(doorPosition, roomBounds, spawnDoorSide);
      if (sideDistance > sideTolerance || sideDistance >= bestDistance)
      {
        continue;
      }

      selectedDoor = roomDoor;
      selectedDoorPosition = doorPosition;
      bestDistance = sideDistance;
    }

    if (selectedDoor == null)
    {
      return false;
    }

    Vector3 inwardDirection = spawnDoorSide == RoomSpawnDoorSide.Bottom ? Vector3.up : Vector3.right;
    spawnPoint = selectedDoorPosition + inwardDirection * doorSpawnInset;
    spawnPoint.z = roomSpawnPosition.z;
    return true;
  }

  // Reads the most stable world position from a door collider, renderer, or transform.
  private Vector3 GetDoorPosition(RoomDoorTransition roomDoor)
  {
    Collider2D doorCollider = roomDoor.GetComponentInChildren<Collider2D>();
    if (doorCollider != null)
    {
      return doorCollider.bounds.center;
    }

    Renderer doorRenderer = roomDoor.GetComponentInChildren<Renderer>();
    if (doorRenderer != null)
    {
      return doorRenderer.bounds.center;
    }

    return roomDoor.transform.position;
  }

  // Detects the room side closest to a door. Left and bottom are blocked as exits.
  private RoomDoorSide GetDoorSide(RoomDoorTransition roomDoor)
  {
    if (roomDoor == null || activeRoomInstance == null)
    {
      return RoomDoorSide.Unknown;
    }

    if (!roomDoor.transform.IsChildOf(activeRoomInstance.transform))
    {
      return RoomDoorSide.Unknown;
    }

    Bounds roomBounds = CalculateRoomBounds(activeRoomInstance);
    if (roomBounds.size == Vector3.zero)
    {
      return RoomDoorSide.Unknown;
    }

    Vector3 doorPosition = GetDoorPosition(roomDoor);
    float leftDistance = Mathf.Abs(doorPosition.x - roomBounds.min.x);
    float rightDistance = Mathf.Abs(doorPosition.x - roomBounds.max.x);
    float bottomDistance = Mathf.Abs(doorPosition.y - roomBounds.min.y);
    float topDistance = Mathf.Abs(doorPosition.y - roomBounds.max.y);

    RoomDoorSide nearestSide = RoomDoorSide.Left;
    float nearestDistance = leftDistance;

    if (rightDistance < nearestDistance)
    {
      nearestSide = RoomDoorSide.Right;
      nearestDistance = rightDistance;
    }

    if (bottomDistance < nearestDistance)
    {
      nearestSide = RoomDoorSide.Bottom;
      nearestDistance = bottomDistance;
    }

    if (topDistance < nearestDistance)
    {
      nearestSide = RoomDoorSide.Top;
      nearestDistance = topDistance;
    }

    float sideTolerance = Mathf.Max(doorSideDetectionTolerance, 0.1f);
    return nearestDistance <= sideTolerance ? nearestSide : RoomDoorSide.Unknown;
  }

  // Measures how close a door is to the left or bottom side of the room bounds.
  private float GetDoorSideDistance(Vector3 doorPosition, Bounds roomBounds, RoomSpawnDoorSide spawnDoorSide)
  {
    if (spawnDoorSide == RoomSpawnDoorSide.Bottom)
    {
      return Mathf.Abs(doorPosition.y - roomBounds.min.y);
    }

    return Mathf.Abs(doorPosition.x - roomBounds.min.x);
  }

  // Calculates room bounds from renderers and colliders so the spawn point can land in the room center.
  private Bounds CalculateRoomBounds(GameObject roomInstance)
  {
    Renderer[] renderers = roomInstance.GetComponentsInChildren<Renderer>();
    Bounds bounds = new Bounds(roomInstance.transform.position, Vector3.zero);
    bool hasBounds = false;

    foreach (Renderer roomRenderer in renderers)
    {
      if (!hasBounds)
      {
        bounds = roomRenderer.bounds;
        hasBounds = true;
      }
      else
      {
        bounds.Encapsulate(roomRenderer.bounds);
      }
    }

    Collider2D[] colliders = roomInstance.GetComponentsInChildren<Collider2D>();
    foreach (Collider2D roomCollider in colliders)
    {
      if (!hasBounds)
      {
        bounds = roomCollider.bounds;
        hasBounds = true;
      }
      else
      {
        bounds.Encapsulate(roomCollider.bounds);
      }
    }

    return hasBounds ? bounds : new Bounds(roomInstance.transform.position, Vector3.zero);
  }

  // Finds the object named "main" first, then falls back to the root object that owns the player.
  private GameObject FindPlayerGroup(PlayerCharacter player)
  {
    GameObject playerGroup = GameObject.Find(playerGroupObjectName);
    if (playerGroup != null)
    {
      return playerGroup;
    }

    if (player != null)
    {
      return player.transform.root.gameObject;
    }

    return null;
  }
}
