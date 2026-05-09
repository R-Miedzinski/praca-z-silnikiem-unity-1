using UnityEngine;
using UnityEngine.InputSystem;

public class TargettingWidget : MonoBehaviour
{
    [SerializeField] Color color = Color.red;
    private SpriteRenderer spriteRenderer;
    InputAction mouseTracker;

    void Awake()
    {
        mouseTracker = new InputAction("MouseTracker", binding: "<Mouse>/position");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = color;
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
        Vector3 mousePosition = context.ReadValue<Vector2>();
        mousePosition.z = -Camera.main.transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
