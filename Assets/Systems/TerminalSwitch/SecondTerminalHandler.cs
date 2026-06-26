using UnityEngine;
using UnityEngine.InputSystem;

public class SecondTerminalHandler : MonoBehaviour, ITerminalHandler
{
  [SerializeField] private UIManager uiManager;

  public void HandleTerminal(TerminalData terminalData, PlayerCharacter player)
  {
    OpenPauseMenu();
  }

  private void OpenPauseMenu()
  {
    UIManager uiManager = GetUIManager();
    if (uiManager == null)
    {
      Debug.LogWarning($"{nameof(SecondTerminalHandler)} cannot open Pause Menu because no UIManager is assigned or instantiated.", this);
      return;
    }

    uiManager.OnMenuClick();
  }

  private UIManager GetUIManager()
  {
    if (uiManager != null)
    {
      return uiManager;
    }

    if (uiManager == null)
    {
      uiManager = UIManager.Instance;
    }

    return uiManager;
  }
}
