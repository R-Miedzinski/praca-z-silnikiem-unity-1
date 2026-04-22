public class TargettingMode
{
  public ETargettingType TargettingType { get { return targettingType; } }
  public float Range { get { return range; } }
  public bool AllowMultipleTargets { get { return allowMultipleTargets; } }

  private ETargettingType targettingType;
  private float range;
  private bool allowMultipleTargets;

  public TargettingMode(ETargettingType targettingType, float range, bool allowMultipleTargets)
  {
    this.targettingType = targettingType;
    this.range = range;
    this.allowMultipleTargets = allowMultipleTargets;
  }

  public TargettingMode(TargettingModeData data)
  {
    this.targettingType = data.TargettingType;
    this.range = data.Range;
    this.allowMultipleTargets = data.AllowMultipleTargets;
  }
}