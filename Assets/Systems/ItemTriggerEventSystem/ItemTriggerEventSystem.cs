using UnityEngine;

public class ItemTriggerEventSystem : MonoBehaviour
{
  public static ItemTriggerEventSystem Instance { get; private set; }

  public delegate void OnMoveTrigger(object context);
  public event OnMoveTrigger MoveTriggerEvent;
  public delegate void OnHitTrigger(object context);
  public event OnHitTrigger HitTriggerEvent;
  public delegate void OnDamageTakenTrigger(object context);
  public event OnDamageTakenTrigger DamageTakenTriggerEvent;
  public delegate void OnHeatGainTrigger(object context);
  public event OnHeatGainTrigger HeatGainTriggerEvent;

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

  public void SendTriggerEvent(ETriggerType triggerType, object context = null)
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
    }
  }
  private void TriggerMove(object context)
  {
    MoveTriggerEvent?.Invoke(context);
  }

  private void TriggerHit(object context)
  {
    HitTriggerEvent?.Invoke(context);
  }

  private void TriggerDamageTaken(object context)
  {
    DamageTakenTriggerEvent?.Invoke(context);
  }

  private void TriggerHeatGain(object context)
  {
    HeatGainTriggerEvent?.Invoke(context);
  }
}