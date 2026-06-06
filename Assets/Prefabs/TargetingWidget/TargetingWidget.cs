using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingWidget : MonoBehaviour
{
    [SerializeField] private Color color = Color.red;
    private SpriteRenderer spriteRenderer;
    InputAction mouseTracker;

    void Awake()
    {
        mouseTracker = new InputAction("MouseTracker", binding: "<Mouse>/position");
        GameObject targetObject = GameObject.Find("Target");
        if (targetObject != null)
        {
            spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    void OnEnable()
    {
        mouseTracker.Enable();
        mouseTracker.performed += UpdateWidgetPosition;
    }

    void OnDisable()
    {
        mouseTracker.Disable();
        mouseTracker.performed -= UpdateWidgetPosition;
    }

    private void UpdateWidgetPosition(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (Camera.main == null)
        {
            return;
        }

        Vector3 mousePosition = context.ReadValue<Vector2>();
        mousePosition.z = -Camera.main.transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
