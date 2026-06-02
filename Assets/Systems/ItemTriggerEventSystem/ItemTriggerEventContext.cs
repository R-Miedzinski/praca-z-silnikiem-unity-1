using UnityEngine;

public abstract class ItemTriggerEventContext
{
  public Vector3 TargetedPosition { get; private set; }

  public ItemTriggerEventContext(Vector3 targetedPosition = default)
  {
    TargetedPosition = targetedPosition;
  }

  public abstract void AppendContext(ItemTriggerEventContext addedContext);
}
