using UnityEngine;

public class EquipmentTerminalHandler : MonoBehaviour, ITerminalHandler
{
  [SerializeField] private GameObject equipmentMenu;
  [SerializeField] private string equipmentMenuObjectName = "Equipment Menu";

  private GameObject spawnedEquipmentMenu;

  public void HandleTerminal(TerminalData terminalData, PlayerCharacter player)
  {
    OpenEquipmentMenu();
  }

  private void OpenEquipmentMenu()
  {
    GameObject menu = GetEquipmentMenu();
    if (menu == null)
    {
      Debug.LogWarning($"Equipment Menu is not assigned to EquipmentTerminalHandler and no loaded scene object named '{equipmentMenuObjectName}' was found.", this);
      return;
    }

    menu.SetActive(true);

    if (menu.transform.localScale == Vector3.zero)
    {
      menu.transform.localScale = Vector3.one;
    }
  }

  private GameObject GetEquipmentMenu()
  {
    if (spawnedEquipmentMenu != null)
    {
      return spawnedEquipmentMenu;
    }

    if (equipmentMenu == null)
    {
      // Allows the handler use a menu already placed in the scene.
      equipmentMenu = FindEquipmentMenuInLoadedScenes();
    }

    if (equipmentMenu == null)
    {
      return null;
    }

    if (equipmentMenu.scene.IsValid() && equipmentMenu.scene.isLoaded)
    {
      return equipmentMenu;
    }

    spawnedEquipmentMenu = Instantiate(equipmentMenu);
    return spawnedEquipmentMenu;
  }

  private GameObject FindEquipmentMenuInLoadedScenes()
  {
    foreach (GameObject candidate in Resources.FindObjectsOfTypeAll<GameObject>())
    {
      if (candidate.name != equipmentMenuObjectName)
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
