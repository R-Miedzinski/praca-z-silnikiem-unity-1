using UnityEngine;

[CreateAssetMenu(fileName = "EffectDefinitionsDataObject", menuName = "Scriptable Objects/EffectDefinitionsDataObject")]
public class EffectDefinitionsDataObject : ScriptableObject
{
  public EffectConfig[] EffectConfigs;
}