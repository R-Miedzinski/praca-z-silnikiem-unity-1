public class DebugEffect : Effect
{
  public override void ApplyEffect(Unit caster, Unit target)
  {
    UnityEngine.Debug.Log("Debug Effect Applied with caster: " + caster + " and target: " + target);
  }
}