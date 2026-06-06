using UnityEngine;

// ScriptableObject definition for a room available in the project.
// Stores editor-friendly metadata and a prefab reference used by room selection and spawning systems.
[CreateAssetMenu(fileName = "RoomDataObject", menuName = "Rooms/RoomDataObject")]
public class RoomDataObject : ScriptableObject
{
  [Tooltip("Unique identifier for the room.")]
  public string Id;
  [Tooltip("Display name shown in editor tools and gameplay UI.")]
  public string RoomName;
  [Tooltip("Short readable description of the room purpose or theme.")]
  [TextArea(2, 4)]
  public string ShortDescription;
  [Tooltip("Prefab used to spawn or preview this room.")]
  public GameObject RoomPrefab;
  [Tooltip("Room size measured in visible grid units.")]
  public Vector2Int GridSize;
  [Tooltip("Default player spawn point relative to the room prefab origin.")]
  public Vector2 DefaultSpawnPoint;
  [Tooltip("Allows this room to be used by room selection or generation systems.")]
  public bool AvailableInRoomPool = true;
}
