using UnityEngine;

public class DealDamageEffect : Effect, IParametrizedEffect
{
  public float DamageMultiplier { get { return damageMultiplier; } set { damageMultiplier = Mathf.Max(0, value); } }
  private float damageMultiplier;

  public void SetParameters(EffectParamsData parameters)
  {
    DamageMultiplier = parameters.Value;
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    target.TakeDamage(caster.Damage * DamageMultiplier);
  }
}