public class HealEffect : Effect, IParametrizedEffect
{
  private float healAmount;

  public void SetParameters(EffectParamsData parameters)
  {
    healAmount = parameters.Value;
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    target.Heal(healAmount);
  }
}