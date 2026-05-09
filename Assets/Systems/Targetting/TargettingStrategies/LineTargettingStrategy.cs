using UnityEngine;
using System.Collections.Generic;

public class LineTargettingStrategy : TargettingStrategy
{
  public override Unit[] Target(TargettingMode targettingMode, Vector3 target, Unit caster)
  {
    Vector3 origin = caster.transform.position;
    var direction = (target - origin).normalized;
    RaycastHit2D[] hits = hits = Physics2D.RaycastAll(origin, direction, targettingMode.Range);;

    // draw debug line
    Debug.DrawLine(origin, origin + direction * targettingMode.Range, Color.red, 1f);

    List<Unit> units = new List<Unit>();
    foreach (var hit in hits)
    {
      Unit unit = hit.collider.GetComponent<Unit>();
      if (unit != null)
      {
        if (!targettingMode.TargetCaster && unit == caster)
          continue;

        if (!targettingMode.AllowMultipleTargets && units.Count > 0)
          continue;

        units.Add(unit);
      }
    }
    return units.ToArray();
  }
}