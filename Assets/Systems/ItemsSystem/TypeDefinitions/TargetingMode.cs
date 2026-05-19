public class TargetingMode
{
  public ETargetingType TargetingType { get { return targetingType; } }
  public float Range { get { return range; } }
  public bool AllowMultipleTargets { get { return allowMultipleTargets; } }
  public bool TargetCaster { get { return targetCaster; } }

  private ETargetingType targetingType;
  private float range;
  private bool allowMultipleTargets;
  private bool targetCaster;

  public TargetingMode(ETargetingType _targetingType, float _range, bool _allowMultipleTargets, bool _targetCaster)
  {
    targetingType = _targetingType;
    range = _range;
    allowMultipleTargets = _allowMultipleTargets;
    targetCaster = _targetCaster;
  }

  public TargetingMode(TargetingModeData data)
  {
    targetingType = data.TargetingType;
    range = data.Range;
    allowMultipleTargets = data.AllowMultipleTargets;
    targetCaster = data.TargetCaster;
  }
}