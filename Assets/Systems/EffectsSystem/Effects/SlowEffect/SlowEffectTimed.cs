public class SlowEffectTimed : SlowEffect, ITimedEffect
{
  public float Duration { get; set; }

  public override void SetParameters(EffectParamsData parameters)
  {
    base.SetParameters(parameters);
    Duration = parameters.Duration;
  }
}