using UnityEngine;

public class DamageOverTimeEffect : Effect, IParametrizedEffect, ITimedEffect
{
  public float DamagePerSecond { get { return damagePerSecond; } set { damagePerSecond = Mathf.Max(0, value); } }
  private float damagePerSecond;

  public float Duration { get; set; }

  public void SetParameters(EffectParamsData parameters)
  {
    DamagePerSecond = parameters.Value;
    Duration = parameters.Duration;
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    EffectInstance effectInstance = target.gameObject.AddComponent<EffectInstance>();
    effectInstance.Initialize(this, caster, target);
    target.ActiveEffects.Add(effectInstance);
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