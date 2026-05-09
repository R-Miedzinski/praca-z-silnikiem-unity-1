public interface ITimedEffect : IPersistentEffect
{
  float Duration { get; set; }
  public void Tick(Unit target, float deltaTime);
}