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
  public delegate void OnRoomEnterTrigger(object context);
  public event OnRoomEnterTrigger RoomEnterTriggerEvent;

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

  public void SendTriggerEvent(EPassiveTrigger triggerType, object context = null)
  {
    switch (triggerType)
    {
      case EPassiveTrigger.OnMove:
        TriggerMove(context);
        break;
      case EPassiveTrigger.OnHit:
        TriggerHit(context);
        break;
      case EPassiveTrigger.OnDamageTaken:
        TriggerDamageTaken(context);
        break;
      case EPassiveTrigger.OnRoomEnter:
        TriggerRoomEnter(context);
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

  private void TriggerRoomEnter(object context)
  {
    RoomEnterTriggerEvent?.Invoke(context);
  }
}