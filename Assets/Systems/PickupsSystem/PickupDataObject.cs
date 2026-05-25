using UnityEngine;

[CreateAssetMenu(fileName = "PickupDataObject", menuName = "Pickups/PickupDataObject")]
public class PickupDataObject : ScriptableObject
{
  public string PickupName;
  public EffectIdParamPair[] Effects;
  public bool InteractOnContact;
}
