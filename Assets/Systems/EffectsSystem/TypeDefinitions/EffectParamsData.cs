using UnityEngine;

// TODO: change to array of any, handle by index on each ParameterizedEffect
[System.Serializable]
public struct EffectParamsData
{
  [Tooltip("Value for the effect. Only needed for certain effects.")]
  public float Value;
  [Tooltip("Duration for the effect. Only needed for certain effects.")]
  public float Duration;
  [Tooltip("String value for the effect. Only needed for certain effects.")]
  public string StringValue;
}