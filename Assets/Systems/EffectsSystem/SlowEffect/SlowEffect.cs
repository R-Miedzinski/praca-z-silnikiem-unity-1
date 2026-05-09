public class SlowEffect : Effect, IParametrizedEffect, ITimedEffect
{
  public float SlowValue { get; set; }
  public float Duration { get; set; }

  public void SetParameters(EffectParamsData parameters)
  {
    SlowValue = -parameters.Value / 100f;
    Duration = parameters.Duration;
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    target.MovementSpeedModifier += SlowValue;

    EffectInstance effectInstance = target.gameObject.AddComponent<EffectInstance>();
    effectInstance.Initialize(this, caster, target);
    target.ActiveEffects.Add(effectInstance);
  }

  public void Lift(Unit target)
  {
    target.MovementSpeedModifier -= SlowValue;
  }

  public void Tick(Unit target, float deltaTime)
  {
    // No periodic effect for slow
  }
}