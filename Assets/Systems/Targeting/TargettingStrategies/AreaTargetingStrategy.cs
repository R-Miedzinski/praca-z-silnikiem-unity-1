using UnityEngine;
using System.Collections.Generic;

public class AreaTargetingStrategy : TargetingStrategy
{
  public override void Target(TargetingMode targetingMode, Vector3 target, Unit caster, Effect[] effects)
  {
    Vector3 origin = caster.transform.position;
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(target, targetingMode.Range);

    // draw debug sphere
    DrawDebugCircle(target, targetingMode.Range, Color.blue, 1f);

    List<Unit> targets = new List<Unit>();
    foreach (var hitCollider in hitColliders)
    {
      Unit unit = hitCollider.GetComponent<Unit>();
      if (unit != null)
      {
        if (!targetingMode.TargetCaster && unit == caster)
          continue;

        if (!targetingMode.AllowMultipleTargets && targets.Count > 0)
          continue;

        targets.Add(unit);
      }
    }

    foreach (var targetUnit in targets)
    {
      ApplyEffectsToUnit(targetUnit, caster, effects);
    }
  }

  private void DrawDebugCircle(Vector3 center, float radius, Color color, float duration)
  {
    int segments = 36;
    float angle = 0f;
    Vector2 lastPoint = center + new Vector3(radius, 0, 0);

    for (int i = 0; i < segments + 1; i++)
    {
      float x = center.x + Mathf.Cos(angle) * radius;
      float y = center.y + Mathf.Sin(angle) * radius;
      Vector3 point = new Vector3(x, y, center.z);
      DrawDebugLine(lastPoint, point, color, duration);

      lastPoint = point;
      angle += 2 * Mathf.PI / segments;
    }
  }
}