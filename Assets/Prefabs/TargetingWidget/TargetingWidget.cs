using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingWidget : MonoBehaviour
{
    [SerializeField] private Color color = Color.red;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GameObject.Find("Target").GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

    private void Update()
    {
        // Update the widget's position to follow the mouse cursor
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = -Camera.main.transform.position.z; // Set z to camera's distance
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
