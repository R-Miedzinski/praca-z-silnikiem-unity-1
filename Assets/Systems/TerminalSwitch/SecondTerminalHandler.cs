using UnityEngine;
using UnityEngine.InputSystem;

public class SecondTerminalHandler : MonoBehaviour, ITerminalHandler
{
  [SerializeField] private GameObject pauseMenu;
  [SerializeField] private string pauseMenuObjectName = "Pause Menu";

  private GameObject spawnedPauseMenu;

  public void HandleTerminal(TerminalData terminalData, PlayerCharacter player)
  {
    OpenPauseMenu();
  }

  private void OpenPauseMenu()
  {
    GameObject menu = GetPauseMenu();
    if (menu == null)
    {
      Debug.LogWarning($"{nameof(SecondTerminalHandler)} cannot open Pause Menu because no assigned object or loaded scene object named '{pauseMenuObjectName}' was found.", this);
      return;
    }

    menu.SetActive(true);

    if (menu.transform.localScale == Vector3.zero)
    {
      menu.transform.localScale = Vector3.one;
    }
  }

  private GameObject GetPauseMenu()
  {
    if (spawnedPauseMenu != null)
    {
      return spawnedPauseMenu;
    }

    if (pauseMenu == null)
    {
      pauseMenu = FindPauseMenuInLoadedScenes();
    }

    if (pauseMenu == null)
    {
      return null;
    }

    if (pauseMenu.scene.IsValid() && pauseMenu.scene.isLoaded)
    {
      return pauseMenu;
    }

    spawnedPauseMenu = Instantiate(pauseMenu);
    return spawnedPauseMenu;
  }

  private GameObject FindPauseMenuInLoadedScenes()
  {
    foreach (GameObject candidate in Resources.FindObjectsOfTypeAll<GameObject>())
    {
      if (candidate.name != pauseMenuObjectName)
      {
        continue;
      }

      if (!candidate.scene.IsValid() || !candidate.scene.isLoaded)
      {
        continue;
      }

      return candidate;
    }

    return null;
  }
}
