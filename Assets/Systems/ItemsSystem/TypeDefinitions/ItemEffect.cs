using UnityEngine;

public class ItemEffect
{
  public Effect[] Effects { get { return effects; } }
  public ETriggerType TriggerType { get { return triggerType; } }
  public float Cooldown { get { return cooldown; } }
  public TargettingMode TargettingMode { get { return targettingMode; } }

  private Effect[] effects;
  private ETriggerType triggerType;
  private float cooldown;
  private float lastUsedTime;
  private TargettingMode targettingMode;

  public ItemEffect(Effect[] _effects, ETriggerType _triggerType, float _cooldown, TargettingMode _targettingMode)
  {
    effects = _effects;
    triggerType = _triggerType;
    cooldown = _cooldown;
    lastUsedTime = Time.time - cooldown;
    targettingMode = _targettingMode;
  }

  public bool CanUse()
  {
    return Time.time >= lastUsedTime + cooldown;
  }

  public void StartCooldown()
  {
    lastUsedTime = Time.time;
  }
}