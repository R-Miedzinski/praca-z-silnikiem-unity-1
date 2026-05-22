using System.Collections.Generic;

public class TargetingStrategyUtils
{
  private static Dictionary<ETargetingType, TargetingStrategy> targetingStrategyMap = new Dictionary<ETargetingType, TargetingStrategy>()
  {
    { ETargetingType.Self, new SelfTargetingStrategy() },
    { ETargetingType.Line, new LineTargetingStrategy() },
    { ETargetingType.Area, new AreaTargetingStrategy() },
    { ETargetingType.Projectile, new ProjectileTargetingStrategy() },
    { ETargetingType.Point, new PointTargetingStrategy() },
  };

  public static TargetingStrategy GetTargetingStrategy(ETargetingType targetingType)
  {
    if (targetingStrategyMap.TryGetValue(targetingType, out var strategy))
    {
      return strategy;
    }
    else
    {
      throw new System.Exception("No targeting strategy found for type: " + targetingType);
    }
  }
}