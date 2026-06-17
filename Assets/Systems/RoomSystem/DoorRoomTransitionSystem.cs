using System.Collections.Generic;
using UnityEngine;

// Loads rooms from RoomOrder and moves the player between them.
public class DoorRoomTransitionSystem : MonoBehaviour
{
  public static DoorRoomTransitionSystem Instance { get; private set; }

  [SerializeField] private RoomSelectionSystem roomSelectionSystem;
  [SerializeField] private Transform roomParent;
  [SerializeField] private PlayerCharacter player;
  [SerializeField] private float playerSpawnOffset = 2f;

  public int CurrentRoomIndex { get; private set; } = -1;
  public GameObject CurrentRoomInstance { get; private set; }
  public List<RoomDataObject> RoomOrder { get { return roomSelectionSystem != null ? roomSelectionSystem.RoomOrder : null; } }

  private void Awake()
  {
    // Keep one scene transition system that owns the active room instance.
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;

    roomSelectionSystem = GetOrCreateRoomSelectionSystem();
    roomSelectionSystem.InitializeRoomOrder();
    LoadFirstRoom();
  }

  public void LoadFirstRoom()
  {
    LoadRoomAtIndex(0);
  }

  public void LoadNextRoom()
  {
    LoadRoomAtIndex(CurrentRoomIndex + 1);
  }

  public void LoadNextRoom(PlayerCharacter transitionPlayer)
  {
    player = transitionPlayer;
    LoadNextRoom();
  }

  private bool LoadRoomAtIndex(int roomIndex)
  {
    if (RoomOrder == null || RoomOrder.Count == 0)
    {
      Debug.LogWarning("DoorRoomTransitionSystem cannot load a room because RoomOrder is empty.");
      return false;
    }

    if (roomIndex < 0 || roomIndex >= RoomOrder.Count)
    {
      Debug.LogWarning("DoorRoomTransitionSystem reached the end of RoomOrder.");
      return false;
    }

    RoomDataObject roomData = RoomOrder[roomIndex];
    if (roomData == null || roomData.RoomPrefab == null)
    {
      Debug.LogWarning($"DoorRoomTransitionSystem cannot load room at index {roomIndex}.");
      return false;
    }

    if (CurrentRoomInstance != null)
    {
      Destroy(CurrentRoomInstance);
    }

    Transform parent = roomParent != null ? roomParent : transform;
    CurrentRoomInstance = Instantiate(roomData.RoomPrefab, Vector3.zero, Quaternion.identity, parent);
    CurrentRoomIndex = roomIndex;
    MovePlayerToSpawn(roomData);

    return true;
  }

  private void MovePlayerToSpawn(RoomDataObject roomData)
  {
    if (player == null)
    {
      player = PlayerCharacter.Instance;
    }

    if (player == null || CurrentRoomInstance == null)
    {
      return;
    }

    RoomDoorTransition entranceDoor = FindEntranceDoor();
    if (entranceDoor != null)
    {
      // Entrance doors decide the exact spawn point.
      player.transform.position = GetPlayerSpawnPositionNearDoor(entranceDoor);
      return;
    }

    // SpawnPoint is used as fallback when the loaded room has no entrance door.
    player.transform.position = CurrentRoomInstance.transform.TransformPoint(roomData.SpawnPoint);
  }

  private RoomDoorTransition FindEntranceDoor()
  {
    if (CurrentRoomInstance == null)
    {
      return null;
    }

    // Designers mark entrance doors.
    foreach (RoomDoorTransition door in CurrentRoomInstance.GetComponentsInChildren<RoomDoorTransition>())
    {
      if (door != null && door.IsEntranceDoor)
      {
        return door;
      }
    }

    return null;
  }

  private Vector3 GetPlayerSpawnPositionNearDoor(RoomDoorTransition door)
  {
    if (door == null)
    {
      return player != null ? player.transform.position : Vector3.zero;
    }

    return door.GetPlayerSpawnPosition(GetCurrentRoomCenter(), playerSpawnOffset);
  }

  private Vector3 GetCurrentRoomCenter()
  {
    if (CurrentRoomInstance == null)
    {
      return Vector3.zero;
    }

    // Renderer bounds give a practical center point.
    Renderer[] renderers = CurrentRoomInstance.GetComponentsInChildren<Renderer>();
    if (renderers == null || renderers.Length == 0)
    {
      return CurrentRoomInstance.transform.position;
    }

    Bounds roomBounds = renderers[0].bounds;
    for (int i = 1; i < renderers.Length; i++)
    {
      roomBounds.Encapsulate(renderers[i].bounds);
    }

    return roomBounds.center;
  }

  private RoomSelectionSystem GetOrCreateRoomSelectionSystem()
  {
    if (roomSelectionSystem != null)
    {
      return roomSelectionSystem;
    }

    if (RoomSelectionSystem.Instance != null)
    {
      return RoomSelectionSystem.Instance;
    }

    RoomSelectionSystem existingRoomSelectionSystem = FindAnyObjectByType<RoomSelectionSystem>();
    if (existingRoomSelectionSystem != null)
    {
      return existingRoomSelectionSystem;
    }

    GameObject gameObject = new GameObject("Room Selection System");
    return gameObject.AddComponent<RoomSelectionSystem>();
  }
}
