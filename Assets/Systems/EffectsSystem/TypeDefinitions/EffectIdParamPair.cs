using UnityEngine;

[System.Serializable]
public struct EffectIdParamPair
{
  [EffectIdSelection]
  public string EffectId;
  public EffectParamsData EffectParams;
}