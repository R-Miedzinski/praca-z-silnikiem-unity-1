using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
  public PickupDataObject pickupData;
  public Effect[] Effects { get { return effects; } }
  [SerializeField] private SpriteRenderer Highlight;
  private Effect[] effects;

  public void Awake()
  {
    List<Effect> newEffects = new List<Effect>();
    for (int i = 0; i < pickupData.EffectIds.Length; i++)
    {
      Effect effect = IdToEffectMap.GetEffectById(pickupData.EffectIds[i], pickupData.EffectParams[i]);
      if (effect != null)
      {
        newEffects.Add(effect);
      }
      else
      {
        Debug.LogError($"Effect with id {pickupData.EffectIds[i]} not found for pickup {pickupData.PickupName}.");
      }
    }

    effects = newEffects.ToArray();
  }

  public void Interact(PlayerCharacter player)
  {
    Debug.Log("Interacted with pickup: " + pickupData.PickupName);
    foreach (var effect in effects)
    {
      effect.ApplyEffect(player, player);
    }
    Destroy(gameObject);
  }

  public void EnableHighlight()
  {
    if (Highlight != null)
    {
      Highlight.enabled = true;
    }
  }

  public void DisableHighlight()
  {
    if (Highlight != null)
    {
      Highlight.enabled = false;
    }
  }
}