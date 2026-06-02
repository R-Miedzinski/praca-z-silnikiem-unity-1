using UnityEngine;

public class DestroyBox : Unit
{
    public int CurrentHitCount { get { return currentHitCount; } }
    public int RequiredHitCount { get { return requiredHitCount; } }

    [SerializeField] private int requiredHitCount = 3;
    [SerializeField] private float hitCooldown = 0.25f;
    [SerializeField] private float minimumDamageToCountHit = 0.1f;
    private int currentHitCount;
    private float lastHitTime = -Mathf.Infinity;

    private void OnValidate()
    {
        requiredHitCount = Mathf.Max(1, requiredHitCount);
        hitCooldown = Mathf.Max(0, hitCooldown);
        minimumDamageToCountHit = Mathf.Max(0, minimumDamageToCountHit);
    }

    private void OnEnable()
    {
        canTakeDamage = true;
        currentHitCount = 0;
        lastHitTime = -Mathf.Infinity;
    }

    private void OnDisable()
    {
        canTakeDamage = false;
    }

    public override void TakeDamage(float amount)
    {
        if (!canTakeDamage)
            return;

        if (amount < minimumDamageToCountHit)
            return;

        if (Time.time < lastHitTime + hitCooldown)
            return;

        lastHitTime = Time.time;
        currentHitCount++;

        if (currentHitCount == requiredHitCount)
        {
            Die();
        }
    }

    public override void Heal(float amount)
    {
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
