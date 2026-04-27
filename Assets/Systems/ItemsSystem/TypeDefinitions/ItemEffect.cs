using UnityEngine;

public class ItemEffect
{
  public Effect Effect { get { return effect; } }
  public ETriggerType TriggerType { get { return triggerType; } }
  public float Cooldown { get { return cooldown; } }
  public TargettingMode TargettingMode { get { return targettingMode; } }

  private Effect effect;
  private ETriggerType triggerType;
  private float cooldown;
  private float lastUsedTime;
  private TargettingMode targettingMode;

  public ItemEffect(Effect _effect, ETriggerType _triggerType, float _cooldown, TargettingMode _targettingMode)
  {
    effect = _effect;
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