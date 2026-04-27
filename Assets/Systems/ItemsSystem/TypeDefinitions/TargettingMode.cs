public class TargettingMode
{
  public ETargettingType TargettingType { get { return targettingType; } }
  public float Range { get { return range; } }
  public bool AllowMultipleTargets { get { return allowMultipleTargets; } }

  private ETargettingType targettingType;
  private float range;
  private bool allowMultipleTargets;

  public TargettingMode(ETargettingType _targettingType, float _range, bool _allowMultipleTargets)
  {
    targettingType = _targettingType;
    range = _range;
    allowMultipleTargets = _allowMultipleTargets;
  }

  public TargettingMode(TargettingModeData data)
  {
    targettingType = data.TargettingType;
    range = data.Range;
    allowMultipleTargets = data.AllowMultipleTargets;
  }
}