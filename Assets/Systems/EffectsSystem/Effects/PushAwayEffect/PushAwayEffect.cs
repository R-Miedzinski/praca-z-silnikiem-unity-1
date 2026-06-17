using UnityEngine;

public class PushAwayEffect : Effect, IParametrizedEffect
{
  private float pushDistance;

  public void SetParameters(string[] parameters)
  {
    if (parameters.Length > 0 && EffectsUtils.TryParseFloat(parameters[0], out float result))
    {
      pushDistance = result;
    }
    else
    {
      EffectsUtils.InvalidParameters(0, "float (pushDistance)");
    }
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    if (!target.CanMove) return;

    Vector2 pushDirection = (target.transform.position - caster.transform.position).normalized;
    Vector2 newPosition = (Vector2)target.transform.position + pushDirection * pushDistance;

    target.transform.position = newPosition;
  }
}