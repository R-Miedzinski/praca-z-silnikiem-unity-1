using UnityEngine;

public class PatrolState : EnemyState
{
    private Vector2 patrolTarget;

    public override void Enter(Enemy enemy)
    {
        patrolTarget = enemy.GetRandomPatrolTarget();
    }

    public override void Execute(Enemy enemy)
    {
        enemy.MoveToward(patrolTarget);

        if (Vector2.Distance(enemy.transform.position, patrolTarget) < enemy.PatrolWaypointTolerance)
            patrolTarget = enemy.GetRandomPatrolTarget();
    }
}
