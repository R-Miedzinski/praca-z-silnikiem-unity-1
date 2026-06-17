using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdTerminalHandler : MonoBehaviour, ITerminalHandler
{
  [SerializeField] private GameObject hudObject;
  [SerializeField] private string hudObjectName = "HuD";

  private GameObject spawnedHudObject;
  private bool hudHiddenByTerminal;

  private void Update()
  {
    HandleRestoreHudInput();
  }

  public void HandleTerminal(TerminalData terminalData, PlayerCharacter player)
  {
    HideHud();
  }

  private void HideHud()
  {
    GameObject hud = GetHudObject();
    if (hud == null)
    {
      Debug.LogWarning($"{nameof(ThirdTerminalHandler)} cannot hide HUD because no assigned object or loaded scene object named '{hudObjectName}' was found.", this);
      return;
    }

    hud.SetActive(false);
    hudHiddenByTerminal = true;
  }

  private void ShowHud()
  {
    GameObject hud = GetHudObject();
    if (hud == null)
    {
      Debug.LogWarning($"{nameof(ThirdTerminalHandler)} cannot restore HUD because no assigned object or loaded scene object named '{hudObjectName}' was found.", this);
      return;
    }

    hud.SetActive(true);
    hudHiddenByTerminal = false;
  }

  private void HandleRestoreHudInput()
  {
    if (!hudHiddenByTerminal || Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      return;
    }

    ShowHud();
  }

  private GameObject GetHudObject()
  {
    if (spawnedHudObject != null)
    {
      return spawnedHudObject;
    }

    if (hudObject == null)
    {
      hudObject = FindHudObjectInLoadedScenes();
    }

    if (hudObject == null)
    {
      return null;
    }

    if (hudObject.scene.IsValid() && hudObject.scene.isLoaded)
    {
      return hudObject;
    }

    spawnedHudObject = Instantiate(hudObject);
    return spawnedHudObject;
  }

  private GameObject FindHudObjectInLoadedScenes()
  {
    foreach (GameObject candidate in Resources.FindObjectsOfTypeAll<GameObject>())
    {
      if (candidate.name != hudObjectName)
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
