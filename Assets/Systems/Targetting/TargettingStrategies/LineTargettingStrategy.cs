using UnityEngine;
using System.Collections.Generic;

public class LineTargettingStrategy : TargettingStrategy
{
  public override Unit[] Target(TargettingMode targettingMode, Vector3 target, Vector3 origin)
  {
    var direction = (target - origin).normalized;
    RaycastHit2D[] hits;

    if (targettingMode.AllowMultipleTargets)
    {
      hits = Physics2D.RaycastAll(origin, direction, targettingMode.Range);
    }
    else
    {
      var hit = Physics2D.Raycast(origin, direction, targettingMode.Range);
      hits = hit.collider != null ? new RaycastHit2D[] { hit } : new RaycastHit2D[] { };
    }

    // draw debug line
    Debug.DrawLine(origin, origin + direction * targettingMode.Range, Color.red, 1f);

    List<Unit> units = new List<Unit>();
    foreach (var hit in hits)
    {
      Unit unit = hit.collider.GetComponent<Unit>();
      if (unit != null)
      {
        units.Add(unit);
      }
    }
    return units.ToArray();
  }
}