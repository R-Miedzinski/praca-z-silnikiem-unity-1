public interface ITimedEffect : IPersistentEffect
{
  // TODO: move to PersistentEffect, for duration > 0, handle as timed, for duration = 0, handle as toggle
  float Duration { get; set; }
  public void Tick(Unit target, float deltaTime);
}