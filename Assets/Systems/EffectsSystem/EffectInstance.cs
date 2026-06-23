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
    EffectApplicationTracker.ReportTarget(target);
  }

  private void Update()
  {
    if (Effect is IPersistentEffect persistentEffect)
    {
      // TODO: add correction for going over duration by partial deltaTime
      persistentEffect.Tick(Target, Time.deltaTime);

      if (persistentEffect.Duration > 0)
      {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= persistentEffect.Duration)
        {
          persistentEffect.Lift(Target);
          Destroy(this);
        }
      }
    }
    else if (Effect is not IPersistentEffect)
    {
      Destroy(this); // Remove the effect instance immediately for non-timed effects
    }
  }
}