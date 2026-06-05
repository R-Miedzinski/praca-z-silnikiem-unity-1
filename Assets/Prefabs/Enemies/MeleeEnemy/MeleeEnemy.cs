using UnityEngine;

// Wróg walczący w zwarciu — biega w stronę gracza i zadaje mu obrażenia.
// Dziedziczy całą logikę stanów (Patrol/Chase/Attack) z klasy Enemy.
// Tu implementujemy tylko to co jest unikalne dla tego typu wroga: atak.
public class MeleeEnemy : Enemy
{
    // Czas w sekundach między kolejnymi atakami.
    [SerializeField] private float attackCooldown = 1.5f;

    // Mnożnik obrażeń względem bazowego Damage wroga (1.0 = 100% damage).
    [SerializeField] private float damageMultiplier = 1f;

    private DealDamageEffect damageEffect;
    private float lastAttackTime = -Mathf.Infinity; // -Infinity gwarantuje że pierwszy atak jest natychmiastowy

    protected override void Start()
    {
        base.Start(); // ważne — wywołuje inicjalizację z klasy Enemy (zdrowie, gracz, patrol)

        // Tworzymy efekt bezpośrednio zamiast przez IdToEffectMap,
        // bo IdToEffectMap używa AssetDatabase dostępnego tylko w edytorze.
        damageEffect = new DealDamageEffect();
        damageEffect.DamageMultiplier = damageMultiplier;
    }

    // Wywoływana co klatkę gdy CurrentState == Attack.
    // Sprawdza cooldown i jeśli minął — zadaje obrażenia graczowi.
    protected override void OnAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        damageEffect.ApplyEffect(this, GetPlayer());
    }
}
