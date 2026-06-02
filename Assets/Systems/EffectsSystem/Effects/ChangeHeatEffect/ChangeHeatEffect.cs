public class ChangeHeatEffect : Effect, IParametrizedEffect
{
  private float heatChangeAmount;

  public void SetParameters(EffectParamsData parameters)
  {
    heatChangeAmount = parameters.Value;
  }
  public override void ApplyEffect(Unit caster, Unit target)
  {
    if (target is PlayerCharacter player)
    {
      player.Heat += heatChangeAmount;
    }
  }
}