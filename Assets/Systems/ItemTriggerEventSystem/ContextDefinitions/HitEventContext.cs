using UnityEngine;
using System.Collections.Generic;

public class HitEventContext : ItemTriggerEventContext
{
  public Dictionary<Unit, float> EnemyHit { get; private set; }

  public HitEventContext(Vector3 targetedPosition = default, Unit enemyHit = null, float changeValue = 0)
    : base(targetedPosition)
  {
    EnemyHit = new Dictionary<Unit, float>();
    if (enemyHit != null)
    {
      EnemyHit[enemyHit] = changeValue;
    }
  }

  public HitEventContext(Vector3 targetedPosition = default, Unit[] enemyHit = null, float[] changeValue = null)
    : base(targetedPosition)
  {
    EnemyHit = new Dictionary<Unit, float>();
    if (enemyHit != null && changeValue != null)
    {
      for (int i = 0; i < enemyHit.Length && i < changeValue.Length; i++)
      {
        EnemyHit[enemyHit[i]] = changeValue[i];
      }
    }
  }

  public override void AppendContext(ItemTriggerEventContext addedContext)
  {
    if (addedContext is HitEventContext hitContext)
    {
      foreach (var kvp in hitContext.EnemyHit)
      {
        if (EnemyHit.ContainsKey(kvp.Key))
        {
          EnemyHit[kvp.Key] += kvp.Value;
        }
        else
        {
          EnemyHit[kvp.Key] = kvp.Value;
        }
      }
    }
  }
}