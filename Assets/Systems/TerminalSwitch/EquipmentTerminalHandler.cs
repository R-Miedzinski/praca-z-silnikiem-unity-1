using UnityEngine;

public class EquipmentTerminalHandler : MonoBehaviour, ITerminalHandler
{
  [SerializeField] private UIManager uiManager;

  public void HandleTerminal(TerminalData terminalData, PlayerCharacter player)
  {
    OpenEquipmentMenu();
  }

  private void OpenEquipmentMenu()
  {
    UIManager uiManager = GetUIManager();
    if (uiManager == null)
    {
      Debug.LogWarning($"{nameof(EquipmentTerminalHandler)} cannot open Equipment Menu because no UIManager is assigned or instantiated.", this);
      return;
    }

    uiManager.OnBackpackClick();
  }

  private UIManager GetUIManager()
  {
    if (uiManager != null)
    {
      return uiManager;
    }

    if (uiManager == null)
    {
      // Allows the handler to use a UIManager instance already placed in the scene.
      uiManager = UIManager.Instance;
    }

    return uiManager;
  }
}
