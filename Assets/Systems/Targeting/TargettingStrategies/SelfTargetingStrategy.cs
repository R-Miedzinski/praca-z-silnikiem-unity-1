using UnityEngine;

public class SelfTargetingStrategy : AreaTargetingStrategy
{
  public override void Target(TargetingMode targetingMode, Vector3 target, Unit caster, Effect[] effects)
  {
    base.Target(targetingMode, caster.transform.position, caster, effects);
  }
}