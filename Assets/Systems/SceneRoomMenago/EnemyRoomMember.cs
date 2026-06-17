using System;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class EnemyRoomMember : MonoBehaviour
{
  public static event Action<EnemyRoomMember> EnemyAppeared;
  public static event Action<EnemyRoomMember> EnemyDisappeared;

  private bool isAnnounced;

  private void OnEnable()
  {
    AnnounceAppeared();
  }

  private void OnDisable()
  {
    AnnounceDisappeared();
  }

  public void AnnounceDisappeared()
  {
    if (!isAnnounced)
    {
      return;
    }

    isAnnounced = false;
    EnemyDisappeared?.Invoke(this);
  }

  private void AnnounceAppeared()
  {
    if (isAnnounced)
    {
      return;
    }

    isAnnounced = true;
    EnemyAppeared?.Invoke(this);
  }
}
