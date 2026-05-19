using UnityEngine;

[System.Serializable]
public struct TargetingModeData
{
  public ETargetingType TargetingType;
  public float Range;
  public bool AllowMultipleTargets;
  public bool TargetCaster;
}