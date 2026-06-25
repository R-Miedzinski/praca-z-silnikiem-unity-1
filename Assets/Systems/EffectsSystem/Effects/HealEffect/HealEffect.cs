public class HealEffect : Effect, IParametrizedEffect
{
  private float healAmount;

  public void SetParameters(string[] parameters)
  {
    if (parameters.Length > 0 && EffectsUtils.TryParseFloat(parameters[0], out float result))
    {
      healAmount = result;
    }
    else
    {
    EffectsUtils.InvalidParameters(0, "float (healAmount)");
    }
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    target.Heal(healAmount);
  }
}