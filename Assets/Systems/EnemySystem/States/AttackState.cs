public class AttackState : EnemyState
{
    public override void Execute(Enemy enemy)
    {
        enemy.PerformAttack();
    }
}
