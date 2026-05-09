using UnityEngine;

[System.Serializable]
public struct ItemEffectData
{
  public string[] EffectIds;
  public ETriggerType TriggerType;
  public float Cooldown;
  public TargettingModeData TargettingMode;
  public EffectParamsData[] EffectParams;
}