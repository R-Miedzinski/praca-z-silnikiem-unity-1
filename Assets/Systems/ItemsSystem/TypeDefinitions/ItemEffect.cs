using UnityEngine;

public class ItemEffect
{
  public Effect[] Effects { get { return effects; } }
  // TODO: Refactor to support multi-trigger actiavtions
  public ETriggerType[] EffectTriggers { get { return effectTriggers; } }
  public TargetingMode TargetingMode { get { return targetingMode; } }
  public float Cooldown { get { return cooldown; } }
  public int Charges { get { return remainingCharges; } }

  private Effect[] effects;
  private ETriggerType[] effectTriggers;
  private TargetingMode targetingMode;
  private float cooldown;
  private float baseCooldown;
  private float lastUsedTime;
  private int remainingCharges;
  private int baseCharges;

  public ItemEffect(Effect[] _effects, ETriggerType[] _effectTriggers, TargetingMode _targetingMode, float _cooldown, int _charges = -1)
  {
    effects = _effects;
    effectTriggers = _effectTriggers;
    cooldown = _cooldown;
    baseCooldown = _cooldown;
    lastUsedTime = -Mathf.Infinity;
    targetingMode = _targetingMode;
    remainingCharges = _charges;
    baseCharges = _charges;
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
  }

  public bool CanUse()
  {
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
}