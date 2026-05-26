using UnityEngine;

[System.Serializable]
public struct EffectConfig
{
  [Tooltip("Unique identifier for this effect. Must be unique across all effects.")]
  public string Id;
  [Tooltip("Full class name for the effect. This is used to dynamically create an instance of the effect at runtime. Must exactly match the class name of the effect implementation class.")]
  public string ClassName;
}