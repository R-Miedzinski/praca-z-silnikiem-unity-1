public class SlowEffect : Effect, IParametrizedEffect, IPersistentEffect
{
  public float SlowValue { get; set; }
  public float Duration { get; set; }

  public virtual void SetParameters(string[] parameters)
  {
    if (parameters.Length > 0 && EffectsUtils.TryParseFloat(parameters[0], out float result))
    {
      SlowValue = -result / 100f;
    }
    else
    {
    EffectsUtils.InvalidParameters(0, "float (SlowValue)");
    }

    if (parameters.Length > 1 && EffectsUtils.TryParseFloat(parameters[1], out float durationResult))
    {
      Duration = durationResult;
    }
    else if (parameters.Length <= 1)
    {
      Duration = 0f;
    }
    else
    {
    EffectsUtils.InvalidParameters(1, "float (Duration)");
    }
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