using System;
using UnityEngine;

public class ConeTargetingStrategy : TargetingStrategy
{
  public override void Target(TargetingMode targetingMode, Vector3 target, Unit caster, Effect[] effects)
  {
    float angle = Mathf.Deg2Rad * targetingMode.Angle;
    Vector3 origin = caster.transform.position;
    Vector3 direction = (target - origin).normalized;
    Vector3 front = caster.FrontDirection.normalized;

    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(origin, targetingMode.Range);

    // draw debug cone
    DrawDebugCone(origin, front, targetingMode.Range, angle, Color.red, 1f);

    foreach (var hitCollider in hitColliders)
    {
      Unit unit = hitCollider.GetComponent<Unit>();
      if (unit != null)
      {
        if (!targetingMode.TargetCaster && unit == caster)
          continue;

        if (!targetingMode.AllowMultipleTargets && unit != caster)
          continue;

        Vector3 toUnit = (unit.transform.position - origin).normalized;
        float angleToUnit = Mathf.Deg2Rad * Vector3.Angle(front, toUnit);

        if (angleToUnit <= angle / 2f)
        {
          ApplyEffectsToUnit(unit, caster, effects);
        }
      }
    }
  }

  private void DrawDebugCone(Vector3 center, Vector3 front, float radius, float angle, Color color, float duration)
  {
    int segments = 36;
    float halfAngle = angle / 2f;
    Vector3 lastPoint = center;
    float frontAngle = Mathf.Atan2(front.y, front.x);

    float angleStep = angle / segments;
    float currentAngle = -halfAngle;
    for (int i = 0; i < segments + 1; i++)
    {
      float x = center.x + Mathf.Cos(currentAngle + frontAngle) * radius;
      float y = center.y + Mathf.Sin(currentAngle + frontAngle) * radius;
      Vector3 point = new Vector3(x, y, center.z);
      DrawDebugLine(lastPoint, point, color, duration);

      lastPoint = point;
      currentAngle += angleStep;
    }
    DrawDebugLine(lastPoint, center, color, duration);
  }
}