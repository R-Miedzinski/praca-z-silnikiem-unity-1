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
    Vector2 targetedPosition = (Vector2)target.transform.position + dashDirection * dashDistance;

    // Check for obstacles in the dash direction
    RaycastHit2D hit = Physics2D.Raycast(target.transform.position, dashDirection, dashDistance, LayerMask.GetMask("Environment"));
    Vector2 newPosition = hit.collider != null ? hit.point : targetedPosition;

    target.transform.position = newPosition;
  }
}