using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private EffectIdParamPair[] attackEffects;

    private Effect[] resolvedEffects;
    private float lastAttackTime = -Mathf.Infinity;

    protected override void Start()
    {
        base.Start();
        resolvedEffects = new Effect[attackEffects.Length];
        for (int i = 0; i < attackEffects.Length; i++)
            resolvedEffects[i] = IdToEffectMap.GetEffectById(attackEffects[i].EffectId, attackEffects[i].EffectParams);
    }

    public override void PerformAttack()
    {
        float effectiveCooldown = attackCooldown * (100f - CooldownReduction) / 100f;
        if (Time.time < lastAttackTime + effectiveCooldown)
            return;

        lastAttackTime = Time.time;
        foreach (var effect in resolvedEffects)
            effect?.ApplyEffect(this, GetPlayer());
    }
}
