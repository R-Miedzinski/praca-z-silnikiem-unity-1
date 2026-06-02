using UnityEngine;

public class DashEffect : Effect, IParametrizedEffect
{
  private float dashDistance;

  public void SetParameters(EffectParamsData parameters)
  {
    dashDistance = parameters.Value;
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    Vector2 dashDirection = target.FrontDirection;
    Vector2 newPosition = (Vector2)target.transform.position + dashDirection * dashDistance;

    target.transform.position = newPosition;
  }
}