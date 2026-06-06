using UnityEngine;

[CreateAssetMenu(fileName = "RoomDataObject", menuName = "Rooms/RoomDataObject")]
public class RoomDataObject : ScriptableObject
{
  // Data used by room selection and room spawning systems.
  [Tooltip("Unique identifier for the room.")]
  public string Id;
  public string RoomName;
  public string Description;
  public GameObject RoomPrefab;
  public Vector2Int GridSize;
  public Vector3 SpawnPoint;
}
