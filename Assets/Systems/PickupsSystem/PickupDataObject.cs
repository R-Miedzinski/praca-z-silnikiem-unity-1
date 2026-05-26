using UnityEngine;

[CreateAssetMenu(fileName = "PickupDataObject", menuName = "Scriptable Objects/PickupDataObject")]
public class PickupDataObject : ScriptableObject
{
  public string PickupName;
  public EffectIdParamPair[] Effects;
}
