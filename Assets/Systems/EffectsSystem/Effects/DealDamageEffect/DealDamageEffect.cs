using UnityEngine;

public class DealDamageEffect : Effect, IParametrizedEffect
{
  public float DamageMultiplier { get { return damageMultiplier; } set { damageMultiplier = Mathf.Max(0, value); } }
  private float damageMultiplier;

  public void SetParameters(string[] parameters)
  {
    if (parameters.Length > 0 && EffectsUtils.TryParseFloat(parameters[0], out float result))
    {
      DamageMultiplier = result;
    }
    else
    {
      EffectsUtils.InvalidParameters(0, "float (damageMultiplier)");
    }
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    target.TakeDamage(caster.Damage * DamageMultiplier);
  }
}