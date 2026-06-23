using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
  public PickupDataObject pickupData;
  public Effect[] Effects { get { return effects; } }
  [SerializeField] private SpriteRenderer Highlight;
  private Effect[] effects;
  public bool InteractOnContact { get { return interactOnContact; } }
  private bool interactOnContact;

  public void Awake()
  {
    interactOnContact = pickupData.InteractOnContact;
    List<Effect> newEffects = new List<Effect>();
    foreach (EffectIdParamPair effectPair in pickupData.Effects)
    {
      Effect effect = IdToEffectMap.GetEffectById(effectPair.EffectId, effectPair.EffectParams);
      if (effect != null)
      {
        effect.Id = string.Join("_", new string[] {
          effectPair.EffectId,
          pickupData.PickupName,
          System.Guid.NewGuid().ToString()
        }); // Ensure unique ID for each pickup instance

        newEffects.Add(effect);
      }
      else
      {
        Debug.LogError($"Effect with id {effectPair.EffectId} not found for pickup {pickupData.PickupName}.");
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