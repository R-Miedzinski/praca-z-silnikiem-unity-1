using System.Collections.Generic;
using UnityEngine;

public class ItemTriggerEventSystem : MonoBehaviour
{
  public static ItemTriggerEventSystem Instance { get; private set; }

  public delegate void OnProcessTriggers(TriggerVector activeTriggers, Dictionary<ETriggerType, ItemTriggerEventContext> triggerContexts);
  public event OnProcessTriggers ProcessTriggersEvent;

  private TriggerVector activatedTriggers = new TriggerVector();
  private Dictionary<ETriggerType, ItemTriggerEventContext> triggerContexts = new Dictionary<ETriggerType, ItemTriggerEventContext>();

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
    }

    foreach (ETriggerType triggerType in System.Enum.GetValues(typeof(ETriggerType)))
    {
      activatedTriggers.Deactivate(triggerType);
      triggerContexts[triggerType] = null;
    }
  }

  private void Update()
  {
    if (activatedTriggers.HasActiveTrigger())
    {
      ProcessTriggersEvent?.Invoke(activatedTriggers, triggerContexts);
    }

    foreach (ETriggerType triggerType in System.Enum.GetValues(typeof(ETriggerType)))
    {
      activatedTriggers.Deactivate(triggerType);
      triggerContexts[triggerType] = null;
    }
  }

  public void SendTriggerEvent(ETriggerType triggerType, ItemTriggerEventContext context = default)
  {
    activatedTriggers.Activate(triggerType);
    triggerContexts.TryGetValue(triggerType, out ItemTriggerEventContext existingContext);
    if (existingContext != null)
    {
      existingContext.AppendContext(context);
    }
    else
    {
      triggerContexts[triggerType] = context;
    }
  }
}