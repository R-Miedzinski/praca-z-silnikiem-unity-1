public class ProtectFromDamageEffect : Effect, ITimedEffect, IParametrizedEffect
{
  public float Duration { get; set; }

  public void SetParameters(EffectParamsData parameters)
  {
    Duration = parameters.Duration;
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    target.CanTakeDamage = false;

    EffectInstance effectInstance = target.gameObject.AddComponent<EffectInstance>();
    effectInstance.Initialize(this, caster, target);
    target.ActiveEffects[Id] = effectInstance;
  }

  public void Tick(Unit target, float deltaTime)
  {
    // No periodic effect
  }

  public void Lift(Unit target)
  {
    target.CanTakeDamage = true;
  }
}