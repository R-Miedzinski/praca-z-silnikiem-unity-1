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
    Vector2 targetedPosition = (Vector2)target.transform.position + pushDirection * pushDistance;

    // Check for obstacles in the push direction, ignoring the target's own collider
    Collider2D targetCollider = target.GetComponent<Collider2D>();
    RaycastHit2D[] hits = Physics2D.RaycastAll(target.transform.position, pushDirection, pushDistance, LayerMask.GetMask("Environment"));

    RaycastHit2D validHit = new RaycastHit2D();
    foreach (RaycastHit2D hit in hits)
    {
      if (hit.collider != null && hit.collider != targetCollider)
      {
        validHit = hit;
        break;
      }
    }

    Debug.DrawLine(target.transform.position, targetedPosition, Color.red, 1f);
    Vector2 newPosition = validHit.collider != null ? validHit.point : targetedPosition;

    target.transform.position = newPosition;
  }
}