using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class Unit : MonoBehaviour
{
  public string UnitName { get { return unitName; } }
  public float CurrentHealth { get { return health; } set { health = Mathf.Clamp(value, 0, maxHealth); } }
  public float MovementSpeed { get { return movementSpeed * Mathf.Max(0, movementSpeedModifier); } }
  public float Damage { get { return baseDamage * Mathf.Max(0, damageModifier); } }
  public float CooldownReduction { get { return cooldownReduction; } }
  public float Armor { get { return armor * Mathf.Max(0, armorModifier); } }
  public float MovementSpeedModifier { get { return movementSpeedModifier; } set { movementSpeedModifier = value; } }
  public float DamageModifier { get { return damageModifier; } set { damageModifier = value; } }
  public float ArmorModifier { get { return armorModifier; } set { armorModifier = value; } }
  public bool CanTakeDamage { get { return canTakeDamage; } set { canTakeDamage = value; } }
  public bool CanMove { get { return canMove; } set { canMove = value; } }
  public Vector2 FrontDirection { get { return frontDirection; } }
  public Dictionary<string, EffectInstance> ActiveEffects { get; } = new Dictionary<string, EffectInstance>();

  [SerializeField] protected string unitName;
  protected float health;
  [SerializeField] protected float maxHealth;
  [SerializeField] protected float movementSpeed;
  [SerializeField] protected float movementSpeedModifier = 1f;
  [SerializeField] protected float baseDamage;
  [SerializeField] protected float damageModifier = 1f;
  [SerializeField] protected float cooldownReduction;
  [SerializeField] protected float armor;
  [SerializeField] protected float armorModifier = 1f;
  [SerializeField] protected bool canMove = true;
  protected bool canTakeDamage = true;
  protected Vector2 frontDirection = Vector2.right;

  private float pendingDamage = 0f;
  private Coroutine damageCoroutine;

  private void OnEnable()
  {
    canMove = true;
    canTakeDamage = true;
    pendingDamage = 0f;

    if (damageCoroutine != null)
    {
      StopCoroutine(damageCoroutine);
    }
    damageCoroutine = StartCoroutine(ApplyDamageAtEndOfFrame());
  }

  private void OnDisable()
  {
    canMove = false;
    canTakeDamage = false;
    pendingDamage = 0f;

    if (damageCoroutine != null)
    {
      StopCoroutine(damageCoroutine);
      damageCoroutine = null;
    }
  }

  public virtual void TakeDamage(float amount)
  {
    pendingDamage += amount;
  }

  public abstract void Heal(float amount);
  public abstract void Die();

  private IEnumerator ApplyDamageAtEndOfFrame()
  {
    while (true)
    {
      yield return new WaitForEndOfFrame();

      if (!canTakeDamage) // is in immunity frame
      {
        pendingDamage = 0f;
        continue;
      }

      if (pendingDamage > 0f)
      {
        CurrentHealth -= pendingDamage;
        pendingDamage = 0f;

        if (CurrentHealth <= 0)
        {
          Die();
        }
      }
    }
  }
}