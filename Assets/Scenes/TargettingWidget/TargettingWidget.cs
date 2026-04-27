using UnityEngine;
using UnityEngine.InputSystem;

public class TargettingWidget : MonoBehaviour
{
    InputAction mouseTracker;

    void Awake()
    {
        mouseTracker = new InputAction("MouseTracker", binding: "<Mouse>/position");
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
