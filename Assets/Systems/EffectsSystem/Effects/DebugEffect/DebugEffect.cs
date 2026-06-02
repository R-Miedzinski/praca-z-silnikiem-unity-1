public class DebugEffect : Effect, IParametrizedEffect
{

  private string message = "Default Debug Effect";

  public override void ApplyEffect(Unit caster, Unit target)
  {
    UnityEngine.Debug.Log(message + " with caster: " + caster + " and target: " + target);
  }

  public void SetParameters(EffectParamsData parameters)
  {
    message = parameters.StringValue;
  }
}