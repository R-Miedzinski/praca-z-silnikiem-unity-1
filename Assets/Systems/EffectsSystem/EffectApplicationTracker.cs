using System.Collections.Generic;

// Static tracker that captures which units are targeted during an effect application.
// Used by toggleable ItemEffects to record targets for later toggle-off.
// Begin a tracking session before calling TargetingStrategy.Target(), then stop it afterward.
// EffectInstance.Initialize() reports each targeted unit automatically.
// Note: async targeting (e.g. projectiles) populates targets after StopTracking(), so those
// targets will not be captured — toggleable effects with projectile targeting are not supported.
public static class EffectApplicationTracker
{
  private static List<Unit> activeTracker = null;

  public static void BeginTracking(List<Unit> tracker)
  {
    activeTracker = tracker;
  }

  public static void StopTracking()
  {
    activeTracker = null;
  }

  public static void ReportTarget(Unit target)
  {
    if (activeTracker != null && !activeTracker.Contains(target))
    {
      activeTracker.Add(target);
    }
  }
}
