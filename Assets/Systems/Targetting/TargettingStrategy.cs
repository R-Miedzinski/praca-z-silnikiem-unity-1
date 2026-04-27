using UnityEngine;

abstract public class TargettingStrategy
{
  public abstract Unit[] Target(TargettingMode targettingMode, Vector3 target, Vector3 origin);
}