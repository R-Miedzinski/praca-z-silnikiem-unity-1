using UnityEngine;

public class ProjectileTargetingStrategy : TargetingStrategy
{
  public override void Target(TargetingMode targetingMode, Vector3 target, Unit caster, Effect[] effects)
  {
    Vector3 origin = caster.transform.position;
    var direction = (target - origin).normalized;
    Vector3 destination = origin + direction * targetingMode.Range;

    // starts a coroutine to move the projectile on a caster
    // this is a unity way to handle async operations without blocking the main thread
    caster.StartCoroutine(CastProjectile(origin, destination, targetingMode.TravelTime, caster, effects, targetingMode.AllowMultipleTargets));
  }

  private System.Collections.IEnumerator CastProjectile(Vector3 from, Vector3 to, float travelTime, Unit caster, Effect[] effects, bool targetMultiple)
  {
    float elapsedTime = 0f;
    Vector3 previousPosition = from;
    bool wasUnitHit = false;

    while (elapsedTime <= travelTime && (!wasUnitHit || targetMultiple))
    {
      float t = elapsedTime / travelTime;
      Vector3 currentPosition = Vector3.Lerp(from, to, t);

      // draw debug line
      Debug.DrawLine(previousPosition, currentPosition, Color.green, 0.2f);

      RaycastHit2D[] hits = Physics2D.RaycastAll(previousPosition, (to - from).normalized, (currentPosition - previousPosition).magnitude);
      foreach (var hit in hits)
      {
        Unit unit = hit.collider.GetComponent<Unit>();
        if (unit != null)
        {
          if (unit == caster)
            continue;

          ApplyEffectsToUnit(unit, caster, effects);
          wasUnitHit = true;

          if (!targetMultiple)
          {
            break;
          }
        }
      }

      previousPosition = currentPosition;

      // this awaits for the next frame, without adding additional artificial delay
      yield return null;
      elapsedTime += Time.deltaTime;
    }
  }
}