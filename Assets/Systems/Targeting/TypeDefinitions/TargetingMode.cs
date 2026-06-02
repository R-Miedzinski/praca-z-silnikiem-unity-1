public class TargetingMode
{
  public ETargetingType TargetingType { get { return targetingType; } }
  public float Range { get { return range; } }
  public float TravelTime { get { return travelTime; } }
  public float Angle { get { return angle; } }
  public bool AllowMultipleTargets { get { return allowMultipleTargets; } }
  public bool TargetCaster { get { return targetCaster; } }

  private ETargetingType targetingType;
  private float range;
  private float travelTime;
  private float angle;
  private bool allowMultipleTargets;
  private bool targetCaster;

  public TargetingMode(ETargetingType _targetingType, float _range, float _travelTime = 0.1f, float _angle = 0f, bool _allowMultipleTargets = false, bool _targetCaster = false)
  {
    targetingType = _targetingType;
    range = _range;
    travelTime = _travelTime;
    angle = _angle;
    allowMultipleTargets = _allowMultipleTargets;
    targetCaster = _targetCaster;
  }

  public TargetingMode(TargetingModeData data)
  {
    targetingType = data.TargetingType;
    range = data.Range;
    travelTime = data.TravelTime;
    angle = data.Angle;
    allowMultipleTargets = data.AllowMultipleTargets;
    targetCaster = data.TargetCaster;
  }
}