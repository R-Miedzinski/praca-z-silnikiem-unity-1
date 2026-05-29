using UnityEngine;

[System.Serializable]
public struct ItemEffectDataObject
{
  [Tooltip("Effects associated with this ItemeEffect. Contains the effect ID and parameters for that effect")]
  public EffectIdParamPair[] Effects;
  public ETriggerType[] EffectTriggers;
  public float Cooldown;
  [Tooltip("Number of times this effect can be used before it is depleted. -1 or leave empty for unlimited uses.")]
  public int Charges;
  public TargetingModeData TargetingMode;
}