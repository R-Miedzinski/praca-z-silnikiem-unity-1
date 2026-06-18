using UnityEngine;

public class ProtectFromDamageEffect : Effect, IPersistentEffect, IParametrizedEffect
{
  public float Duration { get; set; }

  public void SetParameters(string[] parameters)
  {
    if (parameters.Length > 0 && EffectsUtils.TryParseFloat(parameters[0], out float result))
    {
      Duration = result;
    }
    else
    {
    EffectsUtils.InvalidParameters(0, "float (Duration)");
    }
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