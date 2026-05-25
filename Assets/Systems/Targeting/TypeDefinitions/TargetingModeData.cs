using UnityEngine;

[System.Serializable]
public struct TargetingModeData
{
  public ETargetingType TargetingType;
  public float Range;
  public float TravelTime;
  public bool AllowMultipleTargets;
  public bool TargetCaster;
}