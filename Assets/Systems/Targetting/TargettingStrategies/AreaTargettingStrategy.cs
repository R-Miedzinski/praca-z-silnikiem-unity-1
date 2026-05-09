using UnityEngine;
using System.Collections.Generic;

public class AreaTargettingStrategy : TargettingStrategy
{
  public override Unit[] Target(TargettingMode targettingMode, Vector3 target, Unit caster)
  {
    Vector3 origin = caster.transform.position;
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(target, targettingMode.Range);

    // draw debug sphere
    DrawDebugCircle(target, targettingMode.Range, Color.blue, 1f);
    
    List<Unit> targets = new List<Unit>();
    foreach (var hitCollider in hitColliders)
    {
      Unit unit = hitCollider.GetComponent<Unit>();
      if (unit != null)
      {
        if (!targettingMode.TargetCaster && unit == caster)
          continue;

        if (!targettingMode.AllowMultipleTargets && targets.Count > 0)
          continue;

        targets.Add(unit);
      }
    }
    return targets.ToArray();
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
      Debug.DrawLine(lastPoint, point, color, duration);

      lastPoint = point;
      angle += 2 * Mathf.PI / segments;
    }
  }
}