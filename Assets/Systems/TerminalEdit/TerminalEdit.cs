using UnityEngine;
using UnityEngine.InputSystem;

public class TerminalEdit : MonoBehaviour, IInteractable
{
  public bool InteractOnContact { get { return false; } }

  [SerializeField] private GameObject equipmentMenu;
  [SerializeField] private string equipmentMenuObjectName = "Equipment Menu";
  [SerializeField] private string enemyLayerName = "Units";
  [SerializeField] private Unit[] enemiesToDefeat;
  [SerializeField] private SpriteRenderer terminalRenderer;
  [SerializeField] private Color unlockedHighlightColor = Color.cyan;
  [SerializeField] private Color lockedHighlightColor = Color.red;

  private GameObject spawnedEquipmentMenu;
  private Color defaultColor;
  private bool isHighlighted;

  private void Awake()
  {
    if (terminalRenderer == null)
    {
      terminalRenderer = GetComponentInChildren<SpriteRenderer>();
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

    HandleCloseEquipmentMenuInput();
  }

  private void Reset()
  {
    terminalRenderer = GetComponentInChildren<SpriteRenderer>();
  }

  public void Interact(PlayerCharacter player)
  {
    if (!CanUseTerminal())
    {
      Debug.Log("Terminal is locked. Defeat all enemies in the room first.");
      return;
    }

    OpenEquipmentMenu();
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
    if (HasAssignedEnemyAlive())
    {
      return false;
    }

    return !HasSceneEnemyAlive();
  }

  private bool HasAssignedEnemyAlive()
  {
    if (enemiesToDefeat == null)
    {
      return false;
    }

    foreach (Unit enemy in enemiesToDefeat)
    {
      if (IsActiveEnemy(enemy))
      {
        return true;
      }
    }

    return false;
  }

  private bool HasSceneEnemyAlive()
  {
    foreach (Unit unit in Resources.FindObjectsOfTypeAll<Unit>())
    {
      if (IsActiveEnemy(unit))
      {
        return true;
      }
    }

    return false;
  }

  private bool IsActiveEnemy(Unit unit)
  {
    if (unit == null || unit is PlayerCharacter)
    {
      return false;
    }

    if (!unit.gameObject.activeInHierarchy || !unit.gameObject.scene.IsValid() || !unit.gameObject.scene.isLoaded)
    {
      return false;
    }

    int enemyLayer = LayerMask.NameToLayer(enemyLayerName);
    if (enemyLayer == -1)
    {
      Debug.LogWarning($"Enemy layer '{enemyLayerName}' does not exist.");
      return false;
    }

    return unit.gameObject.layer == enemyLayer;
  }

  private void OpenEquipmentMenu()
  {
    GameObject menu = GetEquipmentMenu();
    if (menu == null)
    {
      Debug.LogWarning($"Equipment Menu is not assigned to TerminalEdit and no loaded scene object named '{equipmentMenuObjectName}' was found.");
      return;
    }

    menu.SetActive(true);

    if (menu.transform.localScale == Vector3.zero)
    {
      menu.transform.localScale = Vector3.one;
    }
  }

  private void CloseEquipmentMenu()
  {
    GameObject menu = GetEquipmentMenu();
    if (menu == null || !menu.activeSelf)
    {
      return;
    }

    menu.SetActive(false);
  }

  private void HandleCloseEquipmentMenuInput()
  {
    if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      return;
    }

    CloseEquipmentMenu();
  }

  private GameObject GetEquipmentMenu()
  {
    if (spawnedEquipmentMenu != null)
    {
      return spawnedEquipmentMenu;
    }

    if (equipmentMenu == null)
    {
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

  private void UpdateHighlightColor()
  {
    if (terminalRenderer == null)
    {
      return;
    }

    terminalRenderer.color = CanUseTerminal() ? unlockedHighlightColor : lockedHighlightColor;
  }
}
