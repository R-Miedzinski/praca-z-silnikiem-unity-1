using UnityEngine;

public class PointTargettingStrategy : AreaTargettingStrategy
{
    public override Unit[] Target(TargettingMode targettingMode, Vector3 target, Unit caster)
    {
        TargettingMode rangeAdjustedPointTargettingMode =  new TargettingMode(
            targettingMode.TargettingType,
            targettingMode.Range > 0 ? targettingMode.Range : 0.1f,
            targettingMode.TargetCaster,
            targettingMode.AllowMultipleTargets
        );
        return base.Target(rangeAdjustedPointTargettingMode, target, caster);
    }
}