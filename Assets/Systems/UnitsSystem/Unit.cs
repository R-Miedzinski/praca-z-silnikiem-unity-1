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

  private string unitName;
  private float health;
  private float maxHealth;
  private float movementSpeed;
  private float movementSpeedModifier;
  private float baseDamage;
  private float damageModifier;
  private float cooldownReduction;
  private float armor;
  private float armorModifier;
  private List<Effect> activeEffects; // placeholder for effect instance

  abstract public void TakeDamage(float amount);
  abstract public void Heal(float amount);
  abstract public void Die();
}