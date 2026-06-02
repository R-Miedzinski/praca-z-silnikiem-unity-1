using UnityEngine;

public class PointTargetingStrategy : AreaTargetingStrategy
{
    public override void Target(TargetingMode targetingMode, Vector3 target, Unit caster, Effect[] effects)
    {
        TargetingMode rangeAdjustedPointTargetingMode = new TargetingMode(
            targetingMode.TargetingType,
            targetingMode.Range > 0 ? targetingMode.Range : 0.1f,
            targetingMode.TravelTime > 0.0f ? targetingMode.TravelTime : 0.1f,
            targetingMode.Angle > 0.0f ? targetingMode.Angle : 0.1f,
            targetingMode.TargetCaster,
            targetingMode.AllowMultipleTargets
        );

        base.Target(rangeAdjustedPointTargetingMode, target, caster, effects);
    }
}