using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float damageMultiplier = 1f;

    private DealDamageEffect damageEffect;
    private float lastAttackTime = -Mathf.Infinity; // -Infinity gwarantuje że pierwszy atak jest natychmiastowy

    protected override void Start()
    {
        base.Start();
        damageEffect = new DealDamageEffect();
        damageEffect.DamageMultiplier = damageMultiplier;
    }

    protected override void OnAttack()
    {
        float effectiveCooldown = attackCooldown * (100f - CooldownReduction) / 100f;
        if (Time.time < lastAttackTime + effectiveCooldown)
            return;

        lastAttackTime = Time.time;
        damageEffect.ApplyEffect(this, GetPlayer());
    }
}
