using UnityEngine;

public class PointTargetingStrategy : AreaTargetingStrategy
{
    public override Unit[] Target(TargetingMode targetingMode, Vector3 target, Unit caster)
    {
        TargetingMode rangeAdjustedPointTargetingMode = new TargetingMode(
            targetingMode.TargetingType,
            targetingMode.Range > 0 ? targetingMode.Range : 0.1f,
            targetingMode.TargetCaster,
            targetingMode.AllowMultipleTargets
        );
        return base.Target(rangeAdjustedPointTargetingMode, target, caster);
    }
}