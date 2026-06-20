using UnityEngine;

// Door component that asks DoorRoomTransitionSystem to load the next room.
public class RoomDoorTransition : MonoBehaviour, IInteractable
{
  public bool InteractOnContact { get { return true; } }

  [SerializeField] private DoorRoomTransitionSystem transitionSystem;
  [SerializeField] private SceneRoomMenago sceneRoomMenago;
  [SerializeField] private SpriteRenderer highlightRenderer;
  [SerializeField] private Color highlightColor = Color.yellow;
  [SerializeField]
  [Tooltip("If enabled, this door is used as the room entrance and cannot move the player to the next room. If disabled, this door is an exit.")]
  private bool isEntranceDoor;
  [SerializeField]
  [Tooltip("Optional exact player spawn point used when entering this room through this entrance door.")]
  private Transform playerSpawnPoint;
  [SerializeField]
  [Tooltip("Distance from this entrance door toward the inside of the room when Player Spawn Point is not assigned.")]
  private float insideSpawnOffset = 2f;

  private Color defaultColor;

  public bool IsEntranceDoor { get { return isEntranceDoor; } }

  private void OnEnable()
  {
    if (transitionSystem == null)
    {
      transitionSystem = DoorRoomTransitionSystem.Instance;
    }

    if (sceneRoomMenago == null)
    {
      sceneRoomMenago = SceneRoomMenago.GetOrCreate();
    }

    if (highlightRenderer == null)
    {
      highlightRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    if (highlightRenderer != null)
    {
      defaultColor = highlightRenderer.color;
    }
  }

  private void OnValidate()
  {
    insideSpawnOffset = Mathf.Max(0f, insideSpawnOffset);
  }

  public void Interact(PlayerCharacter player)
  {
    if (!CanTransition())
    {
      return;
    }

    // The door only requests the transition.
    // The scene system owns room loading.
    transitionSystem.LoadNextRoom(player);
  }

  public void EnableHighlight()
  {
    if (highlightRenderer != null)
    {
      highlightRenderer.color = highlightColor;
    }
  }

  public void DisableHighlight()
  {
    if (highlightRenderer != null)
    {
      highlightRenderer.color = defaultColor;
    }
  }

  private bool CanTransition()
  {
    if (transitionSystem == null)
    {
      transitionSystem = FindAnyObjectByType<DoorRoomTransitionSystem>();
    }

    if (transitionSystem == null)
    {
      Debug.LogWarning("RoomDoorTransition cannot change rooms because DoorRoomTransitionSystem was not found.");
      return false;
    }

    if (isEntranceDoor)
    {
      Debug.Log("This door is an entrance door and does not lead to the next room.");
      return false;
    }

    if (sceneRoomMenago == null)
    {
      sceneRoomMenago = SceneRoomMenago.GetOrCreate();
    }

    if (sceneRoomMenago.HasAliveEnemyInRoom())
    {
      Debug.Log("Door is locked. Defeat all enemies in the room first.");
      return false;
    }

    return true;
  }

  public Vector3 GetPlayerSpawnPosition(Vector3 roomCenter, float defaultSpawnOffset)
  {
    if (playerSpawnPoint != null)
    {
      return playerSpawnPoint.position;
    }

    float spawnOffset = insideSpawnOffset > 0f ? insideSpawnOffset : defaultSpawnOffset;
    Vector3 insideDirection = roomCenter - transform.position;
    insideDirection.z = 0f;

    if (insideDirection.sqrMagnitude <= Mathf.Epsilon)
    {
      // Fallback where the door and room center overlap.
      insideDirection = transform.right;
    }

    return transform.position + insideDirection.normalized * spawnOffset;
  }
}
