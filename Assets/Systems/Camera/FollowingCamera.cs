using System.Text.RegularExpressions;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private MonoBehaviour target;
    [SerializeField] private bool matchTargetSpeed;
    private float defaultMovementSpeed;
    private bool isStopped = false;

    private void Start()
    {
        if (matchTargetSpeed)
        {
            MatchMovementSpeedToTarget();
        }

        defaultMovementSpeed = movementSpeed;
    }

    private void Update()
    {
        if (isStopped)
            return;

        if (target != null)
        {
            if (matchTargetSpeed)
            {
                MatchMovementSpeedToTarget();
            }

            Vector3 targetPosition = target.transform.position;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("FollowingCamera has no target assigned.");
        }
    }

    public void Stop()
    {
        isStopped = true;
        movementSpeed = 0f;
    }

    public void Resume()
    {
        isStopped = false;
        movementSpeed = defaultMovementSpeed;
    }

    private void MatchMovementSpeedToTarget()
    {
        if (target is Unit unit)
        {
            movementSpeed = unit.MovementSpeed;
        }
        else
        {
            Debug.LogWarning("FollowingCamera target does not have a Unit component to match movement speed.");
        }
    }
}
