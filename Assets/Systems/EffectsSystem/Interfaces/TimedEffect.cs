public interface ITimedEffect
{
  float Duration { get; set; }
  public void Lift(Unit target);
  public void Tick(Unit target, float deltaTime);
}