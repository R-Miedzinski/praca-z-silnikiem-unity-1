using UnityEngine;

[System.Serializable]
public struct TargetingModeData
{
  public ETargetingType TargetingType;
  [Tooltip("Range for this targeting mode. Not required for point targeting mode.")]
  public float Range;
  [Tooltip("Travel time for the projectile. Required only for projectile targeting type.")]
  public float TravelTime;
  [Tooltip("Whether multiple targets are allowed for this targeting mode. Specific behaviour depends on targeting type.")]
  public bool AllowMultipleTargets;
  [Tooltip("Whether the caster can be targeted by this targeting mode.")]
  public bool TargetCaster;
}