using UnityEngine;

// Defines how a door chooses the next room from DoorRoomTransitionSystem.
public enum RoomDoorTravelDirection
{
  Next,
  Previous,
  SpecificRoomId
}

// Component added directly to a door prefab or door object.
// It activates DoorRoomTransitionSystem through the existing IInteractable flow.
public class RoomDoorTransition : MonoBehaviour, IInteractable
{
  [SerializeField] private RoomDoorTravelDirection travelDirection = RoomDoorTravelDirection.Next;
  [SerializeField] private string targetRoomId;
  [SerializeField] private bool interactOnContact = false;
  [SerializeField] private Vector2 targetSpawnOffset;
  [SerializeField] private RoomSpawnDoorSide targetSpawnDoorSide = RoomSpawnDoorSide.LeftOrBottom;
  [SerializeField] private SpriteRenderer doorRenderer;
  [SerializeField] private Color highlightedColor = Color.cyan;

  public bool InteractOnContact { get { return interactOnContact; } }

  private Color defaultColor;

  // Caches the renderer so the door can show highlight feedback during interaction.
  private void Awake()
  {
    if (doorRenderer == null)
    {
      doorRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    if (doorRenderer != null)
    {
      defaultColor = doorRenderer.color;
    }
  }

  // Called by InteractionCollider when the player uses or touches the door.
  public void Interact(PlayerCharacter player)
  {
    DoorRoomTransitionSystem transitionSystem = DoorRoomTransitionSystem.Instance;
    if (transitionSystem == null)
    {
      Debug.LogWarning("RoomDoorTransition needs DoorRoomTransitionSystem in the scene.");
      return;
    }

    if (!transitionSystem.CanUseDoorAsExit(this))
    {
      return;
    }

    switch (travelDirection)
    {
      case RoomDoorTravelDirection.Previous:
        transitionSystem.MoveToPreviousRoom(player, targetSpawnOffset, targetSpawnDoorSide);
        break;
      case RoomDoorTravelDirection.SpecificRoomId:
        transitionSystem.MoveToRoomById(targetRoomId, player, targetSpawnOffset, targetSpawnDoorSide);
        break;
      default:
        transitionSystem.MoveToNextRoom(player, targetSpawnOffset, targetSpawnDoorSide);
        break;
    }
  }

  // Marks the door as interactable when the player is close enough.
  public void EnableHighlight()
  {
    if (doorRenderer != null)
    {
      doorRenderer.color = highlightedColor;
    }
  }

  // Restores the door color after the player leaves interaction range.
  public void DisableHighlight()
  {
    if (doorRenderer != null)
    {
      doorRenderer.color = defaultColor;
    }
  }
}
