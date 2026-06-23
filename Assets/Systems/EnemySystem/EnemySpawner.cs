using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float respawnDelay = 5f;

    private readonly List<(Enemy enemy, System.Action handler)> activeEnemies = new List<(Enemy, System.Action)>();

    private void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnAt(spawnPoint);
        }
    }

    private void OnDestroy()
    {
        foreach (var (enemy, handler) in activeEnemies)
        {
            if (enemy != null)
                enemy.OnDeath -= handler;
        }
    }

    private void SpawnAt(Transform spawnPoint)
    {
        GameObject go = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogWarning($"[EnemySpawner] Prefab '{enemyPrefab.name}' nie ma komponentu dziedziczącego po Enemy. Respawn nie zadziała.");
            return;
        }

        if (respawnDelay > 0f)
        {
            System.Action handler = null;
            handler = () =>
            {
                activeEnemies.RemoveAll(e => e.enemy == enemy);
                StartCoroutine(RespawnAfterDelay(spawnPoint));
            };
            activeEnemies.Add((enemy, handler));
            enemy.OnDeath += handler;
        }
    }

    private IEnumerator RespawnAfterDelay(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnAt(spawnPoint);
    }
}
