public class ItemEffect
{
  public Effect Effect { get { return effect; } }
  public ETriggerType TriggerType { get { return triggerType; } }
  public float Cooldown { get { return cooldown; } }
  public TargettingMode TargettingMode { get { return targettingMode; } }

  private Effect effect;
  private ETriggerType triggerType;
  private float cooldown;
  private TargettingMode targettingMode;

  public ItemEffect(Effect effect, ETriggerType triggerType, float cooldown, TargettingMode targettingMode)
  {
    this.effect = effect;
    this.triggerType = triggerType;
    this.cooldown = cooldown;
    this.targettingMode = targettingMode;
  }
}