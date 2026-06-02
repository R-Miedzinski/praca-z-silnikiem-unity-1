using UnityEngine;

public class PushAwayEffect : Effect, IParametrizedEffect
{
  private float pushDistance;

  public void SetParameters(EffectParamsData parameters)
  {
    pushDistance = parameters.Value;
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    Vector2 pushDirection = (target.transform.position - caster.transform.position).normalized;
    Vector2 newPosition = (Vector2)target.transform.position + pushDirection * pushDistance;

    target.transform.position = newPosition;
  }
}