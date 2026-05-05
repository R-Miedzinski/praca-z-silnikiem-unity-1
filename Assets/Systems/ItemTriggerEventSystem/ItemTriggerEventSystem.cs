using UnityEngine;

public class ItemTriggerEventSystem : MonoBehaviour
{
  public static ItemTriggerEventSystem Instance { get; private set; }

  public delegate void OnMoveTrigger(ItemTriggerEventContext context);
  public event OnMoveTrigger MoveTriggerEvent;
  public delegate void OnHitTrigger(ItemTriggerEventContext context);
  public event OnHitTrigger HitTriggerEvent;
  public delegate void OnDamageTakenTrigger(ItemTriggerEventContext context);
  public event OnDamageTakenTrigger DamageTakenTriggerEvent;
  public delegate void OnHeatGainTrigger(ItemTriggerEventContext context);
  public event OnHeatGainTrigger HeatGainTriggerEvent;
  public delegate void OnActive1Trigger(ItemTriggerEventContext context);
  public event OnActive1Trigger Active1TriggerEvent;
  public delegate void OnActive2Trigger(ItemTriggerEventContext context);
  public event OnActive2Trigger Active2TriggerEvent;
  public delegate void OnActive3Trigger(ItemTriggerEventContext context);
  public event OnActive3Trigger Active3TriggerEvent;

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
  }

  public void SendTriggerEvent(ETriggerType triggerType, ItemTriggerEventContext context = default)
  {
    switch (triggerType)
    {
      case ETriggerType.OnMove:
        TriggerMove(context);
        break;
      case ETriggerType.OnHit:
        TriggerHit(context);
        break;
      case ETriggerType.OnDamageTaken:
        TriggerDamageTaken(context);
        break;
      case ETriggerType.OnHeatGain:
        TriggerHeatGain(context);
        break;
      case ETriggerType.Active1:
        TriggerActive1(context);
        break;
      case ETriggerType.Active2:
        TriggerActive2(context);
        break;
      case ETriggerType.Active3:
        TriggerActive3(context);
        break;
    }
  }
  private void TriggerMove(ItemTriggerEventContext context)
  {
    MoveTriggerEvent?.Invoke(context);
  }

  private void TriggerHit(ItemTriggerEventContext context)
  {
    HitTriggerEvent?.Invoke(context);
  }

  private void TriggerDamageTaken(ItemTriggerEventContext context)
  {
    DamageTakenTriggerEvent?.Invoke(context);
  }

  private void TriggerHeatGain(ItemTriggerEventContext context)
  {
    HeatGainTriggerEvent?.Invoke(context);
  }

  private void TriggerActive1(ItemTriggerEventContext context)
  {
    Active1TriggerEvent?.Invoke(context);
  }

  private void TriggerActive2(ItemTriggerEventContext context)
  {
    Active2TriggerEvent?.Invoke(context);
  }

  private void TriggerActive3(ItemTriggerEventContext context)
  {
    Active3TriggerEvent?.Invoke(context);
  }
}