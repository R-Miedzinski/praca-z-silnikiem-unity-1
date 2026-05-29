using UnityEngine;

public class TakeDamageEventContext : ItemTriggerEventContext
{
  public float ChangeValue { get; private set; }

  public TakeDamageEventContext(Vector3 targetedPosition = default, float changeValue = 0)
    : base(targetedPosition)
  {
    ChangeValue = changeValue;
  }

  public override void AppendContext(ItemTriggerEventContext addedContext)
  {
    if (addedContext is TakeDamageEventContext damageContext)
    {
      ChangeValue += damageContext.ChangeValue;
    }
  }
}