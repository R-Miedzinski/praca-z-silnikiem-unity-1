public interface IPersistentEffect
{
  float Duration { get; set; }
  public void Lift(Unit target);
  public void Tick(Unit target, float deltaTime);
}