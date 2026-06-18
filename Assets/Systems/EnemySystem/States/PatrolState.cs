using UnityEngine;

public class PatrolState : EnemyState
{
    private Vector2 patrolTarget;
    private bool initialized = false;

    public override void Execute(Enemy enemy)
    {
        if (!initialized)
        {
            patrolTarget = enemy.GetRandomPatrolTarget();
            initialized = true;
        }

        enemy.MoveToward(patrolTarget);

        if (Vector2.Distance(enemy.transform.position, patrolTarget) < enemy.PatrolWaypointTolerance)
            patrolTarget = enemy.GetRandomPatrolTarget();
    }
}
