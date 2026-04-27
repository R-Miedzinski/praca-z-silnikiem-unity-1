using UnityEngine;
using System.Collections.Generic;

abstract public class Unit : MonoBehaviour
{
  public string UnitName { get { return unitName; } }
  public float CurrentHealth { get { return health; } set { health = Mathf.Clamp(value, 0, maxHealth); } }
  public float MovementSpeed { get { return movementSpeed * movementSpeedModifier; } }
  public float Damage { get { return baseDamage * damageModifier; } }
  public float CooldownReduction { get { return cooldownReduction; } }
  public float Armor { get { return armor * armorModifier; } }

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
  protected List<Effect> activeEffects; // placeholder for effect instance

  abstract public void TakeDamage(float amount);
  abstract public void Heal(float amount);
  abstract public void Die();
}