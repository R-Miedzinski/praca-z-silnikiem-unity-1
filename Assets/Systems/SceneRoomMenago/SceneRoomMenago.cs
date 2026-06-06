using UnityEngine;

// Shared room manager that checks if enemies are still alive.
public class SceneRoomMenago : MonoBehaviour
{
  [SerializeField] private string enemyLayerName = "Units";
  [SerializeField] private Unit[] enemiesInRoom;

  public bool HasAliveEnemyInRoom()
  {
    // Assigned enemies are checked first, then the loaded scene is scanned.
    if (HasAssignedEnemyAlive())
    {
      return true;
    }

    return HasSceneEnemyAlive();
  }

  public bool IsAliveEnemy(Unit unit)
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

  private bool HasAssignedEnemyAlive()
  {
    if (enemiesInRoom == null)
    {
      return false;
    }

    foreach (Unit enemy in enemiesInRoom)
    {
      if (IsAliveEnemy(enemy))
      {
        return true;
      }
    }

    return false;
  }

  private bool HasSceneEnemyAlive()
  {
    // Find all loaded Units.
    foreach (Unit unit in Resources.FindObjectsOfTypeAll<Unit>())
    {
      if (IsAliveEnemy(unit))
      {
        return true;
      }
    }

    return false;
  }

  public static SceneRoomMenago GetOrCreate()
  {
    // Hier script ask about scene.
    SceneRoomMenago sceneRoomMenago = FindAnyObjectByType<SceneRoomMenago>();
    if (sceneRoomMenago != null)
    {
      return sceneRoomMenago;
    }

    GameObject gameObject = new GameObject("Scene Room Menago");
    return gameObject.AddComponent<SceneRoomMenago>();
  }
}
