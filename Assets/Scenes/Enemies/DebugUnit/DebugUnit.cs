using UnityEngine;

public class DebugUnit : Unit
{
    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public override void TakeDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public override void Heal(float amount)
    {
        CurrentHealth += amount;
    }

    public override void Die()
    {
        Debug.Log($"{UnitName} has died.");
        Destroy(gameObject);
    }
}
