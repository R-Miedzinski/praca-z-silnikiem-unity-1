using UnityEngine;

public class TerminalEdit : MonoBehaviour, IInteractable
{
  public static event System.Action<TerminalData, PlayerCharacter> TerminalInteracted;

  public bool InteractOnContact { get { return false; } }

  [SerializeField] private TerminalData terminalData;
  [SerializeField] private RoomSceneManager sceneRoomMenago;
  [SerializeField] private SpriteRenderer terminalRenderer;
  [SerializeField] private Color unlockedHighlightColor = Color.cyan;
  [SerializeField] private Color lockedHighlightColor = Color.red;

  private Color defaultColor;
  private bool isHighlighted;

  public TerminalData TerminalData { get { return terminalData; } }

  private void Awake()
  {
    if (terminalRenderer == null)
    {
      terminalRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    if (sceneRoomMenago == null)
    {
      // Enemy checks.
      sceneRoomMenago = RoomSceneManager.GetOrCreate();
    }

    if (terminalRenderer != null)
    {
      defaultColor = terminalRenderer.color;
    }
  }

  private void Update()
  {
    if (isHighlighted)
    {
      UpdateHighlightColor();
    }
  }

  private void Reset()
  {
    terminalRenderer = GetComponentInChildren<SpriteRenderer>();
    sceneRoomMenago = FindAnyObjectByType<RoomSceneManager>();
  }

  public void Interact(PlayerCharacter player)
  {
    if (!CanUseTerminal())
    {
      Debug.Log("Terminal is locked. Defeat all enemies in the room first.");
      return;
    }

    if (terminalData == null)
    {
      Debug.LogWarning("TerminalEdit cannot identify the terminal because TerminalData is not assigned.", this);
    }

    if (player == null)
    {
      Debug.LogWarning("TerminalEdit was interacted with without a PlayerCharacter reference.", this);
    }

    System.Action<TerminalData, PlayerCharacter> terminalInteracted = TerminalInteracted;
    if (terminalInteracted == null)
    {
      Debug.LogWarning("TerminalEdit has no TerminalSwitchSystem subscriber for terminal interaction events.", this);
      return;
    }

    terminalInteracted.Invoke(terminalData, player);
  }

  public void EnableHighlight()
  {
    isHighlighted = true;
    UpdateHighlightColor();
  }

  public void DisableHighlight()
  {
    isHighlighted = false;

    if (terminalRenderer != null)
    {
      terminalRenderer.color = defaultColor;
    }
  }

  private bool CanUseTerminal()
  {
    return sceneRoomMenago == null || !sceneRoomMenago.HasAliveEnemyInRoom();
  }

  private void UpdateHighlightColor()
  {
    if (terminalRenderer == null)
    {
      return;
    }

    terminalRenderer.color = CanUseTerminal() ? unlockedHighlightColor : lockedHighlightColor;
  }
}
