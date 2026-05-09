public class TargettingMode
{
  public ETargettingType TargettingType { get { return targettingType; } }
  public float Range { get { return range; } }
  public bool AllowMultipleTargets { get { return allowMultipleTargets; } }
  public bool TargetCaster { get { return targetCaster; } }

  private ETargettingType targettingType;
  private float range;
  private bool allowMultipleTargets;
  private bool targetCaster;

  public TargettingMode(ETargettingType _targettingType, float _range, bool _allowMultipleTargets, bool _targetCaster)
  {
    targettingType = _targettingType;
    range = _range;
    allowMultipleTargets = _allowMultipleTargets;
    targetCaster = _targetCaster;
  }

  public TargettingMode(TargettingModeData data)
  {
    targettingType = data.TargettingType;
    range = data.Range;
    allowMultipleTargets = data.AllowMultipleTargets;
    targetCaster = data.TargetCaster;
  }
}