using UnityEngine;
using System.Collections.Generic;

public class MoveEventContext : ItemTriggerEventContext
{
  public float ChangeValue { get; private set; }

  public MoveEventContext(Vector3 targetedPosition = default, float changeValue = 0)
    : base(targetedPosition)
  {
    ChangeValue = changeValue;
  }

  public override void AppendContext(ItemTriggerEventContext addedContext)
  {
    if (addedContext is MoveEventContext moveContext)
    {
      ChangeValue += moveContext.ChangeValue;
    }
  }
}