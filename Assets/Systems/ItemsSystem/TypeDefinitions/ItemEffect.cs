using UnityEngine;

public class ItemEffect
{
  public Effect[] Effects { get { return effects; } }
  public ETriggerType TriggerType { get { return triggerType; } }
  public float Cooldown { get { return cooldown; } }
  public TargetingMode TargetingMode { get { return targetingMode; } }

  private Effect[] effects;
  private ETriggerType triggerType;
  private float cooldown;
  private float baseCooldown;
  private float lastUsedTime;
  private TargetingMode targetingMode;

  public ItemEffect(Effect[] _effects, ETriggerType _triggerType, float _cooldown, TargetingMode _targetingMode)
  {
    effects = _effects;
    triggerType = _triggerType;
    cooldown = _cooldown;
    baseCooldown = _cooldown;
    lastUsedTime = Time.time - cooldown;
    targetingMode = _targetingMode;
  }

  public bool CanUse()
  {
    return Time.time >= lastUsedTime + cooldown;
  }

  public void StartCooldown()
  {
    lastUsedTime = Time.time;
  }

  public void StartCooldown(Unit caster)
  {
    lastUsedTime = Time.time;
    cooldown = baseCooldown * (100f - caster.CooldownReduction) / 100f;
  }
}