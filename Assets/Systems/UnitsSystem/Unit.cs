using UnityEngine;
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
  public List<EffectInstance> ActiveEffects { get; } = new List<EffectInstance>(); // placeholder for effect instances, DoT and debuffs

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
  protected bool canMove = true;
  protected bool canTakeDamage = true;

  abstract public void TakeDamage(float amount);
  abstract public void Heal(float amount);
  abstract public void Die();
}