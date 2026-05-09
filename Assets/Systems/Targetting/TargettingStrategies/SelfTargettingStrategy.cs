using UnityEngine;

public class SelfTargettingStrategy : AreaTargettingStrategy
{
  public override Unit[] Target(TargettingMode targettingMode, Vector3 target, Unit caster)
    {
        return base.Target(targettingMode, caster.transform.position, caster);
    }
}