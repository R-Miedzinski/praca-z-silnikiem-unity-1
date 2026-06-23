using UnityEngine;

// Abstrakcyjna klasa bazowa dla wszystkich wrogów.
// Graf stanów (Patrol/Chase/Attack i dowolne nowe) budowany jest w BuildStateGraph().
// Żeby stworzyć wroga bez któregoś ze stanów albo z nowymi — wystarczy nadpisać BuildStateGraph().
public abstract class Enemy : Unit
{
    // ── Protected ─────────────────────────────────────────────────────────────
    protected EnemyState currentState;

    // ── Private fields ────────────────────────────────────────────────────────
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float patrolWaypointTolerance = 0.3f;

    private PlayerCharacter player;
    private Vector2 spawnPosition;

    // ── Events ────────────────────────────────────────────────────────────────
    public event System.Action OnDeath;

    // ── Properties (dla stanów) ───────────────────────────────────────────────
    public float PatrolWaypointTolerance => patrolWaypointTolerance;

    // ── Lifecycle ─────────────────────────────────────────────────────────────
    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
        player = PlayerCharacter.Instance;
        spawnPosition = transform.position;
        currentState = BuildStateGraph();
    }

    private void Update()
    {
        var next = currentState.TryGetNextState();
        if (next != null)
        {
            currentState.Exit(this);
            currentState = next;
            currentState.Enter(this);
        }
        currentState.Execute(this);
    }

    // ── Public ────────────────────────────────────────────────────────────────
    public override void Heal(float amount)
    {
        CurrentHealth += amount;
    }

    public override void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public void MoveToward(Vector2 target)
    {
        if (!canMove)
            return;

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.Translate(direction * MovementSpeed * Time.deltaTime);
    }

    public PlayerCharacter GetPlayer() => player;

    public Vector2 GetRandomPatrolTarget()
    {
        return spawnPosition + Random.insideUnitCircle * patrolRadius;
    }

    public abstract void PerformAttack();

    // ── Protected ─────────────────────────────────────────────────────────────

    // Buduje domyślny graf stanów: Patrol ↔ Chase ↔ Attack.
    // Nadpisz tę metodę żeby dodać nowe stany lub usunąć istniejące.
    protected virtual EnemyState BuildStateGraph()
    {
        var patrol = new PatrolState();
        var chase = new ChaseState();
        var attack = new AttackState();

        patrol.AddTransition(chase, () => DistanceToPlayer() <= detectionRange);
        chase.AddTransition(patrol, () => DistanceToPlayer() > detectionRange);
        chase.AddTransition(attack, () => DistanceToPlayer() <= attackRange);
        attack.AddTransition(chase, () => DistanceToPlayer() > attackRange);

        return patrol;
    }

    // ── Private ───────────────────────────────────────────────────────────────
    private float DistanceToPlayer() => Vector2.Distance(transform.position, player.transform.position);
}
