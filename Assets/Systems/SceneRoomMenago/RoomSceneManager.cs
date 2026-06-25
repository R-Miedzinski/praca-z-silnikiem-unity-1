using System.Collections.Generic;
using UnityEngine;

// Shared room manager that checks if enemies are still alive.
public class RoomSceneManager : MonoBehaviour
{
  public static RoomSceneManager Instance { get; private set; }

  private readonly HashSet<EnemyRoomMember> aliveEnemies = new HashSet<EnemyRoomMember>();
  private bool isSubscribed;

  public int AliveEnemyCount { get { return aliveEnemies.Count; } }

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(this);
      return;
    }

    Instance = this;
  }

  private void OnEnable()
  {
    if (Instance != this)
    {
      return;
    }

    // Apear and disappear.
    EnemyRoomMember.EnemyAppeared += HandleEnemyAppeared;
    EnemyRoomMember.EnemyDisappeared += HandleEnemyDisappeared;
    isSubscribed = true;

    // Covers enemies that were enabled.
    RefreshAliveEnemies();
  }

  private void OnDisable()
  {
    if (!isSubscribed)
    {
      return;
    }

    EnemyRoomMember.EnemyAppeared -= HandleEnemyAppeared;
    EnemyRoomMember.EnemyDisappeared -= HandleEnemyDisappeared;
    isSubscribed = false;

    aliveEnemies.Clear();
  }

  private void OnDestroy()
  {
    if (Instance == this)
    {
      Instance = null;
    }
  }

  public bool HasAliveEnemyInRoom()
  {
    return aliveEnemies.Count > 0;
  }

  private void HandleEnemyAppeared(EnemyRoomMember enemy)
  {
    RegisterEnemy(enemy);
  }

  private void HandleEnemyDisappeared(EnemyRoomMember enemy)
  {
    aliveEnemies.Remove(enemy);
  }

  private void RefreshAliveEnemies()
  {
    aliveEnemies.Clear();

    foreach (EnemyRoomMember enemy in FindObjectsByType<EnemyRoomMember>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
    {
      RegisterEnemy(enemy);
    }
  }

  private void RegisterEnemy(EnemyRoomMember enemy)
  {
    if (enemy == null || !enemy.isActiveAndEnabled)
    {
      return;
    }

    aliveEnemies.Add(enemy);
  }

  public static RoomSceneManager GetOrCreate()
  {
    if (Instance != null)
    {
      return Instance;
    }

    // Reuse a manager already placed in the scene.
    RoomSceneManager sceneRoomMenago = FindAnyObjectByType<RoomSceneManager>();
    if (sceneRoomMenago != null)
    {
      Instance = sceneRoomMenago;
      return sceneRoomMenago;
    }

    GameObject gameObject = new GameObject("Scene Room Menago");
    return gameObject.AddComponent<RoomSceneManager>();
  }
}
