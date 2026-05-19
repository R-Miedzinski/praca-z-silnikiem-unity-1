using UnityEngine;

public class SelfTargetingStrategy : AreaTargetingStrategy
{
  public override Unit[] Target(TargetingMode targetingMode, Vector3 target, Unit caster)
  {
    return base.Target(targetingMode, caster.transform.position, caster);
  }
}