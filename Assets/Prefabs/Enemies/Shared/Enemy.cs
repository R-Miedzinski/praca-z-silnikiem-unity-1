using UnityEngine;

// Abstrakcyjna klasa bazowa dla wszystkich wrogów w grze.
// Każdy konkretny wróg (np. MeleeEnemy) musi ją rozszerzyć i zaimplementować OnAttack().
//
// Diagram przejść między stanami:
//
//   Patrol  ──(gracz w zasięgu detekcji)──►  Chase
//   Chase   ──(gracz poza zasięgiem)──────►  Patrol
//   Chase   ──(gracz w zasięgu ataku)─────►  Attack
//   Attack  ──(gracz za daleko)───────────►  Chase
public abstract class Enemy : Unit
{
    // ── Protected ─────────────────────────────────────────────────────────────
    protected EEnemyState CurrentState { get; private set; } = EEnemyState.Patrol;

    // ── Private fields ────────────────────────────────────────────────────────
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float patrolWaypointTolerance = 0.3f;

    private PlayerCharacter player;
    private Vector2 spawnPosition;
    private Vector2 patrolTarget;

    // ── Events ────────────────────────────────────────────────────────────────
    public event System.Action OnDeath;

    // ── Lifecycle ─────────────────────────────────────────────────────────────
    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
        player = PlayerCharacter.Instance;
        spawnPosition = transform.position;
        patrolTarget = GetNewPatrolTarget();
    }

    private void Update()
    {
        TransitionState();
        ExecuteState();
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

    // ── Protected ─────────────────────────────────────────────────────────────
    protected virtual void OnPatrol()
    {
        MoveToward(patrolTarget);

        if (Vector2.Distance(transform.position, patrolTarget) < patrolWaypointTolerance)
            patrolTarget = GetNewPatrolTarget();
    }

    protected virtual void OnChase()
    {
        MoveToward(player.transform.position);
    }

    protected abstract void OnAttack();

    protected void MoveToward(Vector2 target)
    {
        if (!canMove)
            return;

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.Translate(direction * MovementSpeed * Time.deltaTime);
    }

    protected PlayerCharacter GetPlayer() => player;

    // ── Private ───────────────────────────────────────────────────────────────
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

    private void ExecuteState()
    {
        switch (CurrentState)
        {
            case EEnemyState.Patrol: OnPatrol(); break;
            case EEnemyState.Chase:  OnChase();  break;
            case EEnemyState.Attack: OnAttack(); break;
        }
    }

    private Vector2 GetNewPatrolTarget()
    {
        return spawnPosition + Random.insideUnitCircle * patrolRadius;
    }
}
