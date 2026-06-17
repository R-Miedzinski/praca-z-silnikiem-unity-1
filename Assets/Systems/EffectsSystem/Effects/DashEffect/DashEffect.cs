using UnityEngine;

public class DashEffect : Effect, IParametrizedEffect
{
  private float dashDistance;

  public void SetParameters(string[] parameters)
  {
    if (parameters.Length > 0 && EffectsUtils.TryParseFloat(parameters[0], out float result))
    {
      dashDistance = result;
    }
    else
    {
    EffectsUtils.InvalidParameters(0, "float (dashDistance)");
    }
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    Vector2 dashDirection = target.FrontDirection;
    Vector2 newPosition = (Vector2)target.transform.position + dashDirection * dashDistance;

    target.transform.position = newPosition;
  }
}