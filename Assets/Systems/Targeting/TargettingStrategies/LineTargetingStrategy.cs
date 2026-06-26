using UnityEngine;
using System.Collections.Generic;

public class LineTargetingStrategy : TargetingStrategy
{
  public override void Target(TargetingMode targetingMode, Vector3 target, Unit caster, Effect[] effects)
  {
    Vector3 origin = caster.transform.position;
    var direction = (target - origin).normalized;
    RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, targetingMode.Range);

    // draw debug line
    DrawDebugLine(origin, origin + direction * targetingMode.Range, Color.red, 1f);

    List<Unit> units = new List<Unit>();
    foreach (var hit in hits)
    {
      Unit unit = hit.collider.GetComponent<Unit>();
      if (unit != null)
      {
        if (!targetingMode.TargetCaster && unit == caster)
          continue;

        if (!targetingMode.AllowMultipleTargets && units.Count > 0)
          continue;

        units.Add(unit);
      }
    }

    foreach (var targetUnit in units)
    {
      ApplyEffectsToUnit(targetUnit, caster, effects);
    }
  }
}