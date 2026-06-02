using UnityEngine;
using System.Collections.Generic;

public class ActivateItemEventContext : ItemTriggerEventContext
{
  public List<string> ItemsActivated { get; private set; }

  public ActivateItemEventContext(Vector3 targetedPosition = default, string itemActivated = null)
    : base(targetedPosition)
  {
    ItemsActivated = new List<string>();
    if (itemActivated != null)
    {
      ItemsActivated.Add(itemActivated);
    }
  }

  public ActivateItemEventContext(Vector3 targetedPosition = default, string[] itemsActivated = null)
    : base(targetedPosition)
  {
    ItemsActivated = new List<string>();
    if (itemsActivated != null)
    {
      ItemsActivated.AddRange(itemsActivated);
    }
  }

  public override void AppendContext(ItemTriggerEventContext addedContext)
  {
    if (addedContext is ActivateItemEventContext activateContext)
    {
      ItemsActivated.AddRange(activateContext.ItemsActivated);
    }
  }
}