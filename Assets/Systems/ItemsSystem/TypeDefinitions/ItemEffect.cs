using UnityEngine;
using System.Collections.Generic;

public class ItemEffect
{
  public Effect[] Effects { get { return effects; } }
  // TODO: Refactor to support multi-trigger actiavtions
  public ETriggerType[] EffectTriggers { get { return effectTriggers; } }
  public TargetingMode TargetingMode { get { return targetingMode; } }
  public float Cooldown { get { return cooldown; } }
  public int Charges { get { return remainingCharges; } }
  public bool IsTogglable { get { return isTogglable; } }
  public bool IsToggled { get { return isToggled; } }

  private Effect[] effects;
  private ETriggerType[] effectTriggers;
  private TargetingMode targetingMode;
  private float cooldown;
  private float baseCooldown;
  private float lastUsedTime;
  private int remainingCharges;
  private int baseCharges;
  private bool isTogglable;
  private bool isToggled;
  private List<Unit> toggledTargets = new List<Unit>();

  public ItemEffect(Effect[] _effects, ETriggerType[] _effectTriggers, TargetingMode _targetingMode, float _cooldown, int _charges = -1, bool _isTogglable = false)
  {
    effects = _effects;
    effectTriggers = _effectTriggers;
    cooldown = _cooldown;
    baseCooldown = _cooldown;
    lastUsedTime = -Mathf.Infinity;
    targetingMode = _targetingMode;
    remainingCharges = _charges;
    baseCharges = _charges;
    isTogglable = _isTogglable;
    isToggled = false;

    if (isTogglable)
    {
      ValidateTogglableEffects();
    }
  }

  public ItemEffect(Effect[] _effects, ETriggerType _triggerType, TargetingMode _targetingMode, float _cooldown)
  {
    effects = _effects;
    effectTriggers = new ETriggerType[] { _triggerType };
    cooldown = _cooldown;
    baseCooldown = _cooldown;
    lastUsedTime = -Mathf.Infinity;
    targetingMode = _targetingMode;
    remainingCharges = -1; // Unlimited charges
    baseCharges = -1;
    isTogglable = false;
    isToggled = false;
  }

  public bool CanUse()
  {
    // Toggled-on effects can always be toggled off regardless of cooldown or charges.
    if (isTogglable && isToggled) return true;
    return Time.time >= lastUsedTime + cooldown && remainingCharges != 0;
  }

  public void StartCooldown()
  {
    lastUsedTime = Time.time;
    if (remainingCharges > 0)
    {
      remainingCharges--;
    }
  }

  public void StartCooldown(Unit caster)
  {
    cooldown = baseCooldown * (100f - caster.CooldownReduction) / 100f;
    StartCooldown();
  }

  public void ActivateToggle(List<Unit> targets)
  {
    isToggled = true;
    foreach (var target in targets)
    {
      if (!toggledTargets.Contains(target))
        toggledTargets.Add(target);
    }
  }

  public void LiftToggle()
  {
    foreach (var target in toggledTargets)
    {
      if (target == null) continue;

      foreach (var effect in effects)
      {
        if (!target.ActiveEffects.TryGetValue(effect.Id, out EffectInstance instance) || instance == null)
          continue;

        if (effect is IPersistentEffect persistentEffect)
          persistentEffect.Lift(target);

        target.ActiveEffects.Remove(effect.Id);
        Object.Destroy(instance);
      }
    }

    toggledTargets.Clear();
    isToggled = false;
  }

  private void ValidateTogglableEffects()
  {
    if (targetingMode.TargetingType == ETargetingType.Projectile)
    {
      Debug.LogError("Projectile targeting is not compatible with toggleable ItemEffects.");
    }

    foreach (var effect in effects)
    {
      if (effect is not IPersistentEffect)
      {
        Debug.LogError($"Effect '{effect.GetType().Name}' in a toggleable ItemEffect does not implement IPersistentEffect. Toggle-off will not properly lift this effect.");
      }
    }
  }
}