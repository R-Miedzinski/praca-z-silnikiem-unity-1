using System.Collections;
using UnityEngine;

// Spawnuje wrogów w podanych punktach i opcjonalnie respawnuje ich po śmierci.
// Użycie: dodaj komponent do pustego GameObject w scenie, przypisz prefab wroga
// i utwórz child GameObject-y jako punkty spawnu.
public class EnemySpawner : MonoBehaviour
{
    // Prefab wroga który ma być spawnowany (musi mieć komponent dziedziczący po Enemy).
    [SerializeField] private GameObject enemyPrefab;

    // Lista punktów spawnu. Każdy to Transform w scenie — pozycja tego obiektu
    // wyznacza gdzie pojawi się wróg.
    [SerializeField] private Transform[] spawnPoints;

    // Czas w sekundach po którym wróg odrodzi się po śmierci.
    // Ustaw 0 żeby wyłączyć respawn.
    [SerializeField] private float respawnDelay = 5f;

    private void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnAt(spawnPoint);
        }
    }

    private void SpawnAt(Transform spawnPoint)
    {
        GameObject go = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Próbujemy pobrać komponent Enemy żeby subskrybować zdarzenie śmierci.
        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogWarning($"[EnemySpawner] Prefab '{enemyPrefab.name}' nie ma komponentu dziedziczącego po Enemy. Respawn nie zadziała.");
            return;
        }

        if (respawnDelay > 0f)
        {
            // Lambdy w C# przechwytują zmienne przez referencję —
            // dlatego przekazujemy spawnPoint jako parametr zamiast użyć go bezpośrednio w lambdzie,
            // żeby uniknąć potencjalnych problemów w pętlach.
            enemy.OnDeath += () => StartCoroutine(RespawnAfterDelay(spawnPoint));
        }
    }

    // Czeka respawnDelay sekund, potem spawnuje nowego wroga w tym samym miejscu.
    private IEnumerator RespawnAfterDelay(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnAt(spawnPoint);
    }
}
