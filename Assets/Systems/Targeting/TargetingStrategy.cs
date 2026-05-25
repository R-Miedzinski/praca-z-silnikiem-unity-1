using UnityEngine;

abstract public class TargetingStrategy
{
  public abstract void Target(TargetingMode targetingMode, Vector3 target, Unit caster, Effect[] effects);

  protected void ApplyEffectsToUnit(Unit target, Unit caster, Effect[] effects)
  {
    foreach (var effect in effects)
    {
      effect.ApplyEffect(caster, target);
    }
  }
}