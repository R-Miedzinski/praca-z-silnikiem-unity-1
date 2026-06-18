public class ChaseState : EnemyState
{
    public override void Execute(Enemy enemy)
    {
        enemy.MoveToward(enemy.GetPlayer().transform.position);
    }
}
