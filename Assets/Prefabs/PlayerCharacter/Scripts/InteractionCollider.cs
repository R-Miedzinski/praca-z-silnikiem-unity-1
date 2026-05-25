using System.Collections.Generic;
using UnityEngine;

public class InteractionCollider : MonoBehaviour
{
    private IInteractable currentInteractable;
    private Dictionary<int, IInteractable> interactablesInRange = new Dictionary<int, IInteractable>();

    private void Update()
    {
        SetClosestAsCurrentInteractable();
    }

    public void Interact(PlayerCharacter player)
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (interactable.InteractOnContact)
            {
                interactable.Interact(PlayerCharacter.Instance);
                return;
            }

            int id = interactable.GetHashCode();
            if (!interactablesInRange.ContainsKey(id))
            {
                interactablesInRange.Add(id, interactable);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            int id = interactable.GetHashCode();
            if (interactablesInRange.ContainsKey(id))
            {
                interactablesInRange.Remove(id);

                if (currentInteractable == interactable)
                {
                    currentInteractable.DisableHighlight();
                    currentInteractable = null;
                }
            }
        }
    }

    private void SetClosestAsCurrentInteractable()
    {
        float closestDistance = float.MaxValue;
        IInteractable closestInteractable = null;

        foreach (var interactable in interactablesInRange.Values)
        {
            float distance = Vector2.Distance(transform.position, ((MonoBehaviour)interactable).transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        if (closestInteractable != null && currentInteractable != closestInteractable)
        {
            currentInteractable?.DisableHighlight();
            closestInteractable?.EnableHighlight();

            currentInteractable = closestInteractable;
        }
    }
}
