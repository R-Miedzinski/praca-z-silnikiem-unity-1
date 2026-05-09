using UnityEngine;

abstract public class TargettingStrategy
{
  public abstract Unit[] Target(TargettingMode targettingMode, Vector3 target, Unit caster);
}