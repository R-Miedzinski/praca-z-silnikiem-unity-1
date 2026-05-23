using UnityEngine;

public class PlayerCollisionBlocker : MonoBehaviour
{
    [SerializeField] private Vector2 colliderSize = new Vector2(0.8f, 0.8f);
    [SerializeField] private Vector2 colliderOffset = Vector2.zero;
    [SerializeField] private LayerMask blockingLayers = Physics2D.DefaultRaycastLayers;
    [SerializeField] private bool slideAlongObstacles = true;

    private readonly Collider2D[] overlapResults = new Collider2D[8];
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        contactFilter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = blockingLayers,
            useTriggers = false
        };
    }

    public Vector2 GetAllowedMovement(Vector2 desiredMovement)
    {
        if (desiredMovement == Vector2.zero)
        {
            return Vector2.zero;
        }

        if (CanMove(desiredMovement))
        {
            return desiredMovement;
        }

        if (!slideAlongObstacles)
        {
            return Vector2.zero;
        }

        Vector2 allowedMovement = Vector2.zero;
        Vector2 horizontalMovement = new Vector2(desiredMovement.x, 0f);
        Vector2 verticalMovement = new Vector2(0f, desiredMovement.y);

        if (CanMove(horizontalMovement))
        {
            allowedMovement += horizontalMovement;
        }

        if (CanMove(verticalMovement))
        {
            allowedMovement += verticalMovement;
        }

        return allowedMovement;
    }

    private bool CanMove(Vector2 movement)
    {
        if (movement == Vector2.zero)
        {
            return true;
        }

        Vector2 targetPosition = (Vector2)transform.position + colliderOffset + movement;
        int overlapCount = Physics2D.OverlapBox(targetPosition, colliderSize, 0f, contactFilter, overlapResults);

        for (int i = 0; i < overlapCount; i++)
        {
            Collider2D hit = overlapResults[i];

            if (hit != null && hit.transform != transform && !hit.transform.IsChildOf(transform))
            {
                return false;
            }
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + (Vector3)colliderOffset, colliderSize);
    }
}
