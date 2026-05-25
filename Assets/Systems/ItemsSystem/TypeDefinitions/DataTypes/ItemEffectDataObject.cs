using UnityEngine;

[System.Serializable]
public struct ItemEffectDataObject
{
  [Tooltip("Effects associated with this ItemeEffect. Contains the effect ID and parameters for that effect")]
  public EffectIdParamPair[] Effects;
  public ETriggerType TriggerType;
  public float Cooldown;
  public TargetingModeData TargetingMode;
}