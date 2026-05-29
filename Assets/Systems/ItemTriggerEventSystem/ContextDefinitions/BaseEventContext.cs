using UnityEngine;

public class BaseEventContext : ItemTriggerEventContext
{
  public BaseEventContext(Vector3 targetedPosition = default)
    : base(targetedPosition)
  {
  }

  public override void AppendContext(ItemTriggerEventContext addedContext)
  {
    // Base context does not have any specific data to append, so this method can be left empty or used for logging/debugging purposes.
  }
}