using UnityEngine;

[System.Serializable]
public struct ItemEffectData
{
  public string[] EffectIds;
  public ETriggerType TriggerType;
  public float Cooldown;
  public TargetingModeData TargetingMode;
  public EffectParamsData[] EffectParams;
}