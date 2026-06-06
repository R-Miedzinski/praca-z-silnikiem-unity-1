using System.Collections.Generic;
using UnityEngine;

// Loads rooms from RoomOrder and moves the player between them.
public class DoorRoomTransitionSystem : MonoBehaviour
{
  public enum RoomDoorSide
  {
    Left,
    Bottom,
    Forward
  }

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

  public bool IsEntranceDoor(RoomDoorTransition door)
  {
    RoomDoorSide doorSide = GetDoorSide(door);
    return doorSide == RoomDoorSide.Left || doorSide == RoomDoorSide.Bottom;
  }

  public RoomDoorSide GetDoorSide(RoomDoorTransition door)
  {
    if (door == null || CurrentRoomInstance == null)
    {
      return RoomDoorSide.Forward;
    }

    RoomDoorTransition[] roomDoors = CurrentRoomInstance.GetComponentsInChildren<RoomDoorTransition>();
    if (roomDoors.Length == 0)
    {
      return RoomDoorSide.Forward;
    }

    Vector3 doorPosition = CurrentRoomInstance.transform.InverseTransformPoint(door.transform.position);
    float minX = doorPosition.x;
    float minY = doorPosition.y;

    foreach (RoomDoorTransition roomDoor in roomDoors)
    {
      Vector3 roomDoorPosition = CurrentRoomInstance.transform.InverseTransformPoint(roomDoor.transform.position);
      minX = Mathf.Min(minX, roomDoorPosition.x);
      minY = Mathf.Min(minY, roomDoorPosition.y);
    }

    if (Mathf.Approximately(doorPosition.x, minX))
    {
      return RoomDoorSide.Left;
    }

    if (Mathf.Approximately(doorPosition.y, minY))
    {
      return RoomDoorSide.Bottom;
    }

    return RoomDoorSide.Forward;
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

    RoomDoorTransition bottomDoor = null;
    foreach (RoomDoorTransition door in CurrentRoomInstance.GetComponentsInChildren<RoomDoorTransition>())
    {
      RoomDoorSide doorSide = GetDoorSide(door);
      if (doorSide == RoomDoorSide.Left)
      {
        return door;
      }

      if (doorSide == RoomDoorSide.Bottom)
      {
        bottomDoor = door;
      }
    }

    return bottomDoor;
  }

  private Vector3 GetPlayerSpawnPositionNearDoor(RoomDoorTransition door)
  {
    RoomDoorSide doorSide = GetDoorSide(door);
    Vector3 offset = Vector3.zero;

    if (doorSide == RoomDoorSide.Left)
    {
      offset = Vector3.right * playerSpawnOffset;
    }
    else if (doorSide == RoomDoorSide.Bottom)
    {
      offset = Vector3.up * playerSpawnOffset;
    }

    return door.transform.position + offset;
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
