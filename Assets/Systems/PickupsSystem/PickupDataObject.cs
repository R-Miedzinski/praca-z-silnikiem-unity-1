using UnityEngine;

[CreateAssetMenu(fileName = "PickupDataObject", menuName = "Scriptable Objects/PickupDataObject")]
public class PickupDataObject : ScriptableObject
{
  public string PickupName;

  [EffectIdSelection]
  public string[] EffectIds;
  public EffectParamsData[] EffectParams;
}
