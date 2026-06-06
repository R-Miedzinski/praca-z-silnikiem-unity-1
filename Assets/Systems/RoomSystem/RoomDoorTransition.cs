using UnityEngine;

// Door component that asks DoorRoomTransitionSystem to load the next room.
public class RoomDoorTransition : MonoBehaviour, IInteractable
{
  public bool InteractOnContact { get { return true; } }

  [SerializeField] private DoorRoomTransitionSystem transitionSystem;
  [SerializeField] private SceneRoomMenago sceneRoomMenago;
  [SerializeField] private SpriteRenderer highlightRenderer;
  [SerializeField] private Color highlightColor = Color.yellow;

  private Color defaultColor;

  private void Awake()
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

    if (transitionSystem.IsEntranceDoor(this))
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
}
