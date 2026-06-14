public class SlowEffect : Effect, IParametrizedEffect, IPersistentEffect
{
  public float SlowValue { get; set; }

  public virtual void SetParameters(EffectParamsData parameters)
  {
    SlowValue = -parameters.Value / 100f;
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    target.MovementSpeedModifier += SlowValue;

    EffectInstance effectInstance = target.gameObject.AddComponent<EffectInstance>();
    effectInstance.Initialize(this, caster, target);
    target.ActiveEffects[Id] = effectInstance;
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