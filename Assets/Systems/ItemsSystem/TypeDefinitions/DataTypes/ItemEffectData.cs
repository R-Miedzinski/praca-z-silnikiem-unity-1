using UnityEngine;

[System.Serializable]
public struct ItemEffectData
{
  public string[] EffectIds;
  public ETriggerType[] EffectTriggers;
  public float Cooldown;
  public TargetingModeData TargetingMode;
  public int Charges;
  public string[][] EffectParams;
  public bool IsTogglable;
}