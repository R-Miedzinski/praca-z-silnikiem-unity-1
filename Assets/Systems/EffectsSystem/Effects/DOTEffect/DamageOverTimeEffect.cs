using UnityEngine;

public class DamageOverTimeEffect : Effect, IParametrizedEffect, IPersistentEffect
{
  public float DamagePerSecond { get { return damagePerSecond; } set { damagePerSecond = Mathf.Max(0, value); } }
  private float damagePerSecond;

  public float Duration { get; set; }

  public void SetParameters(string[] parameters)
  {
    if (parameters.Length > 0 && EffectsUtils.TryParseFloat(parameters[0], out float result))
    {
      DamagePerSecond = result;
    }
    else
    {
    EffectsUtils.InvalidParameters(0, "float (DamagePerSecond)");
    }

    if (parameters.Length > 1 && EffectsUtils.TryParseFloat(parameters[1], out float durationResult))
    {
      Duration = durationResult;
    }
    else
    {
    EffectsUtils.InvalidParameters(1, "float (Duration)");
    }
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    EffectInstance effectInstance = target.gameObject.AddComponent<EffectInstance>();
    effectInstance.Initialize(this, caster, target);
    target.ActiveEffects[Id] = effectInstance;
  }

  public void Tick(Unit target, float deltaTime)
  {
    target.TakeDamage(DamagePerSecond * deltaTime);
  }

  public void Lift(Unit target)
  {
    // Clean up the effect if necessary (e.g., remove visual indicators)
  }
}