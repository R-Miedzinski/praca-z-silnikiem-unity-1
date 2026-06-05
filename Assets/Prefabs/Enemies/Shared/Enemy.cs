using UnityEngine;

// Abstrakcyjna klasa bazowa dla wszystkich wrogów w grze.
// "Abstrakcyjna" oznacza że nie można jej bezpośrednio użyć jako komponentu —
// każdy konkretny wróg (np. MeleeEnemy) musi ją rozszerzyć i zaimplementować OnAttack().
//
// Diagram przejść między stanami:
//
//   Patrol  ──(gracz w zasięgu detekcji)──►  Chase
//   Chase   ──(gracz poza zasięgiem)──────►  Patrol
//   Chase   ──(gracz w zasięgu ataku)─────►  Attack
//   Attack  ──(gracz za daleko)───────────►  Chase
public abstract class Enemy : Unit
{
    // Wartości widoczne i edytowalne w Unity Inspectorze (prawy panel edytora).
    // Dzięki [SerializeField] możemy zmieniać je dla każdego prefaba osobno bez dotykania kodu.
    [SerializeField] private float detectionRange = 8f;            // zasięg wykrycia gracza
    [SerializeField] private float attackRange = 2f;               // zasięg ataku
    [SerializeField] private float patrolRadius = 5f;              // jak daleko od miejsca spawnu może patrolować
    [SerializeField] private float patrolWaypointTolerance = 0.3f; // jak blisko celu uznajemy że "dotarliśmy"

    // Aktualny stan wroga. Tylko ta klasa może go zmieniać (private set),
    // ale klasy pochodne mogą go odczytywać (protected).
    protected EEnemyState CurrentState { get; private set; } = EEnemyState.Patrol;

    private PlayerCharacter player;
    private Vector2 spawnPosition;   // zapamiętana pozycja spawnu — centrum obszaru patrolowania
    private Vector2 patrolTarget;    // aktualny cel podczas patrolowania

    // "virtual" — klasy pochodne mogą nadpisać tę metodę (override),
    // ale jeśli tego nie zrobią, wykona się ta domyślna wersja.
    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
        player = PlayerCharacter.Instance;   // pobieramy singleton gracza
        spawnPosition = transform.position;
        patrolTarget = GetNewPatrolTarget();
    }

    // Update jest wywoływany przez Unity raz na każdą klatkę (~60 razy na sekundę).
    private void Update()
    {
        TransitionState();  // najpierw sprawdź czy stan powinien się zmienić
        ExecuteState();     // potem wykonaj logikę aktualnego stanu
    }

    // Sprawdza warunki przejścia i zmienia CurrentState jeśli trzeba.
    // Wywoływana co klatkę przed ExecuteState.
    private void TransitionState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        switch (CurrentState)
        {
            case EEnemyState.Patrol:
                if (distanceToPlayer <= detectionRange)
                    CurrentState = EEnemyState.Chase;
                break;

            case EEnemyState.Chase:
                if (distanceToPlayer > detectionRange)
                    CurrentState = EEnemyState.Patrol;
                else if (distanceToPlayer <= attackRange)
                    CurrentState = EEnemyState.Attack;
                break;

            case EEnemyState.Attack:
                if (distanceToPlayer > attackRange)
                    CurrentState = EEnemyState.Chase;
                break;
        }
    }

    // Wywołuje odpowiednią metodę On* dla aktualnego stanu.
    private void ExecuteState()
    {
        switch (CurrentState)
        {
            case EEnemyState.Patrol:
                OnPatrol();
                break;
            case EEnemyState.Chase:
                OnChase();
                break;
            case EEnemyState.Attack:
                OnAttack();
                break;
        }
    }

    // Domyślna logika patrolowania — ruch do losowego punktu w promieniu patrolRadius,
    // po dotarciu losuje nowy cel.
    protected virtual void OnPatrol()
    {
        MoveToward(patrolTarget);

        if (Vector2.Distance(transform.position, patrolTarget) < patrolWaypointTolerance)
            patrolTarget = GetNewPatrolTarget();
    }

    // Domyślna logika gonienia — ruch prosto w kierunku gracza.
    protected virtual void OnChase()
    {
        MoveToward(player.transform.position);
    }

    // Każdy konkretny wróg MUSI zaimplementować tę metodę — stąd "abstract".
    // Tu umieszczasz logikę ataku specyficzną dla danego typu wroga.
    protected abstract void OnAttack();

    public override void Heal(float amount)
    {
        CurrentHealth += amount;
    }

    // Subskrybuj to zdarzenie żeby reagować na śmierć wroga (np. w EnemySpawner).
    public event System.Action OnDeath;

    public override void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    // Przesuwa wroga w kierunku podanego punktu z prędkością MovementSpeed.
    // Sprawdza canMove — efekty takie jak slow lub immobilizacja mogą to zablokować.
    protected void MoveToward(Vector2 target)
    {
        if (!canMove)
            return;

        // .normalized zamienia wektor na jednostkowy (długość = 1) żeby prędkość była stała
        // niezależnie od odległości od celu
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.Translate(direction * MovementSpeed * Time.deltaTime);
    }

    protected PlayerCharacter GetPlayer() => player;

    // Losuje punkt wewnątrz koła o promieniu patrolRadius wokół miejsca spawnu.
    private Vector2 GetNewPatrolTarget()
    {
        return spawnPosition + Random.insideUnitCircle * patrolRadius;
    }
}
