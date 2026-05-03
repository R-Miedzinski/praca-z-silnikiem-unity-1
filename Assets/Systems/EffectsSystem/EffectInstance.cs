using UnityEngine;

public class EffectInstance : MonoBehaviour
{
  public Effect Effect { get; private set; }
  public Unit Caster { get; private set; }
  public Unit Target { get; private set; }

  private float elapsedTime;

  public void Initialize(Effect effect, Unit caster, Unit target)
  {
    Effect = effect;
    Caster = caster;
    Target = target;
    elapsedTime = 0f;
  }

  private void Update()
  {
    if (Effect is ITimedEffect timedEffect)
    {
      // TODO: add correction for going over duration by partian deltaTime
      timedEffect.Tick(Target, Time.deltaTime);
      elapsedTime += Time.deltaTime;

      if (elapsedTime >= timedEffect.Duration)
      {
        timedEffect.Lift(Target);
        Destroy(this);
      }
    }
    else
    {
      Destroy(this); // Remove the effect instance immediately for non-timed effects
    }
  }
}