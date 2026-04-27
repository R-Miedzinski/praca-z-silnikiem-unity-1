using System.Collections.Generic;

public class TargettingStrategyUtils
{
  private static Dictionary<ETargettingType, TargettingStrategy> targettingStrategyMap = new Dictionary<ETargettingType, TargettingStrategy>()
  {
    // { ETargettingType.Self, new SelfTargettingStrategy() },
    { ETargettingType.Line, new LineTargettingStrategy() },
    // { ETargettingType.Area, new AreaTargettingStrategy() }
  };

  public static TargettingStrategy GetTargettingStrategy(ETargettingType targettingType)
  {
    if (targettingStrategyMap.TryGetValue(targettingType, out var strategy))
    {
      return strategy;
    }
    else
    {
      throw new System.Exception("No targetting strategy found for type: " + targettingType);
    }
  }
}