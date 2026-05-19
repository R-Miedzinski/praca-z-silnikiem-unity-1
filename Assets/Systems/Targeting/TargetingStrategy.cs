using UnityEngine;

abstract public class TargetingStrategy
{
  public abstract Unit[] Target(TargetingMode targetingMode, Vector3 target, Unit caster);
}