public class ChangeHeatEffect : Effect, IParametrizedEffect
{
  private float heatChangeAmount;

  public void SetParameters(string[] parameters)
  {
    if (parameters.Length > 0 && EffectsUtils.TryParseFloat(parameters[0], out float result))
    {
      heatChangeAmount = result;
    }
    else
    {
    EffectsUtils.InvalidParameters(0, "float (heatChangeAmount)");
    }
  }
  public override void ApplyEffect(Unit caster, Unit target)
  {
    if (target is PlayerCharacter player)
    {
      player.Heat += heatChangeAmount;
    }
  }
}