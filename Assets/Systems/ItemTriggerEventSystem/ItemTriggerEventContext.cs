using UnityEngine;

public class ItemTriggerEventContext
{
  public Vector3 TargettedPosition { get; private set; }
  public string ItemActivated { get; private set; }
  public Unit EnemyHit { get; private set; }
  public float ChangeValue { get; private set; }
  public object Context { get; private set; }

  public ItemTriggerEventContext(Vector3 targettedPosition = default, string itemActivated = null, Unit enemyHit = null, float changeValue = 0, object context = null)
  {
    TargettedPosition = targettedPosition;
    ItemActivated = itemActivated;
    EnemyHit = enemyHit;
    ChangeValue = changeValue;
    Context = context;
  }
}
